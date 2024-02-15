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
using System.IO;
using Newtonsoft.Json;

namespace DeadGalaxy.Core.Resources
{
    /// <summary>
    /// Scene metadata
    /// </summary>
    internal class SceneMetadata
    {
        /// <summary>
        /// Resources used by scene
        /// </summary>
        public List<ResourceMetadata> Resources { get; set; } = [];

        /// <summary>
        /// Entities placed on scene
        /// </summary>
        public List<EntityResourceMetadata> Entities { get; set; } = [];

        /// <summary>
        /// Loads content from a file
        /// </summary>
        /// <param name="filePath">Target file path</param>
        public static SceneMetadata? Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            using var file = File.OpenText(filePath);
            var serializer = new JsonSerializer();

            return serializer.Deserialize(file, typeof(SceneMetadata)) as SceneMetadata;
        }
    }
}
