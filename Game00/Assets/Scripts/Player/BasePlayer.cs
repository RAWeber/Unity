using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasePlayer : MonoBehaviour{

    private List<BaseStat> stats = new List<BaseStat>();
    private List<BaseItem> inventory = new List<BaseItem>();

    // Use this for initialization
    void Start()
    {
        inventory.Add(new BaseItem("Sword"));
        inventory.Add(new BaseItem("Amor"));
    }

    // Update is called once per frame
    void Update()
    {

    }


    public List<BaseItem> Inventory { get; set; }

    public List<BaseItem> GetInventory()
    {
        return inventory;
    }
}
