using UnityEngine;
using System.Collections;

public class BaseStat {

    private string name;
    private string description;
    private int baseValue;
    private int modValue;
    private StatTypes statType;

    public enum StatTypes
    {
        STRENGTH, INTELLIGENCE
    }

    public string Name { get; set; }

    public string Description { get; set; }

    public int BaseValue { get; set; }

    public int ModValue { get; set; }

    public StatTypes StatType { get; set; }

}
