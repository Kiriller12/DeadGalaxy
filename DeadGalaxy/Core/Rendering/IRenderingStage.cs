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
    /// Rendering stage
    /// </summary>
    internal interface IRenderingStage
    {
        /// <summary>
        /// Stage shader
        /// </summary>
        public Shader Shader { get; }

        /// <summary>
        /// Processes one frame for stage
        /// </summary>
        /// <param name="values">Input values</param>
        IReadOnlyDictionary<string, Texture2D> Render(IReadOnlyDictionary<string, Texture2D> values);
    }
}
