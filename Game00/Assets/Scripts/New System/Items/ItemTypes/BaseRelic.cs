using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseRelic : BaseItem {

    public enum RelicTypes : int
    {
        WEAPON, ARMOR
    }

    private RelicTypes subType;

    public BaseRelic(BaseItem item) : base(item)
    {
        subType = ((BaseRelic)item).subType;
    }

    public BaseRelic(Dictionary<string, string> itemDictionary) : base(itemDictionary)
    {
        subType = (RelicTypes)System.Enum.Parse(typeof(RelicTypes), itemDictionary["SubType"]);
    }

    public override void use(Slot slot)
    {
        GameObject.FindObjectOfType<KeyHandler>().ComboSetActive(true);
        if (slot.name.Equals("Slot"))
        {
            foreach(GameObject s in GameObject.FindObjectOfType<CombinationWindow>().ComboSlots)
            {
                if (s.name.Equals("ComboSlot") && s.GetComponent<Slot>().IsEmpty())
                {
                    s.GetComponent<Slot>().AddItem(this);
                    slot.ClearSlot();
                    break;
                }
            }
            GameObject.FindObjectOfType<CombinationWindow>().updateSet();
        }
        else
        {
            GameObject.Find("InventoryWindow").GetComponent<Inventory>().AddItemToInventory(this);
            slot.ClearSlot();
        }
    }

    public override Sprite ReturnItemIcon()
    {
        switch (subType)
        {
            case RelicTypes.WEAPON:
                return Resources.Load<Sprite>("ItemIcons/tome");
            case RelicTypes.ARMOR:
                return Resources.Load<Sprite>("ItemIcons/scroll");
            default:
                return Resources.Load<Sprite>("ItemIcons/x");
        }
    }
}
