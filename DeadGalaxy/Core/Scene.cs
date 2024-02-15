/*
=========================================================

DeadGalaxy project

All rights reserved
Kabanov Kirill (Kiriller12) © 2024

Licensed under GNU General Public License v3.0
Full license text available in LICENCE file

=========================================================
*/

using DeadGalaxy.Core.Entities;
using Raylib_cs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DeadGalaxy.Core.Helpers;
using DeadGalaxy.Core.Resources;

namespace DeadGalaxy.Core
{
    /// <summary>
    /// Game scene
    /// </summary>
    internal class Scene
    {
        private readonly Dictionary<string, TextureResource> _textureResources = [];
        private readonly Dictionary<string, ModelResource> _modelResources = [];

        private readonly HashSet<IRenderable> _renderableEntities = [];
        private readonly HashSet<IUpdatable> _updatableEntities = [];

        private readonly HashSet<PointLightEntity> _pointLights = [];
        private readonly HashSet<CameraEntity> _cameras = [];

        /// <summary>
        /// Main scene
        /// </summary>
        public static Scene? Main { get; private set; }

        /// <summary>
        /// Base path to scene directory
        /// </summary>
        public string BasePath { get; }

        /// <summary>
        /// Texture resources
        /// </summary>
        public IReadOnlyDictionary<string, TextureResource> TextureResources => _textureResources;

        /// <summary>
        /// Point light entities
        /// </summary>
        public IReadOnlySet<PointLightEntity> PointLights => _pointLights;

        /// <summary>
        /// MainCamera entities
        /// </summary>
        public IReadOnlySet<CameraEntity> Cameras => _cameras;

        /// <summary>
        /// Scene main camera
        /// </summary>
        public CameraEntity MainCamera { get; }

        /// <summary>
        /// Game scene
        /// </summary>
        /// <param name="metadata">Scene metadata</param>
        /// <param name="basePath">Scene base path</param>
        private Scene(SceneMetadata metadata, string basePath)
        {
            BasePath = basePath;

            // Loading scene resources
            LoadResources(metadata);

            // Loading scene entities
            LoadEntities(metadata);

            // Setting main camera
            var mainCamera = _cameras.FirstOrDefault(x => x.IsMain);
            if (mainCamera == null)
            {
                // Creating new main camera
                mainCamera = new CameraEntity("MainCamera");

                _cameras.Add(mainCamera);
                _updatableEntities.Add(mainCamera);
            }

            MainCamera = mainCamera;
        }

        /// <summary>
        /// Loads scene as main
        /// </summary>
        /// <param name="sceneName">Scene name</param>
        public static void Load(string sceneName)
        {
            var basePath = $"Data/Lvl/{sceneName}";

            // Loading scene metadata
            var metadata = SceneMetadata.Load($"{basePath}/Metadata.json");
            if (metadata == null)
            {
                Raylib.TraceLog(TraceLogLevel.Error, $"[Scene]: Couldn't load game scene \"{sceneName}\". Unable to load metadata!");

                return;
            }

            // Loading new scene data
            Main?.Clear();
            Main = new Scene(metadata, basePath);
        }

        /// <summary>
        /// Updates entities game logic
        /// </summary>
        /// <param name="dt">Time since last frame update</param>
        public void Update(float dt)
        {
            foreach (var entity in _updatableEntities)
            {
                entity.Update(dt);
            }
        }

        /// <summary>
        /// Renders entities
        /// </summary>
        public void Render()
        {
            foreach (var entity in _renderableEntities)
            {
                entity.Render();
            }
        }

        /// <summary>
        /// Cleans resources
        /// </summary>
        public void Clear()
        {
            _pointLights.ClearWithDispose();
            _cameras.ClearWithDispose();

            _renderableEntities.ClearWithDispose();
            _updatableEntities.ClearWithDispose();

            _textureResources.ClearWithDispose();
            _modelResources.ClearWithDispose();
        }

