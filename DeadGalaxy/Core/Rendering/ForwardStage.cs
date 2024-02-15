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
using Raylib_cs;

namespace DeadGalaxy.Core.Rendering
{
    /// <summary>
    /// Forward rendering stage
    /// </summary>
    internal class ForwardStage : IRenderingStage
    {
        /// <summary>
        /// Geometry rendering stage
        /// </summary>
        public ForwardStage()
        {
            // Loading shader
            var defaultMaterial = Raylib.LoadMaterialDefault();
            Shader = defaultMaterial.Shader;
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

            // Enabling camera rendering mode
            scene.MainCamera.BeginCameraMode();

            // Rendering scene entities
            scene.Render();

            // Disabling camera rendering mode
            scene.MainCamera.EndCameraMode();

            return new Dictionary<string, Texture2D>();
        }
    }
}
