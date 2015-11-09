using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseConsumable : BaseItem
{
    public enum ConsumableTypes
    {
        HEALTH, MANA
    }

    public ConsumableTypes subType;

    public BaseConsumable(Dictionary<string, string> itemDictionary) : base(itemDictionary)
    {
        subType = (ConsumableTypes)System.Enum.Parse(typeof(ConsumableTypes), itemDictionary["SubType"]);
    }

    public override void use()
    {
        switch (subType)
        {
            case ConsumableTypes.HEALTH:
                Debug.Log("Health potion used!");
                break;
            case ConsumableTypes.MANA:
                Debug.Log("Mana potion used!");
                break;
            default:
                Debug.Log("Some type of potion was used");
                break;
        }
    }

    public override Sprite ReturnItemIcon()
    {
        switch (subType)
        {
            case ConsumableTypes.HEALTH:
                return Resources.Load<Sprite>("ItemIcons/potionRed");
            case ConsumableTypes.MANA:
                return Resources.Load<Sprite>("ItemIcons/potionBlue");
            default:
                return Resources.Load<Sprite>("ItemIcons/x");
        }

    }
}
