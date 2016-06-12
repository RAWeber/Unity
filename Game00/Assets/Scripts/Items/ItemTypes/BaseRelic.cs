using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class BaseRelic : BaseItem {

    public enum RelicTypes : int
    {
        WEAPON, ARMOR
    }

    [SerializeField]
    private RelicTypes subType;

    public BaseRelic(BaseItem item) : base(item)
    {
        subType = ((BaseRelic)item).subType;
    }

    public BaseRelic(Dictionary<string, string> itemDictionary) : base(itemDictionary)
    {
        subType = (RelicTypes)Enum.Parse(typeof(RelicTypes), itemDictionary["SubType"]);
    }

    public override void use(Slot slot)
    {
        GameObject.FindObjectOfType<KeyHandler>().ComboSetActive(true);
        if (slot.name.Equals("Slot"))
        {
            foreach(Slot s in GameControl.comboWindow.ComboSlots)
            {
                if (s.name.Equals("ComboSlot") && s.IsEmpty())
                {
                    s.AddItem(this);
                    slot.ClearSlot();
                    break;
                }
            }
            GameControl.comboWindow.updateSet();
        }
        else
        {
            GameControl.inventory.AddItemToInventory(this);
            slot.ClearSlot();
        }
    }

    public override Sprite ReturnItemIcon()
    {
        switch (subType)
        {
            case RelicTypes.WEAPON:
                return Resources.Load<Sprite>("ItemIcons/book");
            case RelicTypes.ARMOR:
                return Resources.Load<Sprite>("ItemIcons/scroll");
            default:
                return Resources.Load<Sprite>("ItemIcons/Meat");
        }
    }
}
