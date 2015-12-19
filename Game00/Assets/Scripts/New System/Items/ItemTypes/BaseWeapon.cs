using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BaseWeapon : BaseItem {

    public enum WeaponTypes : int
    {
        SWORD, DAGGER, AXE, BOW, CROSSBOW, BLOWGUN, STAFF, WAND, WHIP
    }

    private WeaponTypes subType;
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
        subType = (WeaponTypes)System.Enum.Parse(typeof(WeaponTypes), itemDictionary["SubType"]);
        Stats = new WeaponStatCollection();
        Stats.AddStat<LinkableStat>(StatType.ATTACKPOWER, Int32.Parse(itemDictionary["BaseStat1"]));
        Stats.AddStat<LinkableStat>(StatType.ATTACKSPEED, Int32.Parse(itemDictionary["BaseStat2"]));
        Stats.AddStat<LinkableStat>(StatType.ATTACKRANGE, Int32.Parse(itemDictionary["BaseStat3"]));
        hand = Int32.Parse(itemDictionary["BaseStat4"]);
    }

    public override void use(Slot slot)
    {
        GameObject.FindObjectOfType<KeyHandler>().EquipSetActive(true);
        if (slot.name.Equals("Slot") || slot.name.Equals("ResultSlot"))
        {
            Slot weaponSlot;
            if (GameObject.Find("MAINHAND").GetComponent<Slot>().IsEmpty() || this.Hand==2 || ((BaseWeapon)GameObject.Find("MAINHAND").GetComponent<Slot>().itemsInStack()).Hand==2)
            {
                weaponSlot = GameObject.Find("MAINHAND").GetComponent<Slot>();
                if (this.Hand==2)
                {
                    BaseWeapon item = (BaseWeapon)GameObject.Find("OFFHAND").GetComponent<Slot>().itemsInStack();
                    GameObject.Find("InventoryWindow").GetComponent<Inventory>().AddItemToInventory(item);
                    GameObject.Find("OFFHAND").GetComponent<Slot>().ClearSlot();
                }
            }
            else
            {
                weaponSlot = GameObject.Find("OFFHAND").GetComponent<Slot>();
            }
            BaseItem tmp = weaponSlot.itemsInStack();
            weaponSlot.ClearSlot();
            weaponSlot.AddItem(this);
            slot.ClearSlot();
            if (slot.name.Equals("Slot"))
            {
                slot.AddItem(tmp);
            }
            else
            {
                GameObject.Find("InventoryWindow").GetComponent<Inventory>().AddItemToInventory(tmp);
                GameObject.Find("CombinationWindow").GetComponent<CombinationWindow>().Created = false;
            }
            GameObject.FindObjectOfType<Player>().Stats.TransferStats(this.Stats);
        }
        else
        {
            if (slot.name.Equals("MAINHAND"))
            {
                BaseWeapon item = (BaseWeapon)GameObject.Find("OFFHAND").GetComponent<Slot>().itemsInStack();
                GameObject.Find("InventoryWindow").GetComponent<Inventory>().AddItemToInventory(this);
                GameObject.Find("InventoryWindow").GetComponent<Inventory>().AddItemToInventory(item);
                GameObject.Find("OFFHAND").GetComponent<Slot>().ClearSlot();
            }
            else
            {
                GameObject.Find("InventoryWindow").GetComponent<Inventory>().AddItemToInventory(this);
            }
            GameObject.FindObjectOfType<Player>().Stats.RemoveStats(this.Stats);
            slot.ClearSlot();
        }
    }

    public override Sprite ReturnItemIcon()
    {
        switch (subType)
        {
            case WeaponTypes.SWORD:
                return Resources.Load<Sprite>("ItemIcons/upg_sword");
            case WeaponTypes.DAGGER:
                return Resources.Load<Sprite>("ItemIcons/upg_dagger");
            case WeaponTypes.AXE:
                return Resources.Load<Sprite>("ItemIcons/upg_axe");
            case WeaponTypes.BOW:
                return Resources.Load<Sprite>("ItemIcons/upg_bow");
            case WeaponTypes.CROSSBOW:
                return Resources.Load<Sprite>("ItemIcons/bow");
            case WeaponTypes.BLOWGUN:
                return Resources.Load<Sprite>("ItemIcons/tools");
            case WeaponTypes.STAFF:
                return Resources.Load<Sprite>("ItemIcons/upg_spear");
            case WeaponTypes.WAND:
                return Resources.Load<Sprite>("ItemIcons/upg_wand");
            case WeaponTypes.WHIP:
                return Resources.Load<Sprite>("ItemIcons/upg_hammer");
            default:
                return Resources.Load<Sprite>("ItemIcons/x");
        }
    }
}
