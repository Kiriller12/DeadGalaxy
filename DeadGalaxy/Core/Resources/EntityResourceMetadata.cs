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

namespace DeadGalaxy.Core.Resources
{
    /// <summary>
    /// Game entity resource metadata
    /// </summary>
    internal class EntityResourceMetadata
    {
        /// <summary>
        /// Entity name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Entity resource type
        /// </summary>
        public EntityResourceType Type { get; set; }

        /// <summary>
        /// Entity properties
        /// </summary>
        public Dictionary<string, object> Properties { get; set; } = [];
    }
}
