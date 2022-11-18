using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class Miscellaneous
    {
        private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
        
        public static WaitForSeconds NewWaitForSeconds(float time)
        {
            if (WaitDictionary.TryGetValue(time, out WaitForSeconds wait)) return wait;

            WaitDictionary[time] = new WaitForSeconds(time);
            return WaitDictionary[time];
        }
    }

    public static class ComponentErrorHandler
    {
        /// <summary>
        /// Checks if a component is missing, if so log an error.
        /// </summary>
        public static T ReturnIfNotNull<T>(T objectToReturn) where T : class
        {
            if (objectToReturn != null) return objectToReturn;
            else
            {
                Debug.LogError("Missing " + objectToReturn.GetType());
                return null;
            }
        }

        /// <summary>
        /// Checks if a component is missing, if so log an error with its parent's name.
        /// </summary>
        public static T ReturnIfNotNull<T>(T objectToReturn, string optionalParentName) where T : class
        {
            if (objectToReturn.Equals(null)) // == operator does not work for generic
            {
                Debug.LogError("Missing " + objectToReturn.GetType() + " on " + optionalParentName);
                return null;
            }
            else
            {
                return objectToReturn;
            }
        }
    }
}