using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BaseWeapon : BaseItem {

    public enum WeaponTypes : int
    {
        SWORD, DAGGER, AXE, BOW, CROSSBOW, BLOWGUN, STAFF, WAND, WHIP
    }

    private WeaponTypes subType;


    public BaseWeapon(Dictionary<string, string> itemDictionary) : base(itemDictionary)
    {
        subType = (WeaponTypes)System.Enum.Parse(typeof(WeaponTypes), itemDictionary["SubType"]);
    }

    public override void use()
    {
        Debug.Log("Weapon equipped");
    }

    public override Sprite ReturnItemIcon()
    {
        switch (subType)
        {
            case WeaponTypes.SWORD:
                return Resources.Load<Sprite>("ItemIcons/upg_sword");
            case WeaponTypes.DAGGER:
                return Resources.Load<Sprite>("ItemIcons/upg_dagger");
            case WeaponTypes.AXE:
                return Resources.Load<Sprite>("ItemIcons/upg_axe");
            case WeaponTypes.BOW:
                return Resources.Load<Sprite>("ItemIcons/upg_bow");
            case WeaponTypes.CROSSBOW:
                return Resources.Load<Sprite>("ItemIcons/bow");
            case WeaponTypes.BLOWGUN:
                return Resources.Load<Sprite>("ItemIcons/tools");
            case WeaponTypes.STAFF:
                return Resources.Load<Sprite>("ItemIcons/upg_spear");
            case WeaponTypes.WAND:
                return Resources.Load<Sprite>("ItemIcons/upg_wand");
            case WeaponTypes.WHIP:
                return Resources.Load<Sprite>("ItemIcons/upg_hammer");
            default:
                return Resources.Load<Sprite>("ItemIcons/x");
        }
    }
}
