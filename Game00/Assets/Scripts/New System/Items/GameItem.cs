﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameItem : MonoBehaviour {
    public string itemName;
    private BaseItem item;

    public BaseItem Item
    {
        get
        {
            return item;
        }

        set
        {
            item = value;
        }
    }

    // Use this for initialization
    void Start () {
        item = ItemDatabase.itemDatabase[itemName];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
