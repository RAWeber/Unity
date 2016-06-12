using UnityEngine;
using System.Collections;
using System;

public abstract class StatModifier {

    private float modValue;
    private bool stacks;

    public event EventHandler OnValueChange;

    public float ModValue
    {
        get { return modValue; }
        set {
            if (modValue != value)
            {
                modValue = value;
                TriggerValueChange();
            }
        }
    }

    public bool Stacks
    {
        get { return stacks; }
        set { stacks = value; }
    }

    public abstract int Order { get; }

    public StatModifier(float modValue,bool stacks)
    {
        ModValue = modValue;
        Stacks = stacks;
    }

    public abstract int ApplyModifier(int statValue, float modValue);

    protected void TriggerValueChange()
    {
        if (OnValueChange != null)
        {
            OnValueChange(this, null);
        }
    }
}
