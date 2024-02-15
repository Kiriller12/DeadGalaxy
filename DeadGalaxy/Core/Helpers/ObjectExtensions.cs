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
using Newtonsoft.Json.Linq;

namespace DeadGalaxy.Core.Helpers
{
    /// <summary>
    /// Object extension methods
    /// </summary>
    internal static class ObjectExtensions
    {
        /// <summary>
        /// Converts object value to target type
        /// </summary>
        /// <typeparam name="T">Target type</typeparam>
        /// <param name="target">Target object</param>
        public static T? Convert<T>(this object target)
        {
            return (T?)Convert(target, typeof(T));
        }

        /// <summary>
        /// Converts object value to target type
        /// </summary>
        /// <param name="target">Target object</param>
        /// <param name="type">Target type</param>
        public static object? Convert(this object target, Type type)
        {
            if (type.IsInstanceOfType(target))
            {
                return target;
            }

            if (target is JObject jsonValue)
            {
                return jsonValue.ToObject(type);
            }

            try
            {
                return System.Convert.ChangeType(target, type);
            }
            catch
            {
                return default;
            }
        }
    }
}
