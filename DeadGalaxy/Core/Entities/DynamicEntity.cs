/*
=========================================================

DeadGalaxy project

All right reserved
Kabanov Kirill (Kiriller12) © 2024

Licensed under GNU General Public License v3.0
Full license text available in LICENCE file

=========================================================
*/

using DeadGalaxy.Core.Resources;
using System.Collections.Generic;

namespace DeadGalaxy.Core.Entities
{
    /// <summary>
    /// Game dynamic entity
    /// </summary>
    internal class DynamicEntity : StaticEntity, IUpdatable
    {
        /// <summary>
        /// Game dynamic entity
        /// </summary>
        /// <param name="name">Entity name</param>
        /// <param name="model">Model to render</param>
        public DynamicEntity(string name, ModelResource? model) : base(name, model)
        {
            
        }

        /// <summary>
        /// Game dynamic entity
        /// </summary>
        /// <param name="name">Entity name</param>
        /// <param name="properties">Entity properties</param>
        public DynamicEntity(string name, Dictionary<string, object> properties) : base(name, properties)
        {
            
        }

        /// <summary>
        /// Updates entity game logic
        /// </summary>
        /// <param name="dt">Time since last frame update</param>
        public void Update(float dt)
        {
            
        }
    }
}
