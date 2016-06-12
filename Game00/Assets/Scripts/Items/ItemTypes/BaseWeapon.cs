using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class BaseWeapon : BaseItem {

    public enum WeaponTypes : int
    {
        SWORD, DAGGER, AXE, BOW, CROSSBOW, BLOWGUN, STAFF, WAND, WHIP
    }

    [SerializeField]
    private WeaponTypes subType;
    [SerializeField]
    private int hand;

    public WeaponTypes SubType
    {
        get { return subType; }
        //set { subType = value; }
    }

    public int Hand
    {
        get { return hand; }
        set { hand = value; }
    }

    public BaseWeapon(BaseWeapon item) : base(item)
    {
        subType = item.subType;
        Stats = new WeaponStatCollection();
        Stats.AddStat<LinkableStat>(StatType.ATTACKPOWER, item.Stats.GetStat(StatType.ATTACKPOWER).BaseValue);
        Stats.AddStat<LinkableStat>(StatType.ATTACKSPEED, item.Stats.GetStat(StatType.ATTACKSPEED).BaseValue);
        Stats.AddStat<LinkableStat>(StatType.ATTACKRANGE, item.Stats.GetStat(StatType.ATTACKRANGE).BaseValue);
        hand = item.Hand;
    }

    public BaseWeapon(Dictionary<string, string> itemDictionary) : base(itemDictionary)
    {
        subType = (WeaponTypes)Enum.Parse(typeof(WeaponTypes), itemDictionary["SubType"]);
        Stats = new WeaponStatCollection();
        Stats.AddStat<LinkableStat>(StatType.ATTACKPOWER, int.Parse(itemDictionary["BaseStat1"]));
        Stats.AddStat<LinkableStat>(StatType.ATTACKSPEED, int.Parse(itemDictionary["BaseStat2"]));
        Stats.AddStat<LinkableStat>(StatType.ATTACKRANGE, int.Parse(itemDictionary["BaseStat3"]));
        hand = int.Parse(itemDictionary["BaseStat4"]);
    }

    public override void use(Slot slot)
    {
        GameObject.FindObjectOfType<KeyHandler>().EquipSetActive(true);
        Slot mainHand = GameObject.Find("MAINHAND").GetComponent<Slot>();
        Slot offHand = GameObject.Find("OFFHAND").GetComponent<Slot>();
        if (slot.name.Equals("Slot") || slot.name.Equals("ResultSlot"))
        {
            Slot weaponSlot;
            if (mainHand.IsEmpty() || this.Hand==2 || ((BaseWeapon)mainHand.SlotItems()).Hand==2)
            {
                weaponSlot = mainHand;
                if (this.Hand==2)
                {
                    BaseWeapon item = (BaseWeapon)offHand.SlotItems();
                    GameControl.inventory.AddItemToInventory(item);
                    offHand.ClearSlot();
                }
            }
            else
            {
                weaponSlot = offHand;
            }
            BaseItem tmp = weaponSlot.SlotItems();
            weaponSlot.ClearSlot();
            weaponSlot.AddItem(this);
            slot.ClearSlot();
            if (slot.name.Equals("Slot"))
            {
                slot.AddItem(tmp);
            }
            else
            {
                GameControl.inventory.AddItemToInventory(tmp);
                GameControl.comboWindow.Created = false;
            }
            GameControl.player.Stats.TransferStats(this.Stats);
        }
        else
        {
            if (slot.name.Equals("MAINHAND"))
            {
                BaseWeapon item = (BaseWeapon)offHand.SlotItems();
                GameControl.inventory.AddItemToInventory(this);
                GameControl.inventory.AddItemToInventory(item);
                offHand.ClearSlot();
            }
            else
            {
                GameControl.inventory.AddItemToInventory(this);
            }
            GameControl.player.Stats.RemoveStats(this.Stats);
            slot.ClearSlot();
        }
    }

    public override Sprite ReturnItemIcon()
    {
        switch (subType)
        {
            case WeaponTypes.SWORD:
                return Resources.Load<Sprite>("ItemIcons/sword");
            case WeaponTypes.DAGGER:
                return Resources.Load<Sprite>("ItemIcons/sword");
            case WeaponTypes.AXE:
                return Resources.Load<Sprite>("ItemIcons/axe");
            case WeaponTypes.BOW:
                return Resources.Load<Sprite>("ItemIcons/b_t_01");
            case WeaponTypes.CROSSBOW:
                return Resources.Load<Sprite>("ItemIcons/b_t_01");
            case WeaponTypes.BLOWGUN:
                return Resources.Load<Sprite>("ItemIcons/b_t_01");
            case WeaponTypes.STAFF:
                return Resources.Load<Sprite>("ItemIcons/axe");
            case WeaponTypes.WAND:
                return Resources.Load<Sprite>("ItemIcons/axe");
            case WeaponTypes.WHIP:
                return Resources.Load<Sprite>("ItemIcons/axe");
            default:
                return Resources.Load<Sprite>("ItemIcons/Meat");
        }
    }
}
