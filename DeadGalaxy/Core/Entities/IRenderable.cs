/*
=========================================================

DeadGalaxy project

All right reserved
Kabanov Kirill (Kiriller12) © 2024

Licensed under GNU General Public License v3.0
Full license text available in LICENCE file

=========================================================
*/

using System.Numerics;

namespace DeadGalaxy.Core.Entities
{
    /// <summary>
    /// Renderable entity interface
    /// </summary>
    internal interface IRenderable
    {
        /// <summary>
        /// Entity position
        /// </summary>
        public Vector3 Position { get; }

        /// <summary>
        /// Entity rotation
        /// </summary>
        public Vector3 Rotation { get; }

        /// <summary>
        /// Entity scale
        /// </summary>
        public Vector3 Scale { get; }

        /// <summary>
        /// Renders entity
        /// </summary>
        public void Render();
    }
}
