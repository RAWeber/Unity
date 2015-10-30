using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    public enum ItemType { HEALTH, MANA }   //Enum for types of items

    public ItemType type;   //Item type
    public int maxSize; //Max stack size

    //Use item
	public void use()
    {
        switch (type)
        {
            case ItemType.MANA:
                Debug.Log("Used a mana potion");
                break;
            case ItemType.HEALTH:
                Debug.Log("Used a health potion");
                break;
        }
    }
}
