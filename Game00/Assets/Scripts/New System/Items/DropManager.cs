using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class DropManager : MonoBehaviour {

    public GameObject itemDrop;
    public static List<String> relicNames = new List<String>();

    public void CreateDrop(Vector3 vector)
    {
        GameObject item = Instantiate(itemDrop);
        Transform itemRect = item.GetComponent<Transform>();
        itemRect.position = vector;
        item.GetComponent<GameItem>().itemName = relicNames[UnityEngine.Random.Range(0,relicNames.Count)];
        item.name = item.GetComponent<GameItem>().itemName;
    }
}
