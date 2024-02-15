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

namespace DeadGalaxy.Core
{
    /// <summary>
    /// Game engine exception
    /// </summary>
    internal class EngineException : Exception
    {
        /// <summary>
        /// Game engine exception
        /// </summary>
        /// <param name="message">Error message</param>
        public EngineException(string message) : base(message)
        {

        }
    }
}
