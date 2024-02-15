/*
=========================================================

DeadGalaxy project

All rights reserved
Kabanov Kirill (Kiriller12) © 2024

Licensed under GNU General Public License v3.0
Full license text available in LICENCE file

=========================================================
*/

using Raylib_cs;

namespace DeadGalaxy.Core.Helpers
{
    /// <summary>
    /// GUI helper methods
    /// </summary>
    internal static class GuiHelpers
    {
        private static int _isGuiMode;

        /// <summary>
        /// Returns true if in GUI mode
        /// </summary>
        public static bool IsGuiMode()
        {
            return _isGuiMode > 0;
        }

        /// <summary>
        /// Sets GUI input mode
        /// </summary>
        /// <param name="state">GUI mode state</param>
        public static void SetGuiMode(bool state = true)
        {
            // Updating GUI mode counter
            if (state)
            {
                _isGuiMode++;
            }
            else
            {
                _isGuiMode--;

                if (_isGuiMode < 0)
                {
                    _isGuiMode = 0;
                }
            }

            // Changing cursor state
            if (IsGuiMode())
            {
                Raylib.EnableCursor();
            }
            else
            {
                Raylib.DisableCursor();
            }
        }
    }
}
