using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class VitalStat : LinkableStat {

    public event EventHandler OnCurrentValueChange;

    private int currentValue;

    public int CurrentValue
    {
        get
        {
            if (currentValue > TotalStatValue)
            {
                currentValue = TotalStatValue;
            }
            else if (currentValue < 0)
            {
                currentValue = 0;
            }
            return currentValue;
        }
        set
        {
            if (currentValue != value)
            {
                currentValue = value;
                TriggerCurrentValueChange();
            }
        }
    }

    public VitalStat() : base()
    {
        currentValue=BaseValue;
    }

    public VitalStat(string name, int baseValue) : base(name, baseValue)
    {
        currentValue = BaseValue;
    }

    public void SetCurrentValueToMax()
    {
        currentValue = TotalStatValue;
    }

    private void TriggerCurrentValueChange()
    {
        if (OnCurrentValueChange != null)
        {
            OnCurrentValueChange(this, null);
        }
    }
}
