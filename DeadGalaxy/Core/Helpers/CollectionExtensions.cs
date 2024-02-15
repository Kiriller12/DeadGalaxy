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
using System.Collections.Generic;

namespace DeadGalaxy.Core.Helpers
{
    /// <summary>
    /// Collection extension methods
    /// </summary>
    internal static class CollectionExtensions
    {
        /// <summary>
        /// Clears collection with calling Dispose() on collection elements
        /// </summary>
        /// <typeparam name="T">Collection element type</typeparam>
        /// <param name="collection">Target collection</param>
        public static void ClearWithDispose<T>(this ICollection<T> collection)
        {
            // Disposing elements
            foreach (var element in collection)
            {
                if (element is IDisposable disposableElement)
                {
                    disposableElement.Dispose();
                }
            }

            // Clearing collection
            collection.Clear();
        }

        /// <summary>
        /// Clears collection with calling Dispose() on collection elements
        /// </summary>
        /// <typeparam name="T1">Collection key type</typeparam>
        /// <typeparam name="T2">Collection value type</typeparam>
        /// <param name="collection">Target collection</param>
        public static void ClearWithDispose<T1, T2>(this IDictionary<T1, T2> collection)
        {
            // Disposing elements
            foreach (var (_, element) in collection)
            {
                if (element is IDisposable disposableElement)
                {
                    disposableElement.Dispose();
                }
            }

            // Clearing collection
            collection.Clear();
        }
    }
}
