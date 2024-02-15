/*
=========================================================

DeadGalaxy project

All rights reserved
Kabanov Kirill (Kiriller12) © 2024

Licensed under GNU General Public License v3.0
Full license text available in LICENCE file

=========================================================
*/

namespace DeadGalaxy.Core.Entities
{
    /// <summary>
    /// Updatable entity interface
    /// </summary>
    internal interface IUpdatable
    {
        /// <summary>
        /// Updates entity game logic
        /// </summary>
        /// <param name="dt">Time since last frame update</param>
        public void Update(float dt);
    }
}
