/*
=========================================================

DeadGalaxy project

All right reserved
Kabanov Kirill (Kiriller12) © 2024

Licensed under GNU General Public License v3.0
Full license text available in LICENCE file

=========================================================
*/

using Raylib_cs;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DeadGalaxy.Core.Helpers
{
    /// <summary>
    /// Raylib helper methods
    /// </summary>
    internal static class RaylibHelpers
    {
        private static Action<TraceLogLevel, string?>? _traceLogCallback;

        /// <summary>
        /// Generates model by algorithm
        /// </summary>
        /// <param name="algorithm">Model generation algorithm type</param>
        public static Model GenerateModelByAlgorithm(string? algorithm)
        {
            switch (algorithm)
            {
                case "box":
                    var mesh = Raylib.GenMeshCube(1.0f, 1.0f, 1.0f);
                    Raylib.GenMeshTangents(ref mesh);

                    return Raylib.LoadModelFromMesh(mesh);

                default:
                    Raylib.TraceLog(TraceLogLevel.Error,
                        $"[Model]: Couldn't generate model resource with algorithm \"{algorithm}\". Unknown generation algorithm type!");

                    return new Model();
            }
        }

        /// <summary>
        /// Sets raylib trace log callback
        /// </summary>
        /// <param name="callback">Callback function</param>
        public static unsafe void SetTraceLogCallback(Action<TraceLogLevel, string?> callback)
        {
            _traceLogCallback = callback;
            Raylib.SetTraceLogCallback(&LogMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static unsafe void LogMessage(int logLevel, sbyte* text, sbyte* args)
        {
            var message = Logging.GetLogMessage(new IntPtr(text), new IntPtr(args));

            _traceLogCallback?.Invoke((TraceLogLevel)logLevel, message);
        }
    }
}
