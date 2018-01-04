using System;
using System.Collections.Generic;

namespace UU_GameProject
{
    public static class Misc
    {
        public static Dictionary<U, V> Copy<U, V>(Dictionary<U, V> orig)
        {
            Dictionary<U, V> newd = new Dictionary<U, V>();
            foreach (KeyValuePair<U, V> entry in orig)
                newd.Add(entry.Key, entry.Value);
            return newd;
        }

        public static List<T> Copy<T>(List<T> orig)
        {
            List<T> newl = new List<T>();
            for (int i = 0; i < orig.Count; i++)
                newl.Add(orig[i]);
            return newl;
        }
    }
}