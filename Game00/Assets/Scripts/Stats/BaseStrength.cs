using UnityEngine;
using System.Collections;

public class BaseStrength : BaseStat {

	public BaseStrength()
    {
        Name = "Strength";
        Description = "Modifies attack value";
        BaseValue = 0;
        ModValue = 0;
        StatType = StatTypes.STRENGTH;
    }
}
