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
    }
}