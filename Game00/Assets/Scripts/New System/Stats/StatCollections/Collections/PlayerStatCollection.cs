using System;

[Serializable]
public class PlayerStatCollection : StatCollection {

    protected override void SetBaseStats()
    {
        VitalStat health = CreateStat<VitalStat>(StatType.HEALTH, 200);
        VitalStat mana = CreateStat<VitalStat>(StatType.MANA, 100);

        LinkableStat armor = CreateStat<LinkableStat>(StatType.ARMOR, 0);
        LinkableStat attackPower = CreateStat<LinkableStat>(StatType.ATTACKPOWER, 50);
        LinkableStat attackSpeed = CreateStat<LinkableStat>(StatType.ATTACKSPEED, 10);
        /*LinkableStat attackRange = */CreateStat<LinkableStat>(StatType.ATTACKRANGE, 5);

        ModifiableStat strength = CreateStat<ModifiableStat>(StatType.STRENGTH, 5);
        ModifiableStat intelligence = CreateStat<ModifiableStat>(StatType.INTELLIGENCE, 5);
        ModifiableStat dexterity = CreateStat<ModifiableStat>(StatType.DEXTERITY, 5);
        ModifiableStat stamina = CreateStat<ModifiableStat>(StatType.STAMINA, 5);
        ModifiableStat defense = CreateStat<ModifiableStat>(StatType.DEFENSE, 5);

        health.AddLinker(new RatioLinker(stamina, 10));
        health.SetCurrentValueToMax();
        mana.AddLinker(new RatioLinker(intelligence, 2));
        mana.SetCurrentValueToMax();

        armor.AddLinker(new RatioLinker(defense, 5));
        attackPower.AddLinker(new RatioLinker(strength, 5));
        attackSpeed.AddLinker(new RatioLinker(dexterity, 0.5f));
    }

    public override void RemoveStat<T>(StatType statType, int baseValue)
    {
        T stat = GetStat<T>(statType);
        if (stat != null)
        {
            stat.BaseValue -= baseValue;
            if (stat.BaseValue < 0)
            {
                stat.BaseValue = 0;
            }
        }
    }
}
