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
using System.Numerics;
using System.Reflection;
using Raylib_cs;

namespace DeadGalaxy.Core.Rendering
{
    /// <summary>
    /// Final rendering stage
    /// </summary>
    internal class DebugStage : IRenderingStage
    {
        private const float RenderingDebugScale = 0.1f;

        /// <summary>
        /// Stage shader
        /// </summary>
        public Shader Shader { get; } = new();

        /// <summary>
        /// Processes one frame for stage
        /// </summary>
        /// <param name="values">Input values</param>
        public IReadOnlyDictionary<string, Texture2D> Render(IReadOnlyDictionary<string, Texture2D> values)
        {
            if (Configuration.Get<bool>("debug:fps"))
            {
                Raylib.DrawFPS(8, 8);
            }

            if (Configuration.Get<bool>("debug:rendering"))
            {
                DrawRenderingDebug(values);
            }

            if (Configuration.Get<bool>("debug:console"))
            {
                var console = Console.Instance;
                if (console != null && console.Shown)
                {
                    console.Render();
                }
            }

            // Drawing game version text
            DrawGameVersion();

            return new Dictionary<string, Texture2D>();
        }

        /// <summary>
        /// Draws rendering debug information
        /// </summary>
        /// <param name="values">Rendering pipeline values</param>
        private void DrawRenderingDebug(IReadOnlyDictionary<string, Texture2D> values)
        {
            var diffuse = values["diffuse"];

            var source = new Rectangle(0, 0, diffuse.Width, -diffuse.Height);
            var target = new Rectangle(Vector2.Zero, new Vector2(diffuse.Width, diffuse.Height) * RenderingDebugScale);
            var basePosition = new Vector2(8, -40);
            var offsetPosition = new Vector2(0, -target.Height);

            // Drawing textures from buffer
            Raylib.DrawTexturePro(diffuse, source, target, basePosition, 0, Color.White);
            Raylib.DrawTexturePro(values["normal"], source, target, basePosition + offsetPosition, 0, Color.White);
            Raylib.DrawTexturePro(values["specular"], source, target, basePosition + offsetPosition * 2, 0, Color.White);
            Raylib.DrawTexturePro(values["depth"], source, target, basePosition + offsetPosition * 3, 0, Color.White);
            Raylib.DrawTexturePro(values["lighting"], source, target, basePosition + offsetPosition * 4, 0, Color.White);
        }

        /// <summary>
        /// Draws game version text in screen left bottom corner
        /// </summary>
        private void DrawGameVersion()
        {
            var height = Raylib.GetScreenHeight();

            // Retrieving game version from assembly
            var assembly = Assembly.GetEntryAssembly();
            var assemblyVersion = assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            var version = assemblyVersion?.InformationalVersion ?? "unknown";

            Raylib.DrawText($"ver. {version}", 8, height - 28, 20, Color.Gold);
        }
    }
}
