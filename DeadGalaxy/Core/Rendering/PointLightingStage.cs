/*
=========================================================

DeadGalaxy project

All rights reserved
Kabanov Kirill (Kiriller12) © 2024

Licensed under GNU General Public License v3.0
Full license text available in LICENCE file

=========================================================
*/

using System.Collections.Generic;
using DeadGalaxy.Core.Entities;
using DeadGalaxy.Core.Helpers;
using Raylib_cs;

namespace DeadGalaxy.Core.Rendering
{
    /// <summary>
    /// Point lighting rendering stage
    /// </summary>
    internal class PointLightingStage : IRenderingStage
    {
        private const int MaxLights = 10;

        private readonly RenderBuffer _buffer;
        private readonly Shader _shader;

        private readonly int _viewPosLocation;
        private readonly int _screenSizeLocation;
        private readonly int _lightsCountLocation;
        private readonly List<PointLightShaderLocations> _lightLocations = [];

        private Model _lightModel;

        /// <summary>
        /// Point lighting rendering stage
        /// </summary>
        public PointLightingStage()
        {
            // Loading shader
            _shader = Raylib.LoadShader("Data/Shaders/pointLighting.vert", "Data/Shaders/pointLighting.frag");

            // Setting shader locations
            _shader.SetLocation(ShaderLocationIndex.MapDiffuse, "diffuseTexture");
            _shader.SetLocation(ShaderLocationIndex.MapNormal, "normalTexture");
            _shader.SetLocation(ShaderLocationIndex.MapSpecular, "specularTexture");
            _shader.SetLocation(ShaderLocationIndex.MapHeight, "depthTexture");

            _viewPosLocation = Raylib.GetShaderLocation(_shader, "viewPos");
            _screenSizeLocation = Raylib.GetShaderLocation(_shader, "screenSize");
            _lightsCountLocation = Raylib.GetShaderLocation(_shader, "lightsCount");

            // Setting light locations
            for (var i = 0; i < MaxLights; i++)
            {
                var position = Raylib.GetShaderLocation(_shader, $"lights[{i}].position");
                var radius = Raylib.GetShaderLocation(_shader, $"lights[{i}].radius");
                var intensity = Raylib.GetShaderLocation(_shader, $"lights[{i}].intensity");
                var color = Raylib.GetShaderLocation(_shader, $"lights[{i}].color");

                _lightLocations.Add(new PointLightShaderLocations(position, radius, intensity, color));
            }

            // Setting render buffer
            _buffer = new RenderBuffer();
            _buffer.RegisterTexture("lighting", PixelFormat.UncompressedR8G8B8);

            // Creating base point light model
            var lightMesh = Raylib.GenMeshSphere(1.0f, 32, 32);
            _lightModel = Raylib.LoadModelFromMesh(lightMesh);
            Raylib.SetMaterialShader(ref _lightModel, 0, ref _shader);
        }

        /// <summary>
        /// Stage shader
        /// </summary>
        public Shader Shader => _shader;

        /// <summary>
        /// Processes one frame for stage
        /// </summary>
        /// <param name="values">Input values</param>
        public IReadOnlyDictionary<string, Texture2D> Render(IReadOnlyDictionary<string, Texture2D> values)
        {
            // Retrieving main scene
            var scene = Scene.Main;
            if (scene == null)
            {
                return new Dictionary<string, Texture2D>();
            }

            // Updating shader values
            float[] cameraPos = [scene.MainCamera.Position.X, scene.MainCamera.Position.Y, scene.MainCamera.Position.Z];
            Raylib.SetShaderValue(Shader, _viewPosLocation, cameraPos, ShaderUniformDataType.Vec3);

            float[] screenSize = [ Raylib.GetScreenWidth(), Raylib.GetScreenHeight() ];
            Raylib.SetShaderValue(_shader, _screenSizeLocation, screenSize, ShaderUniformDataType.Vec2);

            var diffuse = values["diffuse"];
            Raylib.SetMaterialTexture(ref _lightModel, 0, MaterialMapIndex.Diffuse, ref diffuse);

            var normal = values["normal"];
            Raylib.SetMaterialTexture(ref _lightModel, 0, MaterialMapIndex.Normal, ref normal);

            var specular = values["specular"];
            Raylib.SetMaterialTexture(ref _lightModel, 0, MaterialMapIndex.Specular, ref specular);

            var depth = values["depth"];
            Raylib.SetMaterialTexture(ref _lightModel, 0, MaterialMapIndex.Height, ref depth);

            // Setting shader lights values
            var lights = new List<PointLightEntity>();
            foreach (var light in scene.PointLights)
            {
                // If there are too many light, skipping them
                if (lights.Count == MaxLights)
                {
                    break;
                }

                var lightLocations = _lightLocations[lights.Count];

                float[] pos = [ light.Position.X, light.Position.Y, light.Position.Z ];
                Raylib.SetShaderValue(_shader, lightLocations.Position, pos, ShaderUniformDataType.Vec3);

                Raylib.SetShaderValue(_shader, lightLocations.Radius, light.Radius, ShaderUniformDataType.Float);
                Raylib.SetShaderValue(_shader, lightLocations.Intensity, light.Intensity, ShaderUniformDataType.Float);

                var colorValues = Raylib.ColorNormalize(light.Color);
                float[] color = [ colorValues.X, colorValues.Y, colorValues.Z ];
                Raylib.SetShaderValue(_shader, lightLocations.Color, color, ShaderUniformDataType.Vec3);

                lights.Add(light);
            }

            // Setting shader lights count value
            Raylib.SetShaderValue(_shader, _lightsCountLocation, lights.Count, ShaderUniformDataType.Int);

            // Enabling buffer rendering mode
            _buffer.BeginBufferMode();
            scene.MainCamera.BeginCameraMode();
            Rlgl.DisableBackfaceCulling();

            // Rendering point light meshes
            foreach (var light in lights)
            {
                Raylib.DrawModel(_lightModel, light.Position, light.Radius, Color.White);
            }

            // Disabling buffer rendering mode
            Rlgl.EnableBackfaceCulling();
            scene.MainCamera.EndCameraMode();
            _buffer.EndBufferMode();

            return _buffer.Textures;
        }
    }
}
