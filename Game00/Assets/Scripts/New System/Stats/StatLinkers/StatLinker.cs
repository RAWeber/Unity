using UnityEngine;
using System.Collections;
using System;

public abstract class StatLinker {

    public BaseStat LinkedStat { get; private set; }

    public event EventHandler OnValueChange;

    public abstract int Value { get; }

    public StatLinker(BaseStat stat)
    {
        LinkedStat = stat;
        ModifiableStat s = LinkedStat as ModifiableStat;
        s.OnValueChange += OnLinkedStatValueChange;
    }

    private void OnLinkedStatValueChange(object stat, EventArgs args)
    {
        if (OnValueChange != null)
        {
            OnValueChange(this, null);
        }
    }
}
