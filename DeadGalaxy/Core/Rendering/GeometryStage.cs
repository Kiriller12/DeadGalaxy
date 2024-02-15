/*
=========================================================

DeadGalaxy project

All right reserved
Kabanov Kirill (Kiriller12) © 2024

Licensed under GNU General Public License v3.0
Full license text available in LICENCE file

=========================================================
*/

using System.Collections.Generic;
using DeadGalaxy.Core.Helpers;
using Raylib_cs;

namespace DeadGalaxy.Core.Rendering
{
    /// <summary>
    /// Geometry rendering stage
    /// </summary>
    internal class GeometryStage : IRenderingStage
    {
        private readonly RenderBuffer _buffer;
        private readonly int _viewPosLocation;

        /// <summary>
        /// Geometry rendering stage
        /// </summary>
        public GeometryStage()
        {
            // Loading shader
            Shader = Raylib.LoadShader("Data/Shader/Geometry.vert", "Data/Shader/Geometry.frag");

            // Setting shader locations
            Shader.SetLocation(ShaderLocationIndex.MapDiffuse,"diffuseTexture");
            Shader.SetLocation(ShaderLocationIndex.MapNormal, "normalTexture");
            Shader.SetLocation(ShaderLocationIndex.MapSpecular, "specularTexture");

            _viewPosLocation = Raylib.GetShaderLocation(Shader, "viewPos");

            // Setting render buffer
            _buffer = new RenderBuffer();

            _buffer.RegisterTexture("diffuse", PixelFormat.UncompressedR8G8B8A8);
            _buffer.RegisterTexture("normal", PixelFormat.UncompressedR8G8B8);
            _buffer.RegisterTexture("specular", PixelFormat.UncompressedR8G8B8);
            _buffer.RegisterTexture("depth", PixelFormat.UncompressedR32);
        }

        /// <summary>
        /// Stage shader
        /// </summary>
        public Shader Shader { get; }

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
            float[] cameraPos = [ scene.MainCamera.Position.X, scene.MainCamera.Position.Y, scene.MainCamera.Position.Z ];
            Raylib.SetShaderValue(Shader, _viewPosLocation, cameraPos, ShaderUniformDataType.Vec3);

            // Enabling buffer rendering mode
            _buffer.BeginBufferMode();
            scene.MainCamera.BeginCameraMode();

            // Rendering scene entities
            scene.Render();

            // Disabling buffer rendering mode
            scene.MainCamera.EndCameraMode();
            _buffer.EndBufferMode();

            return _buffer.Textures;
        }
    }
}
