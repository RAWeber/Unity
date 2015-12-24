using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[Serializable]
public class ModifiableStat : BaseStat {

    private List<StatModifier> modList;
    private int totalModValue;

    public event EventHandler OnValueChange;

    public override int TotalStatValue
    {
        get { return base.TotalStatValue + TotalModValue; }
    }

    public int TotalModValue
    {
        get { return totalModValue; }
    }

    public ModifiableStat() : base()
    {
        modList = new List<StatModifier>();
        totalModValue = 0;
    }

    public ModifiableStat(string name, int baseValue) : base(name, baseValue) {
        modList = new List<StatModifier>();
        totalModValue = 0;
    }

    public void AddModifier(StatModifier mod)
    {
        modList.Add(mod);
        mod.OnValueChange += OnModValueChange;

    }

    public void RemoveModifier(StatModifier mod)
    {
        modList.Remove(mod);
        mod.OnValueChange -= OnModValueChange;

    }

    public void ClearModifiers()
    {
        foreach (var mod in modList)
        {
            mod.OnValueChange -= OnModValueChange;
        }
        modList.Clear();
    }

    public void UpdateModifiers()
    {
        totalModValue = 0;

        var orderGroups = modList.OrderBy(m => m.Order).GroupBy(m => m.Order);
        foreach (var group in orderGroups)
        {
            float sum = 0, max = 0;
            foreach (var mod in group)
            {
                if (mod.Stacks == false)
                {
                    if (mod.ModValue > max)
                    {
                        max = mod.ModValue;
                    }
                }
                else
                {
                    sum += mod.ModValue;
                }
            }

            totalModValue += group.First().ApplyModifier(BaseValue + totalModValue, sum > max ? sum : max);
        }
        TriggerValueChange();
    }

    protected void TriggerValueChange()
    {
        if (OnValueChange != null)
        {
            OnValueChange(this, null);
        }
    }

    public void OnModValueChange(object modifier, System.EventArgs args)
    {
        UpdateModifiers();
    }
}
