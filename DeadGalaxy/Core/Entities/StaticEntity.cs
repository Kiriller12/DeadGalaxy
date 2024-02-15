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
using DeadGalaxy.Core.Resources;

namespace DeadGalaxy.Core.Entities
{
    /// <summary>
    /// Game static entity
    /// </summary>
    internal class StaticEntity : BaseEntity, IRenderable
    {
        /// <summary>
        /// Game static entity
        /// </summary>
        /// <param name="name">Entity name</param>
        /// <param name="model">Model to render</param>
        public StaticEntity(string name, ModelResource? model) : base(name)
        {
            Model = model;
        }

        /// <summary>
        /// Game static entity
        /// </summary>
        /// <param name="name">Entity name</param>
        /// <param name="properties">Entity properties</param>
        public StaticEntity(string name, Dictionary<string, object> properties) : base(name)
        {
            LoadProperties(properties);
        }

        /// <summary>
        /// Model to render
        /// </summary>
        public ModelResource? Model { get; set; }

        /// <summary>
        /// Entity position
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Entity rotation
        /// </summary>
        public Vector3 Rotation { get; set; }

        /// <summary>
        /// Entity scale
        /// </summary>
        public Vector3 Scale { get; set; } = Vector3.One;

        /// <summary>
        /// Renders entity
        /// </summary>
        public void Render()
        {
            Model?.Render(Position, Rotation, Scale);
        }
    }
}
