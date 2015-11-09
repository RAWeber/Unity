using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseItem
{
    //Enum for types of items
    public enum ItemType
    {
        EQUIPMENT, WEAPON, CONSUMABLE, RELIC
    }

    private string itemName;
    private string description;
    private int itemID;
    public ItemType type;   //Item type
    public int maxSize; //Max stack size

    public string ItemName
    {
        get
        {
            return itemName;
        }

        set
        {
            itemName = value;
        }
    }

    public BaseItem()
    {
        //Debug.Log("HMMMM");
    }

    public BaseItem(Dictionary<string, string> itemDictionary)
    {
        itemName = itemDictionary["ItemName"];
        itemID = int.Parse(itemDictionary["ItemID"]);
        description= itemDictionary["ItemName"];
        type = (ItemType)System.Enum.Parse(typeof(ItemType), itemDictionary["ItemType"]);
        maxSize = int.Parse(itemDictionary["MaxSize"]);
        
    }

    //Use item
    public abstract void use();

    public abstract Sprite ReturnItemIcon();
}
