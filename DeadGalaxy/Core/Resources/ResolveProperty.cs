/*
=========================================================

DeadGalaxy project

All right reserved
Kabanov Kirill (Kiriller12) © 2024

Licensed under GNU General Public License v3.0
Full license text available in LICENCE file

=========================================================
*/

namespace DeadGalaxy.Core.Resources
{
    /// <summary>
    /// Resolvable property
    /// </summary>
    internal class ResolveProperty
    {
        /// <summary>
        /// Property resource name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Resolvable resource type
        /// </summary>
        public ResourceType ResolveType { get; set; }
    }
}
