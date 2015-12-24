using System;

[Serializable]
public class WeaponStatCollection : StatCollection {

    protected override void SetBaseStats()
    {
        /*LinkableStat attackPower = */CreateStat<LinkableStat>(StatType.ATTACKPOWER, 0);
        /*LinkableStat attackSpeed = */CreateStat<LinkableStat>(StatType.ATTACKSPEED, 0);
        /*LinkableStat range = */CreateStat<LinkableStat>(StatType.ATTACKRANGE, 0);
    }
}
