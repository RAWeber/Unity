using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseRelic : BaseItem {

    public enum RelicTypes : int
    {
        WEAPON, ARMOR
    }

    private RelicTypes subType;


    public BaseRelic(Dictionary<string, string> itemDictionary) : base(itemDictionary)
    {
        subType = (RelicTypes)System.Enum.Parse(typeof(RelicTypes), itemDictionary["SubType"]);
    }

    public override void use()
    {
        Debug.Log("This be a relic");
    }

    public override Sprite ReturnItemIcon()
    {
        switch (subType)
        {
            case RelicTypes.WEAPON:
                return Resources.Load<Sprite>("ItemIcons/tome");
            default:
                return Resources.Load<Sprite>("ItemIcons/x");
        }
    }
}
