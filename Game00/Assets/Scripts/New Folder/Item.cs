using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    public enum ItemType { HEALTH, MANA }

    public ItemType type;
    public int maxSize;

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
