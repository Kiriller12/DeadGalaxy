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
using Raylib_cs;

namespace DeadGalaxy.Core.Entities
{
    /// <summary>
    /// Game point light entity
    /// </summary>
    internal class PointLightEntity : BaseEntity
    {
        /// <summary>
        /// Game point light entity
        /// </summary>
        /// <param name="name">MainCamera name</param>
        /// <param name="properties">MainCamera properties</param>
        public PointLightEntity(string name, Dictionary<string, object> properties) : base(name)
        {
            LoadProperties(properties);
        }

        /// <summary>
        /// Light position
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Light radius
        /// </summary>
        public float Radius { get; set; }

        /// <summary>
        /// Light intensity
        /// </summary>
        public float Intensity { get; set; }

        /// <summary>
        /// Light color
        /// </summary>
        public Color Color { get; set; }
    }
}
