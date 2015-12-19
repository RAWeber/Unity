using UnityEngine;
using System.Collections;

public enum StatType {
    //Vital - CurrentState, Linkable, Modifiable
    HEALTH,
    MANA,
    ENERGY,

    //Stats - Linkable, Modifiable
    ARMOR,
    ATTACKPOWER,
    ATTACKSPEED,
    ATTACKRANGE,

    //Attributes - Modifiable
    STRENGTH,
    INTELLIGENCE,
    DEXTERITY,
    STAMINA,
    DEFENSE,
}
