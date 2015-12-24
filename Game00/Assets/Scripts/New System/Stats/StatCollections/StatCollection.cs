using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class StatCollection {

    [SerializeField]
    private StatDictionary<StatType, BaseStat> statDictionary;

    public StatDictionary<StatType, BaseStat> StatDictionary
    {
        get { return statDictionary; }
        //set { statDictionary = value; }
    }

    public StatCollection()
    {
        statDictionary = new StatDictionary<StatType, BaseStat>();
        SetBaseStats();
    }

    protected virtual void SetBaseStats() { }

    public bool ContainStat(StatType statType)
    {
        return statDictionary.ContainsKey(statType);
    }

    public BaseStat GetStat(StatType statType)
    {
        if (ContainStat(statType))
        {
            return statDictionary[statType];
        }
        return null;
    }

    public T GetStat<T>(StatType type) where T : BaseStat
    {
        return GetStat(type) as T;
    }

    protected T CreateStat<T>(StatType statType, int baseValue) where T : ModifiableStat
    {
        T stat = Activator.CreateInstance<T>();
        stat.BaseValue = baseValue;
        stat.Name = statType.ToString()[0] + statType.ToString().Substring(1).ToLower();
        statDictionary.Add(statType, stat);
        return stat;
    }

    public virtual void AddStat<T>(StatType statType, int baseValue) where T : ModifiableStat
    {
        T stat = GetStat<T>(statType);
        if (stat == null)
        {
            stat = CreateStat<T>(statType , baseValue);
        }
        else
        {
            stat.BaseValue += baseValue;
        }
    }

    public virtual void RemoveStat<T>(StatType statType, int baseValue) where T : ModifiableStat
    {
        T stat = GetStat<T>(statType);
        if (stat != null)
        {
            stat.BaseValue -= baseValue;
            if (stat.BaseValue <= 0)
            {
                statDictionary.Remove(statType);
            }
        }
    }

    public void TransferStats(StatCollection collection)
    {
        foreach (var key in collection.StatDictionary.Keys)
        {
            AddStat<ModifiableStat>(key, collection.GetStat(key).BaseValue);
        }
    }

    public void RemoveStats(StatCollection collection)
    {
        foreach (var key in collection.StatDictionary.Keys)
        {
            RemoveStat<ModifiableStat>(key, collection.GetStat(key).BaseValue);
        }
    }

    public string StatList()
    {
        string statList = string.Empty;
        foreach (var key in statDictionary.Keys)
        {
            BaseStat s = GetStat(key);
            if (s.GetType().Equals(typeof(VitalStat))){
                statList += "<color=orange>";
                statList += "\n" + s.Name + ": " + ((VitalStat)s).CurrentValue+"/"+s.BaseValue + "</color>";
            }
            else if (s.GetType().Equals(typeof(LinkableStat))){
                statList += "<color=blue>";
                statList += "\n" + s.Name + ": " + s.BaseValue + "</color>";
            }
            else
            {
                statList += "<color=green>";
                statList += "\n" + s.Name + ": " + s.BaseValue + "</color>";
            }
        }
        return statList;
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;

        StatCollection statCol = obj as StatCollection;
        if (statCol == null) return false;
        return this.StatDictionary.Equals(statCol.StatDictionary);
    }

    public override int GetHashCode()
    {
        return this.StatDictionary.GetHashCode();
    }
}
