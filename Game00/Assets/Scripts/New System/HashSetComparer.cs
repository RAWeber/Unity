using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class HashSetEqualityComparer<T> : IEqualityComparer<HashSet<T>>
{
    public int GetHashCode(HashSet<T> hashSet)
    {
        if (hashSet == null)
            return 0;
        int h = 0x14345843;//some arbitrary number
        foreach (T elem in hashSet)
        {
            h = h + hashSet.Comparer.GetHashCode(elem);
        }
        return h;
    }

    public bool Equals(HashSet<T> set1, HashSet<T> set2)
    {
        if (set1 == set2)
            return true;
        if (set1 == null || set2 == null)
            return false;
        return set1.SetEquals(set2);
    }
}
