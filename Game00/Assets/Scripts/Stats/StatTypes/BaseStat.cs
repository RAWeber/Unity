using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public abstract class BaseStat {

    private string name;
    private string description;
    private int baseValue;
    //private StatTypes statType;

    public string Name { get; set; }

    public string Description { get; set; }

    public virtual int BaseValue { get; set; }

    public virtual int TotalStatValue { get { return BaseValue; } }

    public BaseStat()
    {
        Name = string.Empty;
        BaseValue = 0;
    }

    public BaseStat(string name, int baseValue)
    {
        Name = name;
        BaseValue = baseValue;
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;

        BaseStat stat = obj as BaseStat;
        if (stat == null) return false;
        //return this.StatID == stat.StatID;
        return this.Name.Equals(stat.Name);
    }

    public override int GetHashCode()
    {
        return this.Name.GetHashCode();
    }
}
