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
using System.Linq;
using Raylib_cs;

namespace DeadGalaxy.Core.Rendering
{
    /// <summary>
    /// Render buffer
    /// </summary>
    internal class RenderBuffer
    {
        private readonly List<(string name, Texture2D texture)> _textures = [];

        private int _width;
        private int _height;

        private uint _bufferId;
        private uint _depthBufferId;

        /// <summary>
        /// Buffer textures
        /// </summary>
        public IReadOnlyDictionary<string, Texture2D> Textures => _textures.ToDictionary();

        /// <summary>
        /// Registers texture
        /// </summary>
        /// <param name="name">Texture name</param>
        /// <param name="format">Pixel format</param>
        public void RegisterTexture(string name, PixelFormat format)
        {
            _textures.Add((name, new Texture2D
            {
                Format = format,
                Mipmaps = 1
            }));
        } 

        /// <summary>
        /// Begins buffer draw mode
        /// </summary>
        public void BeginBufferMode()
        {
            var width = Raylib.GetScreenWidth();
            var height = Raylib.GetScreenHeight();

            // If buffer not initialized or screen size was changed
            if (_width != width || _height != height)
            {
                // Updating screen dimensions
                _width = width;
                _height = height;

                // Reinitializing buffer
                Initialize();
            }

            // Update internal render batch
            Rlgl.DrawRenderBatchActive();

            // Enabling render buffer
            Rlgl.EnableFramebuffer(_bufferId);

            // Configure render settings
            Rlgl.EnableDepthTest();
            Rlgl.DisableColorBlend();
            Rlgl.EnableDepthMask();

            // Clearing buffer
            Raylib.ClearBackground(Color.Blank);
        }

        /// <summary>
        /// Ends buffer draw mode
        /// </summary>
        public void EndBufferMode()
        {
            // Update internal render batch
            Rlgl.DrawRenderBatchActive();

            // Switch back to standard render buffer
            Rlgl.DisableFramebuffer();

            // Reset render settings
            Rlgl.DisableDepthTest();
            Rlgl.EnableColorBlend();
            Rlgl.DisableDepthMask();
        }

        /// <summary>
        /// Initializes buffer
        /// </summary>
        private unsafe void Initialize()
        {
            // If buffer was previously initialized
            if (_bufferId != 0)
            {
                // Clearing buffer data
                Rlgl.UnloadFramebuffer(_bufferId);

                // Clearing textures
                foreach (var (_, texture) in _textures)
                {
                    Raylib.UnloadTexture(texture);
                }
            }

            // Loading render buffer
            _bufferId = Rlgl.LoadFramebuffer(_width, _height);
            Rlgl.EnableFramebuffer(_bufferId);

            // Activating texture slots
            Rlgl.ActiveDrawBuffers(_textures.Count);

            // Initializing textures
            for (var i = 0; i < _textures.Count; i++)
            {
                var texture = _textures[i].texture;
                
                texture.Width = _width;
                texture.Height = _height;
                texture.Id = Rlgl.LoadTexture(null, texture.Width, texture.Height, texture.Format, texture.Mipmaps);

                Rlgl.FramebufferAttach(_bufferId, texture.Id, FramebufferAttachType.ColorChannel0 + i,
                    FramebufferAttachTextureType.Texture2D, 0);

                _textures[i] = (_textures[i].name, texture);
            }

            // Loading depth buffer
            _depthBufferId = Rlgl.LoadTextureDepth(_width, _height, true);

            Rlgl.FramebufferAttach(_bufferId, _depthBufferId, FramebufferAttachType.Depth,
                FramebufferAttachTextureType.Renderbuffer, 0);

            // Checking for errors
            if (!Rlgl.FramebufferComplete(_bufferId))
            {
                Raylib.TraceLog(TraceLogLevel.Error, "[Render buffer]: Frame buffer not complete!");
            }

            // Switch back to standard render buffer
            Rlgl.DisableFramebuffer();
        }
    }
}
