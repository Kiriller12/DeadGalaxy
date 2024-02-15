/*
=========================================================

DeadGalaxy project

All right reserved
Kabanov Kirill (Kiriller12) © 2024

Licensed under GNU General Public License v3.0
Full license text available in LICENCE file

=========================================================
*/

namespace DeadGalaxy.Core.Rendering
{
    /// <summary>
    /// Point light shader locations
    /// </summary>
    internal class PointLightShaderLocations
    {
        /// <summary>
        /// Point light shader locations
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        /// <param name="intensity"></param>
        /// <param name="color"></param>
        public PointLightShaderLocations(int position, int radius, int intensity, int color)
        {
            Position = position;
            Radius = radius;
            Intensity = intensity;
            Color = color;
        }

        /// <summary>
        /// Position location
        /// </summary>
        public int Position { get; }

        /// <summary>
        /// Radius location
        /// </summary>
        public int Radius { get; }

        /// <summary>
        /// Intensity location
        /// </summary>
        public int Intensity { get; }

        /// <summary>
        /// Color location
        /// </summary>
        public int Color { get; }
    }
}
