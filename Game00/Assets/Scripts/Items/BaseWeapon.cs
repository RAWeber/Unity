using UnityEngine;
using System.Collections;

public class BaseWeapon : BaseStatItem {

    public enum WeaponTypes : int
    {
        SWORD, DAGGER, AXE, BOW, CROSSBOW, BLOWGUN
    }

    private WeaponTypes weaponType;

    public WeaponTypes WeaponType { get; set; }
}
