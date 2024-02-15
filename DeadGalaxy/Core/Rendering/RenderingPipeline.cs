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
using Raylib_cs;

namespace DeadGalaxy.Core.Rendering
{
    /// <summary>
    /// Game rendering pipeline
    /// </summary>
    internal class RenderingPipeline
    {
        private readonly List<IRenderingStage> _stages;

        /// <summary>
        /// Game rendering pipeline
        /// </summary>
        /// <param name="stages">Rendering stages</param>
        private RenderingPipeline(List<IRenderingStage> stages)
        {
            _stages = stages;

            DefaultShader = _stages[0].Shader;
        }

        /// <summary>
        /// Rendering pipeline instance
        /// </summary>
        public static RenderingPipeline? Instance { get; private set; }

        /// <summary>
        /// Default shader for rendering objects
        /// </summary>
        public Shader DefaultShader { get; }

        /// <summary>
        /// Initializes rendering pipeline
        /// </summary>
        /// <param name="stages">Rendering stages</param>
        public static void Init(List<IRenderingStage> stages)
        {
            if (Instance != null)
            {
                Raylib.TraceLog(TraceLogLevel.Warning, "[Rendering pipeline]: Couldn't create rendering pipeline because it is already created!");

                return;
            }

            Instance = new RenderingPipeline(stages);
        }

        /// <summary>
        /// Processes one frame in the pipeline
        /// </summary>
        public void Render()
        {
            // Processing all stages
            var values = new Dictionary<string, Texture2D>();
            foreach (var stage in _stages)
            {
                var outputs = stage.Render(values);
                foreach (var (name, value) in outputs)
                {
                    values.Add(name, value);
                }
            }
        }
    }
}
