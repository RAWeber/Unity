using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseEquipment : BaseItem {

    public enum EquipmentType : int
    {
        HELM,CHEST,SHOULDERS,GLOVES,LEGS,BOOTS,ACCESSORY
    }

    private EquipmentType subType;


    public BaseEquipment(Dictionary<string, string> itemDictionary) : base(itemDictionary)
    {
        subType = (EquipmentType)System.Enum.Parse(typeof(EquipmentType), itemDictionary["SubType"]);
    }

    public override void use()
    {
        Debug.Log("Armor equipped");
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
            case EquipmentType.ACCESSORY:
                return Resources.Load<Sprite>("ItemIcons/gemBlue");
            default:
                return Resources.Load<Sprite>("ItemIcons/x");
        }
    }
}
