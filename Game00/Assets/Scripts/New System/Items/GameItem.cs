using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameItem : MonoBehaviour
{
    public string itemName;
    private BaseItem item;

    public BaseItem Item
    {
        get { return item; }
        //set { item = value; }
    }

    // Use this for initialization
    void Start()
    {
        Physics.IgnoreCollision(GameObject.FindObjectOfType<Player>().GetComponent<Collider>(), this.GetComponent<Collider>());
        try
        {
            switch (ItemDatabase.itemDatabase[itemName].Type)
            {
                case BaseItem.ItemType.WEAPON:
                    item = new BaseWeapon((BaseWeapon)ItemDatabase.itemDatabase[itemName]);
                    break;
                case BaseItem.ItemType.EQUIPMENT:
                    item = new BaseEquipment(ItemDatabase.itemDatabase[itemName]);
                    break;
                case BaseItem.ItemType.RELIC:
                    item = new BaseRelic(ItemDatabase.itemDatabase[itemName]);
                    for (int i = 0; i < UnityEngine.Random.Range(1, 3); i++)
                    {
                        int randStat = UnityEngine.Random.Range((int)StatType.STRENGTH, (int)StatType.DEFENSE+1);
                        item.Stats.AddStat<ModifiableStat>((StatType)randStat, UnityEngine.Random.Range(1, 11));
                    }
                    break;
                case BaseItem.ItemType.CONSUMABLE:
                    item = new BaseConsumable(ItemDatabase.itemDatabase[itemName]);
                    break;
            }
        }
        catch (Exception)
        {
            Debug.Log("No item of name "+itemName+" within itemDatabase");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        this.GetComponent<SphereCollider>().attachedRigidbody.isKinematic = true;
        this.GetComponent<SphereCollider>().isTrigger = true;
        this.GetComponent<SphereCollider>().radius = 5;
        Physics.IgnoreCollision(GameObject.FindObjectOfType<Player>().GetComponent<Collider>(), this.GetComponent<Collider>(), false);
    }
}
