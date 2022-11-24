using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class GenericNotImplementedError
    {
        /// <summary>
        /// Checks if a component is missing, if so log an error.
        /// </summary>
        public static T TryGet<T>(T value)
        {
            if (value != null && !value.Equals(null))
            {
                return value;
            }

            Debug.LogError($"{typeof(T)} not implmented!");
            return default;
        }

        /// <summary>
        /// Checks if a component is missing, if so log an error with an optional name.
        /// </summary>
        public static T TryGet<T>(T value, string name)
        {
            if (value != null && !value.Equals(null))
            {
                return value;
            }

            Debug.LogError($"{typeof(T)} not implmented on {name}!");
            return default;
        }

    }
}

