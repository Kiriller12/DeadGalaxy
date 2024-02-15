/*
=========================================================

DeadGalaxy project

All right reserved
Kabanov Kirill (Kiriller12) © 2024

Licensed under GNU General Public License v3.0
Full license text available in LICENCE file

=========================================================
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using DeadGalaxy.Core.Helpers;
using DeadGalaxy.Core.Resources;
using Raylib_cs;

namespace DeadGalaxy.Core.Entities
{
    /// <summary>
    /// Game world entity
    /// </summary>
    internal class WorldEntity : BaseEntity, IRenderable, IDisposable
    {
        private readonly HashSet<StaticEntity> _chunks = [];

        /// <summary>
        /// Game world entity
        /// </summary>
        /// <param name="name">World name</param>
        /// <param name="properties">World properties</param>
        /// <param name="basePath">Base scene path</param>
        public WorldEntity(string name, Dictionary<string, object> properties, string basePath) : base(name)
        {
            LoadProperties(properties);

            // Loading map from texture
            var mapFile = $"{basePath}/{MapPath}";
            if (!File.Exists(mapFile))
            {
                throw new EngineException($"[WorldEntity]: Couldn't load world \"{name}\". Map file not found!");
            }

            // Loading scene chunks
            var mapImage = Raylib.LoadImage(mapFile);
            var mapChunk = LoadChunk(mapImage);

            _chunks.Add(mapChunk);
        }

        /// <summary>
        /// Entity position
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Entity rotation
        /// </summary>
        public Vector3 Rotation { get; set; }

        /// <summary>
        /// Entity scale
        /// </summary>
        public Vector3 Scale { get; set; }

        /// <summary>
        /// world chunk count
        /// </summary>
        public int ChunkCount { get; set; }

        /// <summary>
        /// world chunk size
        /// </summary>
        public int ChunkSize { get; set; }

        /// <summary>
        /// Map path
        /// </summary>
        public string? MapPath { get; set; }

        /// <summary>
        /// Diffuse texture
        /// </summary>
        public TextureResource? DiffuseTexture { get; set; }

        /// <summary>
        /// Normal texture
        /// </summary>
        public TextureResource? NormalTexture { get; set; }

        /// <summary>
        /// Specular texture
        /// </summary>
        public TextureResource? SpecularTexture { get; set; }

        /// <summary>
        /// Renders entity
        /// </summary>
        public void Render()
        {
            foreach (var chunk in _chunks)
            {
                chunk.Render();
            }
        }

        /// <summary>
        /// Clears resources
        /// </summary>
        public void Dispose()
        {
            // Clearing generated models
            foreach (var chunk in _chunks)
            {
                chunk.Model?.Dispose();
            }

            // Clear resources
            _chunks.ClearWithDispose();
        }

        /// <summary>
        /// Loads map chunk
        /// </summary>
        /// <param name="chunkImage">Map chunk structure image</param>
        private StaticEntity LoadChunk(Image chunkImage)
        {
            var mesh = Raylib.GenMeshCubicmap(chunkImage, Vector3.One);
            Raylib.GenMeshTangents(ref mesh);

            var model = Raylib.LoadModelFromMesh(mesh);
            var modelResource = new ModelResource("Chunk1", model);

            if (DiffuseTexture != null)
            {
                modelResource.SetMaterialTexture(0, MaterialMapIndex.Diffuse, DiffuseTexture.Texture);
            }

            if (NormalTexture != null)
            {
                modelResource.SetMaterialTexture(0, MaterialMapIndex.Normal, NormalTexture.Texture);
            }

            if (SpecularTexture != null)
            {
                modelResource.SetMaterialTexture(0, MaterialMapIndex.Specular, SpecularTexture.Texture);
            }

            return new StaticEntity("Chunk1", modelResource);
        }
    }
}
