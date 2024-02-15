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
using System.Numerics;
using Raylib_cs;

namespace DeadGalaxy.Core.Rendering
{
    /// <summary>
    /// Final rendering stage
    /// </summary>
    internal class FinalStage : IRenderingStage
    {
        private readonly int _diffuseLocation;
        private readonly int _lightingLocation;

        /// <summary>
        /// Stage shader
        /// </summary>
        public Shader Shader { get; }

        /// <summary>
        /// Финальная стадия отрисовки
        /// </summary>
        public FinalStage()
        {
            // Loading shader
            Shader = Raylib.LoadShader(null, "Data/Shader/Final.frag");

            // Setting shader locations
            _diffuseLocation = Raylib.GetShaderLocation(Shader, "diffuseTexture");
            _lightingLocation = Raylib.GetShaderLocation(Shader, "lightingTexture");
        }

        /// <summary>
        /// Processes one frame for stage
        /// </summary>
        /// <param name="values">Input values</param>
        public IReadOnlyDictionary<string, Texture2D> Render(IReadOnlyDictionary<string, Texture2D> values)
        {
            // Clearing screen
            Raylib.ClearBackground(Color.Blank);

            // Enabling shader rendering move
            Raylib.BeginShaderMode(Shader);

            // Updating shader values
            var diffuse = values["diffuse"];

            Raylib.SetShaderValueTexture(Shader, _diffuseLocation, diffuse);
            Raylib.SetShaderValueTexture(Shader, _lightingLocation, values["lighting"]);

            // Drawing texture to apply shader to
            Raylib.DrawTextureRec(diffuse, new Rectangle(0, 0, diffuse.Width, -diffuse.Height), Vector2.Zero, Color.White);

            // Ending shader rendering move
            Raylib.EndShaderMode();

            return new Dictionary<string, Texture2D>();
        }
    }
}
