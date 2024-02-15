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
using DeadGalaxy.Core;
using Raylib_cs;

namespace DeadGalaxy
{
    internal class Program
    {
        /// <summary>
        /// Program entry point
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args)
        {
            try
            {
                using var engine = new Engine(args);
                engine.Run();
            }
            catch (Exception e)
            {
                Raylib.TraceLog(TraceLogLevel.Error, $"[Engine]: Unhandled error: {e.Message}");
            }
        }
    }
}
