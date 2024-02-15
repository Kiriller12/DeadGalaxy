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
using System.Numerics;
using DeadGalaxy.Core.Helpers;
using Raylib_cs;

namespace DeadGalaxy.Core.Entities
{
    /// <summary>
    /// Game camera entity
    /// </summary>
    internal class CameraEntity : BaseEntity, IUpdatable
    {
        private Camera3D _camera;

        /// <summary>
        /// Game camera entity
        /// </summary>
        /// <param name="name">Camera name</param>
        public CameraEntity(string name) : base(name)
        {
            _camera = new Camera3D
            {
                Position = Vector3.One,
                Target = Vector3.Zero,
                Up = Vector3.UnitY,
                FovY = 45.0f,
                Projection = CameraProjection.Perspective
            };
        }

        /// <summary>
        /// Game camera entity
        /// </summary>
        /// <param name="name">Camera name</param>
        /// <param name="properties">Camera properties</param>
        public CameraEntity(string name, Dictionary<string, object> properties) : this(name)
        {
            LoadProperties(properties);
        }

        /// <summary>
        /// Camera position
        /// </summary>
        public Vector3 Position
        {
            get => _camera.Position;
            set => _camera.Position = value;
        }

        /// <summary>
        /// Camera look at target position
        /// </summary>
        public Vector3 Target
        {
            get => _camera.Target;
            set => _camera.Target = value;
        }

        /// <summary>
        /// Is main scene camera
        /// </summary>
        public bool IsMain { get; set; }

        /// <summary>
        /// Updates game camera logic
        /// </summary>
        /// <param name="dt">Time since last frame update</param>
        public void Update(float dt)
        {
            if (GuiHelpers.IsGuiMode())
            {
                return;
            }

            Raylib.UpdateCamera(ref _camera, CameraMode.Custom);
        }

        /// <summary>
        /// Initializes camera rendering mode
        /// </summary>
        public void BeginCameraMode()
        {
            Raylib.BeginMode3D(_camera);
        }

        /// <summary>
        /// Ends camera rendering mode
        /// </summary>
        public void EndCameraMode()
        {
            Raylib.EndMode3D();
        }
    }
}
