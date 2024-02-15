/*
=========================================================

DeadGalaxy project

All rights reserved
Kabanov Kirill (Kiriller12) © 2024

Licensed under GNU General Public License v3.0
Full license text available in LICENCE file

=========================================================
*/

using System;
using DeadGalaxy.Core.Rendering;
using Raylib_cs;

namespace DeadGalaxy.Core
{
    /// <summary>
    /// Main engine class
    /// </summary>
    internal class Engine : IDisposable
    {
        /// <summary>
        /// Main engine class
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public Engine(string[] args)
        {
            // Initializing debug console
            Console.Init();

            // Initializing configuration
            Configuration.Init(args);

            // Initializing raylib
            Raylib.InitWindow(1, 1, "DeadGalaxy");
            
            // Applying loaded game config
            ApplyConfig();

            // Initializing rendering pipeline
            RenderingPipeline.Init([
                new GeometryStage(),
                new PointLightingStage(),
                new FinalStage(),
                new DebugStage()
            ]);

            // Loading init scene
            Scene.Load("Init");
        }

        /// <summary>
        /// Runs main game loop
        /// </summary>
        public void Run()
        {
            while (!Raylib.WindowShouldClose())
            {
                if (Scene.Main == null)
                {
                    continue;
                }

                // Updating scene game logic
                var dt = Raylib.GetFrameTime();
                Scene.Main.Update(dt);

                // Updating debug console logic
                Console.Instance?.Update();

                // Rendering scene
                Raylib.BeginDrawing();

                // Using rendering pipeline to render
                RenderingPipeline.Instance?.Render();

                Raylib.EndDrawing();
            }
        }

        /// <summary>
        /// Cleans resources
        /// </summary>
        public void Dispose()
        {
            Scene.Main?.Clear();
            Raylib.CloseWindow();
        }

        /// <summary>
        /// Applies loaded configuration
        /// </summary>
        private void ApplyConfig()
        {
            Configuration.CheckInitialization();

            // Applying graphics settings
            Configuration.ApplyScreenSize();
            Configuration.ApplyVsync();
            Configuration.ApplyFpsLock();
            Configuration.ApplyFullscreen();

            // Applying audio settings
            Configuration.ApplyMasterVolume();
            Configuration.ApplyMusicVolume();
            Configuration.ApplyEffectsVolume();

            // TODO Applying input settings
            // ...

            // TODO Applying game settings
            // ...

            Raylib.SetWindowFocused();
            Raylib.DisableCursor();
        }
    }
}
