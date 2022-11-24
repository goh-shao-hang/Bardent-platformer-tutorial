using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class Helper
    {
        private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();

        /// <summary>
        /// Create a new WaitForSeconds instance that can be reused without memory allocation.
        /// </summary>
        public static WaitForSeconds NewWaitForSeconds(float time)
        {
            if (WaitDictionary.TryGetValue(time, out WaitForSeconds wait)) return wait;

            WaitDictionary[time] = new WaitForSeconds(time);
            return WaitDictionary[time];
        }
    }
}