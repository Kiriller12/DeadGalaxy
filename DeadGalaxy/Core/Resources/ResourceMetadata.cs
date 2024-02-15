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
    /// Game resource metadata
    /// </summary>
    internal class ResourceMetadata
    {
        /// <summary>
        /// Resource name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Resource type
        /// </summary>
        public ResourceType Type { get; set; }

        /// <summary>
        /// Resource target path
        /// </summary>
        public string? FilePath { get; set; }

        /// <summary>
        /// Is resource generated
        /// </summary>
        public bool Generated { get; set; }

        /// <summary>
        /// Generated resource algorithm type
        /// </summary>
        public string? AlgorithmType { get; set; }
    }
}
