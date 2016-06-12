using System;

[Serializable]
public class PlayerStatCollection : StatCollection {

    VitalStat health;
    VitalStat mana;
    LinkableStat armor;
    LinkableStat attackPower;
    LinkableStat attackSpeed;
    LinkableStat attackRange;
    ModifiableStat strength;
    ModifiableStat intelligence;
    ModifiableStat dexterity;
    ModifiableStat stamina;
    ModifiableStat defense;

    public PlayerStatCollection()
    {
        health = CreateStat<VitalStat>(StatType.HEALTH, 100);
        mana = CreateStat<VitalStat>(StatType.MANA, 100);

        armor = CreateStat<LinkableStat>(StatType.ARMOR, 0);
        attackPower = CreateStat<LinkableStat>(StatType.ATTACKPOWER, 10);
        attackSpeed = CreateStat<LinkableStat>(StatType.ATTACKSPEED, 10);
        attackRange = CreateStat<LinkableStat>(StatType.ATTACKRANGE, 2);

        strength = CreateStat<ModifiableStat>(StatType.STRENGTH, 5);
        intelligence = CreateStat<ModifiableStat>(StatType.INTELLIGENCE, 5);
        dexterity = CreateStat<ModifiableStat>(StatType.DEXTERITY, 5);
        stamina = CreateStat<ModifiableStat>(StatType.STAMINA, 5);
        defense = CreateStat<ModifiableStat>(StatType.DEFENSE, 5);

        health.AddLinker(new RatioLinker(stamina, 10));
        health.SetCurrentValueToMax();
        mana.AddLinker(new RatioLinker(intelligence, 2));
        mana.SetCurrentValueToMax();

        armor.AddLinker(new RatioLinker(defense, 0.5f));
        attackPower.AddLinker(new RatioLinker(strength, 0.5f));
        attackSpeed.AddLinker(new RatioLinker(dexterity, 0.5f));
    }

    public PlayerStatCollection(int health, int mana)
    {
        this.health = CreateStat<VitalStat>(StatType.HEALTH, health);
        this.mana = CreateStat<VitalStat>(StatType.MANA, mana);

        LinkableStat armor = CreateStat<LinkableStat>(StatType.ARMOR, 0);
        LinkableStat attackPower = CreateStat<LinkableStat>(StatType.ATTACKPOWER, 10);
        LinkableStat attackSpeed = CreateStat<LinkableStat>(StatType.ATTACKSPEED, 10);
        LinkableStat attackRange = CreateStat<LinkableStat>(StatType.ATTACKRANGE, 2);

        ModifiableStat strength = CreateStat<ModifiableStat>(StatType.STRENGTH, 5);
        ModifiableStat intelligence = CreateStat<ModifiableStat>(StatType.INTELLIGENCE, 5);
        ModifiableStat dexterity = CreateStat<ModifiableStat>(StatType.DEXTERITY, 5);
        ModifiableStat stamina = CreateStat<ModifiableStat>(StatType.STAMINA, 5);
        ModifiableStat defense = CreateStat<ModifiableStat>(StatType.DEFENSE, 5);

        this.health.AddLinker(new RatioLinker(stamina, 10));
        this.health.SetCurrentValueToMax();
        this.mana.AddLinker(new RatioLinker(intelligence, 2));
        this.mana.SetCurrentValueToMax();

        armor.AddLinker(new RatioLinker(defense, 0.5f));
        attackPower.AddLinker(new RatioLinker(strength, 0.5f));
        attackSpeed.AddLinker(new RatioLinker(dexterity, 0.5f));
    }

    public PlayerStatCollection(int health, int mana, int armor, int attackPower, int attackSpeed, int attackRange)
    {
        this.health = CreateStat<VitalStat>(StatType.HEALTH, health);
        this.mana = CreateStat<VitalStat>(StatType.MANA, mana);

        this.armor = CreateStat<LinkableStat>(StatType.ARMOR, armor);
        this.attackPower = CreateStat<LinkableStat>(StatType.ATTACKPOWER, attackPower);
        this.attackSpeed = CreateStat<LinkableStat>(StatType.ATTACKSPEED, attackSpeed);
        this.attackRange = CreateStat<LinkableStat>(StatType.ATTACKRANGE, attackRange);

        ModifiableStat strength = CreateStat<ModifiableStat>(StatType.STRENGTH, 5);
        ModifiableStat intelligence = CreateStat<ModifiableStat>(StatType.INTELLIGENCE, 5);
        ModifiableStat dexterity = CreateStat<ModifiableStat>(StatType.DEXTERITY, 5);
        ModifiableStat stamina = CreateStat<ModifiableStat>(StatType.STAMINA, 5);
        ModifiableStat defense = CreateStat<ModifiableStat>(StatType.DEFENSE, 5);

        this.health.AddLinker(new RatioLinker(stamina, 10));
        this.health.SetCurrentValueToMax();
        this.mana.AddLinker(new RatioLinker(intelligence, 2));
        this.mana.SetCurrentValueToMax();

        this.armor.AddLinker(new RatioLinker(defense, 0.5f));
        this.attackPower.AddLinker(new RatioLinker(strength, 0.5f));
        this.attackSpeed.AddLinker(new RatioLinker(dexterity, 0.5f));
    }

    public PlayerStatCollection(int health, int mana, int armor, int attackPower, int attackSpeed, int attackRange, int strength, int intelligence, int dexterity, int stamina, int defense)
    {
        this.health = CreateStat<VitalStat>(StatType.HEALTH, health);
        this.mana = CreateStat<VitalStat>(StatType.MANA, mana);

        this.armor = CreateStat<LinkableStat>(StatType.ARMOR, armor);
        this.attackPower = CreateStat<LinkableStat>(StatType.ATTACKPOWER, attackPower);
        this.attackSpeed = CreateStat<LinkableStat>(StatType.ATTACKSPEED, attackSpeed);
        this.attackRange = CreateStat<LinkableStat>(StatType.ATTACKRANGE, attackRange);

        this.strength = CreateStat<ModifiableStat>(StatType.STRENGTH, strength);
        this.intelligence = CreateStat<ModifiableStat>(StatType.INTELLIGENCE, intelligence);
        this.dexterity = CreateStat<ModifiableStat>(StatType.DEXTERITY, dexterity);
        this.stamina = CreateStat<ModifiableStat>(StatType.STAMINA, stamina);
        this.defense = CreateStat<ModifiableStat>(StatType.DEFENSE, defense);

        this.health.AddLinker(new RatioLinker(this.stamina, 10));
        this.health.SetCurrentValueToMax();
        this.mana.AddLinker(new RatioLinker(this.intelligence, 2));
        this.mana.SetCurrentValueToMax();

        this.armor.AddLinker(new RatioLinker(this.defense, 0.5f));
        this.attackPower.AddLinker(new RatioLinker(this.strength, 0.5f));
        this.attackSpeed.AddLinker(new RatioLinker(this.dexterity, 0.5f));
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
