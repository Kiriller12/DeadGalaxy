/*
=========================================================

DeadGalaxy project

All right reserved
Kabanov Kirill (Kiriller12) © 2024

Licensed under GNU General Public License v3.0
Full license text available in LICENCE file

=========================================================
*/

using System;
using DeadGalaxy.Core.Helpers;
using Raylib_cs;
using System.Collections.Generic;
using System.Linq;

namespace DeadGalaxy.Core.Entities
{
    /// <summary>
    /// Base game entity class
    /// </summary>
    internal abstract class BaseEntity
    {
        /// <summary>
        /// Base game entity class
        /// </summary>
        /// <param name="name">Entity name</param>
        protected BaseEntity(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Entity name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Loads entity properties
        /// </summary>
        /// <param name="properties">Properties to load</param>
        protected void LoadProperties(Dictionary<string, object> properties)
        {
            var type = GetType();
            var typeProperties = type.GetProperties();

            foreach (var (name, value) in properties)
            {
                var property = typeProperties.FirstOrDefault(x =>
                    x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

                if (property == null)
                {
                    Raylib.TraceLog(TraceLogLevel.Warning,
                        $"[{type.Name}]: Couldn't set unknown property \"{name}\"!");

                    continue;
                }

                property.SetValue(this, value.Convert(property.PropertyType));
            }
        }
    }
}
