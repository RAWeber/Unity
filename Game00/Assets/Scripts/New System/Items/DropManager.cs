using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class DropManager : MonoBehaviour {

    public GameObject itemDrop;

    public void CreateDrop(Vector3 vector)
    {
        GameObject item = Instantiate(itemDrop);
        Transform itemRect = item.GetComponent<Transform>();
        itemRect.position = vector;
        item.GetComponent<GameItem>().itemName = ItemDatabase.relicNames[UnityEngine.Random.Range(0,ItemDatabase.relicNames.Count)];
        item.name = item.GetComponent<GameItem>().itemName;
    }
}
