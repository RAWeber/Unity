﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseItem
{
    //Enum for types of items
    public enum ItemType
    {
        EQUIPMENT, WEAPON, CONSUMABLE, RELIC
    }

    public enum Quality
    {
        COMMON,UNCOMMON,RARE
    }

    private StatCollection stats;

    private string itemName;
    private string description;
    private int itemID;
    private ItemType type;   //Item type
    private Quality quality;
    private int maxSize; //Max stack size

    public string ItemName
    {
        get { return itemName; }
        //set { itemName = value; }
    }

    public string Description
    {
        get { return description; }
        //set { description = value; }
    }

    public int MaxSize
    {
        get { return maxSize; }
        //set { maxSize = value; }
    }

    public ItemType Type
    {
        get { return type; }
        //set { type = value; }
    }

    public StatCollection Stats
    {
        get { return stats; }
        protected set { stats = value; }
    }

    public int ItemID
    {
        get { return itemID; }
        //set { itemID = value; }
    }

    public BaseItem(BaseItem item)
    {
        this.itemID = item.ItemID;
        this.itemName = item.ItemName;
        this.description = item.Description;
        this.type = item.Type;
        this.maxSize = item.MaxSize;
        stats = new StatCollection();
    }

    public BaseItem(Dictionary<string, string> itemDictionary)
    {
        itemID = int.Parse(itemDictionary["ItemID"]);
        itemName = itemDictionary["ItemName"];
        description= itemDictionary["Description"];
        type = (ItemType)System.Enum.Parse(typeof(ItemType), itemDictionary["ItemType"]);
        quality = (Quality)System.Enum.Parse(typeof(Quality), itemDictionary["Quality"]);
        maxSize = int.Parse(itemDictionary["MaxSize"]);
        stats = new StatCollection();
    }

    //Use item
    public abstract void use(Slot slot);

    public abstract Sprite ReturnItemIcon();

    public virtual string getToolTip()
    {
        string color=string.Empty;
        string statList=stats.StatList();

        switch (quality)
        {
            case Quality.COMMON:
                color = "white";
                break;
            case Quality.UNCOMMON:
                color = "lime";
                break;
            case Quality.RARE:
                color = "blue";
                break;
        }

        return string.Format("<color="+color+"><size=16>{0}</size></color>\n<color=black><size=14>{1}</size></color><color=green><size=14>{2}</size></color>", itemName, description, statList);
    }
}
