using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseItemOld {

    private string name;
    private string description;
    //private int itemID;

    public enum ItemTypes
    {
        EQUIPMENT, WEAPON, CONSUMABLE
    }

    private ItemTypes itemType;

    public BaseItemOld() { }

    public BaseItemOld(string n)
    {
        name = n;
        itemType = ItemTypes.WEAPON;
    }

    public BaseItemOld(Dictionary<string,string> itemDictionary)
    {
        name = itemDictionary["ItemName"];
        //itemID = int.Parse(itemDictionary["ItemID"]);
        itemType = (ItemTypes)System.Enum.Parse(typeof(ItemTypes),itemDictionary["ItemType"]);
    }

    public string Name { get; set; }

    public string GetName()
    {
        return name;
    }

    public string Description { get; set; }

    public int ItemID { get; set; }

    public ItemTypes ItemType { get; set; }

    public ItemTypes GetItemType()
    {
        return itemType;
    }

}
