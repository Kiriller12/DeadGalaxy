/*
=========================================================

DeadGalaxy project

All rights reserved
Kabanov Kirill (Kiriller12) © 2024

Licensed under GNU General Public License v3.0
Full license text available in LICENCE file

=========================================================
*/

using System;
using System.Numerics;
using DeadGalaxy.Core.Rendering;
using Raylib_cs;

namespace DeadGalaxy.Core.Resources
{
    /// <summary>
    /// Model resource
    /// </summary>
    internal class ModelResource : IDisposable
    {
        private Model _model;

        /// <summary>
        /// Model resource
        /// </summary>
        /// <param name="name">Model name</param>
        /// <param name="model">Model data</param>
        public ModelResource(string name, Model model)
        {
            Name = name;
            _model = model;

            // Setting model shaders
            SetDefaultShader();
        }

        /// <summary>
        /// Model name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Model data
        /// </summary>
        public Model Model => _model;

        /// <summary>
        /// Sets model material texture
        /// </summary>
        /// <param name="index">Material index</param>
        /// <param name="map">Material map</param>
        /// <param name="texture">Target texture</param>
        public void SetMaterialTexture(int index, MaterialMapIndex map, Texture2D texture)
        {
            Raylib.SetMaterialTexture(ref _model, index, map, ref texture);
        }

        /// <summary>
        /// Renders model
        /// </summary>
        public void Render(Vector3 position)
        {
            Render(position, Vector3.Zero, Vector3.One, Color.White);
        }

        /// <summary>
        /// Renders model
        /// </summary>
        public void Render(Vector3 position, Vector3 rotation)
        {
            Render(position, rotation, Vector3.One, Color.White);
        }

        /// <summary>
        /// Renders model
        /// </summary>
        public void Render(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            Render(position, rotation, scale, Color.White);
        }

        /// <summary>
        /// Renders model
        /// </summary>
        public unsafe void Render(Vector3 position, Vector3 rotation, Vector3 scale, Color color)
        {
            // Calculating transform matrix for given position and rotation
            var scaleMatrix = Raymath.MatrixScale(scale.X, scale.Y, scale.Z);
            var rotationMatrix = Raymath.MatrixRotateXYZ(rotation);
            var scaleAndRotationMatrix = Raymath.MatrixMultiply(scaleMatrix, rotationMatrix);
            var translationMatrix = Raymath.MatrixTranslate(position.X, position.Y, position.Z);

            var transformMatrix = Raymath.MatrixMultiply(scaleAndRotationMatrix, translationMatrix);

            // Drawing all model meshes
            for (var i = 0; i < _model.MeshCount; i++)
            {
                Raylib.DrawMesh(_model.Meshes[i], _model.Materials[i], transformMatrix);
            }
        }

        /// <summary>
        /// Clears resources
        /// </summary>
        public void Dispose()
        {
            Raylib.UnloadModel(_model);
        }

        /// <summary>
        /// Sets model default shader
        /// </summary>
        private void SetDefaultShader()
        {
            if (RenderingPipeline.Instance == null)
            {
                return;
            }

            var shader = RenderingPipeline.Instance.DefaultShader;
            for (var i = 0; i < _model.MaterialCount; i++)
            {
                Raylib.SetMaterialShader(ref _model, i, ref shader);
            }
        }
    }
}
