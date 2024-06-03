using System;
using UnityEngine;

namespace _DatabaseAPI.Scripts
{
    public static class JsonUtils
    {
        public static T[] ToArrayOf<T>(string json, bool fromDatabase = true)
        {
            if (fromDatabase)
            {
                json = "{\"items\":" + json + "}";
            }
            return JsonUtility.FromJson<Wrapper<T>>(json).items;
        }
        
        public static string ToJson<T>(T[] array)
        {
            var wrapper = new Wrapper<T>
            {
                items = array
            };
            return JsonUtility.ToJson(wrapper);
        }
        
        
        [Serializable]
        private struct Wrapper<T>
        {
            public T[] items;
        }
    }
}