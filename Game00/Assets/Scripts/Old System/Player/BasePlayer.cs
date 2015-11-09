using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasePlayer : MonoBehaviour{

    //private List<BaseStat> stats = new List<BaseStat>();
    private List<BaseItemOld> inventory = new List<BaseItemOld>();

    // Use this for initialization
    void Start()
    {
        inventory.Add(new BaseItemOld("Sword"));
        inventory.Add(new BaseItemOld("Amor"));
    }

    // Update is called once per frame
    void Update()
    {

    }


    public List<BaseItemOld> Inventory { get; set; }

    public List<BaseItemOld> GetInventory()
    {
        return inventory;
    }
}
