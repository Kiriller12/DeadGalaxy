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
using Raylib_cs;

namespace DeadGalaxy.Core.Resources
{
    /// <summary>
    /// Texture resource
    /// </summary>
    internal class TextureResource : IDisposable
    {
        private Texture2D _texture;

        /// <summary>
        /// Texture resource
        /// </summary>
        /// <param name="name">Texture name</param>
        /// <param name="texture">Texture data</param>
        public TextureResource(string name, Texture2D texture)
        {
            Name = name;
            _texture = texture;

            // Setting texture mipmaps
            var mipmaps = Configuration.Get<bool>("graphic:mipmaps");
            SetMipmaps(mipmaps);

            // Setting default filter after mipmaps generation
            // If it is not defined here, setting will not apply
            SetFiltering(TextureFilter.Point);

            // Setting texture filter
            var filter = Configuration.Get<TextureFilter>("graphic:filtering");
            SetFiltering(filter);
        }

        /// <summary>
        /// Texture name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Texture data
        /// </summary>
        public Texture2D Texture => _texture;

        /// <summary>
        /// Sets texture filtering mode
        /// </summary>
        /// <param name="filter">Texture filter</param>
        public void SetFiltering(TextureFilter filter)
        {
            Raylib.SetTextureFilter(_texture, filter);
        }

        /// <summary>
        /// Generates texture mipmaps
        /// </summary>
        /// <param name="enabled">Is mipmaps enabled</param>
        public void SetMipmaps(bool enabled)
        {
            if (enabled)
            {
                if (_texture.Mipmaps == 1)
                {
                    Raylib.GenTextureMipmaps(ref _texture);
                }

                return;
            }

            _texture.Mipmaps = 1;
        }

        /// <summary>
        /// Clears resources
        /// </summary>
        public void Dispose()
        {
            Raylib.UnloadTexture(_texture);
        }
    }
}
