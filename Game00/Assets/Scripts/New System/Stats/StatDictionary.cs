using System.Collections.Generic;
using System;

[Serializable]
public class StatDictionary<TKey, TValue>: Dictionary<TKey, TValue> {

    public override bool Equals(object obj)
    {
        if (obj == null) return false;

        StatDictionary<TKey,TValue> dict2 = obj as StatDictionary<TKey, TValue>;
        if (dict2 == null) return false;
        bool equal = false;
        if (this.Count == dict2.Count) // Require equal count.
        {
            equal = true;
            foreach (var pair in this)
            {
                TValue value;
                if (dict2.TryGetValue(pair.Key, out value))
                {
                    // Require value be equal.
                    if (!value.Equals(pair.Value))
                    {
                        equal = false;
                        break;
                    }
                }
                else
                {
                    // Require key be present.
                    equal = false;
                    break;
                }
            }
        }
        return equal;
    }

    public override int GetHashCode()
    {
        if (this == null)
            return 0;
        int h = 0x14345843;//some arbitrary number
        foreach (var pair in this)
        {
            h = h + this.Comparer.GetHashCode(pair.Key);
        }
        return h;
    }
}
