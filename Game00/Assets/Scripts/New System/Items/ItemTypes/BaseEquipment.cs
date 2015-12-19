using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BaseEquipment : BaseItem {

    public enum EquipmentType : int
    {
        HELM,CHEST,SHOULDERS,GLOVES,LEGS,BOOTS,BACK,ACCESSORY
    }

    private EquipmentType subType;

    public EquipmentType SubType
    {
        get { return subType; }
        //set { subType = value; }
    }

    public BaseEquipment(BaseItem item) : base(item)
    {
        subType = ((BaseEquipment)item).subType;
        Stats = new EquipmentStatCollection();
        Stats.AddStat<LinkableStat>(StatType.ARMOR, item.Stats.GetStat(StatType.ARMOR).BaseValue);
    }

    public BaseEquipment(Dictionary<string, string> itemDictionary) : base(itemDictionary)
    {
        subType = (EquipmentType)System.Enum.Parse(typeof(EquipmentType), itemDictionary["SubType"]);
        Stats = new EquipmentStatCollection();
        Stats.AddStat<LinkableStat>(StatType.ARMOR, Int32.Parse(itemDictionary["BaseStat1"]));
    }

    public override void use(Slot slot)
    {
        GameObject.FindObjectOfType<KeyHandler>().EquipSetActive(true);
        if (slot.name.Equals("Slot") || slot.name.Equals("ResultSlot"))
        {
            Slot equipSlot = GameObject.Find(this.SubType.ToString()).GetComponent<Slot>();
            BaseItem tmp = equipSlot.itemsInStack();
            equipSlot.ClearSlot();
            equipSlot.AddItem(this);
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
            GameObject.Find("InventoryWindow").GetComponent<Inventory>().AddItemToInventory(this);
            GameObject.FindObjectOfType<Player>().Stats.RemoveStats(this.Stats);
            slot.ClearSlot();
        }
    }

    public override Sprite ReturnItemIcon()
    {
        switch (subType)
        {
            case EquipmentType.HELM:
                return Resources.Load<Sprite>("ItemIcons/helmet");
            case EquipmentType.CHEST:
                return Resources.Load<Sprite>("ItemIcons/armor");
            case EquipmentType.SHOULDERS:
                return Resources.Load<Sprite>("ItemIcons/backpack");
            case EquipmentType.GLOVES:
                return Resources.Load<Sprite>("ItemIcons/coin");
            case EquipmentType.LEGS:
                return Resources.Load<Sprite>("ItemIcons/gemRed");
            case EquipmentType.BOOTS:
                return Resources.Load<Sprite>("ItemIcons/gemGreen");
            case EquipmentType.BACK:
                return Resources.Load<Sprite>("ItemIcons/gemGreen");
            case EquipmentType.ACCESSORY:
                return Resources.Load<Sprite>("ItemIcons/gemBlue");
            default:
                return Resources.Load<Sprite>("ItemIcons/x");
        }
    }
}