        /// <summary>
        /// Loads scene resources by its metadata
        /// </summary>
        /// <param name="metadata">Scene metadata</param>
        private void LoadResources(SceneMetadata metadata)
        {
            // Loading fallback texture
            var fallbackTexture = Raylib.LoadTexture("Data/Common/Fallback.png");
            var fallbackTextureResource = new TextureResource("fallback", fallbackTexture);
            _textureResources.Add(fallbackTextureResource.Name, fallbackTextureResource);

            // Loading fallback model
            var fallbackMesh = Raylib.GenMeshCube(0.1f, 0.1f, 0.1f);
            var fallbackModel = Raylib.LoadModelFromMesh(fallbackMesh);
            var fallbackModelResource = new ModelResource("fallback", fallbackModel);
            fallbackModelResource.SetMaterialTexture(0, MaterialMapIndex.Diffuse, fallbackTexture);
            _modelResources.Add(fallbackModelResource.Name, fallbackModelResource);

            // Loading scene resources
            foreach (var resource in metadata.Resources)
            {
                // Checking resource data
                if (string.IsNullOrWhiteSpace(resource.Name))
                {
                    Raylib.TraceLog(TraceLogLevel.Error, "[Scene]: Couldn't load resource. Bad resource name!");

                    continue;
                }

                var filePath = $"{BasePath}/{resource.FilePath}";
                if (!resource.Generated && (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath)))
                {
                    Raylib.TraceLog(TraceLogLevel.Error, $"[Scene]: Couldn't load resource \"{resource.Name}\". File not found!");

                    continue;
                }

                if (resource.Generated && string.IsNullOrWhiteSpace(resource.AlgorithmType))
                {
                    Raylib.TraceLog(TraceLogLevel.Error,
                        $"[Scene]: Couldn't load resource \"{resource.Name}\". Generation algorithm type is undefined!");

                    continue;
                }

                // Loading resource by its type
                switch (resource.Type)
                {
                    case ResourceType.Texture:
                        if (resource.Generated)
                        {
                            // TODO Support texture generation algorithms

                            break;
                        }

                        var texture = Raylib.LoadTexture(filePath);
                        var textureResource = new TextureResource(resource.Name, texture);

                        _textureResources.Add(textureResource.Name, textureResource);

                        break;
                    case ResourceType.Model:
                        var model = resource.Generated
                            ? RaylibHelpers.GenerateModelByAlgorithm(resource.AlgorithmType)
                            : Raylib.LoadModel(filePath);

                        if (model.MeshCount == 0)
                        {
                            Raylib.TraceLog(TraceLogLevel.Error,
                                $"[Scene]: Couldn't load resource \"{resource.Name}\". Model is corrupted!");

                            break;
                        }

                        var modelResource = new ModelResource(resource.Name, model);
                        _modelResources.Add(modelResource.Name, modelResource);

                        break;
                    default:
                        Raylib.TraceLog(TraceLogLevel.Error,
                            $"[Scene]: Couldn't load resource \"{resource.Name}\". Unknown resource type!");

                        break;
                }
            }
        }

        /// <summary>
        /// Loads scene entities by its metadata
        /// </summary>
        /// <param name="metadata">Scene metadata</param>
        private void LoadEntities(SceneMetadata metadata)
        {
            foreach (var entity in metadata.Entities)
            {
                // Checking resource data
                if (string.IsNullOrWhiteSpace(entity.Name))
                {
                    Raylib.TraceLog(TraceLogLevel.Error, "[Scene]: Couldn't load entity resource. Bad entity name!");

                    continue;
                }

                // Resolving properties
                foreach (var (name, value) in entity.Properties)
                {
                    var resolvableProperty = value.Convert<ResolveProperty>();
                    if (resolvableProperty == null || string.IsNullOrEmpty(resolvableProperty.Name))
                    {
                        continue;
                    }

                    switch (resolvableProperty.ResolveType)
                    {
                        case ResourceType.Texture:
                            if (_textureResources.TryGetValue(resolvableProperty.Name, out var textureResource))
                            {
                                entity.Properties[name] = textureResource;

                                break;
                            }

                            entity.Properties[name] = _textureResources["fallback"];

                            Raylib.TraceLog(TraceLogLevel.Error,
                                $"[Scene]: Couldn't resolve property \"{resolvableProperty.Name}\".Couldn't find target texture!");

                            break;
                        case ResourceType.Model:
                            if (_modelResources.TryGetValue(resolvableProperty.Name, out var modelResource))
                            {
                                entity.Properties[name] = modelResource;

                                break;
                            }

                            entity.Properties[name] = _modelResources["fallback"];

                            Raylib.TraceLog(TraceLogLevel.Error,
                                $"[Scene]: Couldn't resolve property \"{resolvableProperty.Name}\".Couldn't find target model!");

                            break;
                        default:
                            Raylib.TraceLog(TraceLogLevel.Error,
                                $"[Scene]: Couldn't resolve property \"{resolvableProperty.Name}\". Unknown resource type!");

                            break;
                    }
                }

                // Loading entity by its type
                switch (entity.Type)
                {
                    case EntityResourceType.Static:
                        var staticEntity = new StaticEntity(entity.Name, entity.Properties);
                        _renderableEntities.Add(staticEntity);

                        break;
                    case EntityResourceType.Dynamic:
                        var dynamicEntity = new DynamicEntity(entity.Name, entity.Properties);
                        _renderableEntities.Add(dynamicEntity);

                        break;
                    case EntityResourceType.World:
                        var world = new WorldEntity(entity.Name, entity.Properties, BasePath);
                        _renderableEntities.Add(world);

                        break;
                    case EntityResourceType.Camera:
                        var camera = new CameraEntity(entity.Name, entity.Properties);

                        _cameras.Add(camera);
                        _updatableEntities.Add(camera);

                        break;
                    case EntityResourceType.PointLight:
                        var pointLight = new PointLightEntity(entity.Name, entity.Properties);
                        _pointLights.Add(pointLight);

                        break;
                    default:
                        Raylib.TraceLog(TraceLogLevel.Error, $"[Scene]: Couldn't load entity resource \"{entity.Name}\". Unknown entity type!");

                        break;
                }
            }
        }
    }
}
