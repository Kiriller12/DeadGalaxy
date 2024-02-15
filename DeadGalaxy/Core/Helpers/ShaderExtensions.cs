/*
=========================================================

DeadGalaxy project

All right reserved
Kabanov Kirill (Kiriller12) © 2024

Licensed under GNU General Public License v3.0
Full license text available in LICENCE file

=========================================================
*/

using Raylib_cs;

namespace DeadGalaxy.Core.Helpers
{
    /// <summary>
    /// Shader extension methods
    /// </summary>
    internal static class ShaderExtensions
    {
        /// <summary>
        /// Sets shader location
        /// </summary>
        /// <param name="shader">Target shader</param>
        /// <param name="index">Location index</param>
        /// <param name="uniformName">Shader uniform name</param>
        public static unsafe void SetLocation(this Shader shader, ShaderLocationIndex index, string uniformName)
        {
            shader.Locs[(int)index] = Raylib.GetShaderLocation(shader, uniformName);
        }
    }
}
