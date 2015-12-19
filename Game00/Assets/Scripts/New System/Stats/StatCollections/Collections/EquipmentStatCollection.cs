using UnityEngine;
using System.Collections;

public class EquipmentStatCollection : StatCollection {

    protected override void SetBaseStats()
    {
        LinkableStat armor = CreateStat<LinkableStat>(StatType.ARMOR, 0);
    }
}
