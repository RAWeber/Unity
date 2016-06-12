using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
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
        subType = (EquipmentType)Enum.Parse(typeof(EquipmentType), itemDictionary["SubType"]);
        Stats = new EquipmentStatCollection();
        Stats.AddStat<LinkableStat>(StatType.ARMOR, int.Parse(itemDictionary["BaseStat1"]));
    }

    public override void use(Slot slot)
    {
        GameObject.FindObjectOfType<KeyHandler>().EquipSetActive(true);
        if (slot.name.Equals("Slot") || slot.name.Equals("ResultSlot"))
        {
            Slot equipSlot = GameObject.Find(this.SubType.ToString()).GetComponent<Slot>();
            BaseItem tmp = equipSlot.SlotItems();
            equipSlot.ClearSlot();
            equipSlot.AddItem(this);
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
            GameControl.inventory.AddItemToInventory(this);
            GameControl.player.Stats.RemoveStats(this.Stats);
            slot.ClearSlot();
        }
    }

    public override Sprite ReturnItemIcon()
    {
        switch (subType)
        {
            case EquipmentType.HELM:
                return Resources.Load<Sprite>("ItemIcons/helmets");
            case EquipmentType.CHEST:
                return Resources.Load<Sprite>("ItemIcons/armor");
            case EquipmentType.SHOULDERS:
                return Resources.Load<Sprite>("ItemIcons/shoulders");
            case EquipmentType.GLOVES:
                return Resources.Load<Sprite>("ItemIcons/gloves");
            case EquipmentType.LEGS:
                return Resources.Load<Sprite>("ItemIcons/pants");
            case EquipmentType.BOOTS:
                return Resources.Load<Sprite>("ItemIcons/boots");
            case EquipmentType.BACK:
                return Resources.Load<Sprite>("ItemIcons/cloaks");
            case EquipmentType.ACCESSORY:
                return Resources.Load<Sprite>("ItemIcons/necklace");
            default:
                return Resources.Load<Sprite>("ItemIcons/Meat");
        }
    }
}
