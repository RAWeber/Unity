using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class StackSplitter : MonoBehaviour {

    private int max;
    private int counter;


    public Slot slot;
    public Text splitterText;

	// Use this for initialization
	void Start () {
        max = slot.Items.Count;
        counter = Int32.Parse(splitterText.text);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void incrementCounter()
    {
        if (counter < max)
        {
            counter++;
            splitterText.text = counter.ToString();
        }
    }

    public void decrementCounter()
    {
        if (counter > 0)
        {
            counter--;
            splitterText.text = counter.ToString();
        }
    }

    public void splitStack()
    {
        List<Item> splitList = new List<Item>();
        GetComponentInParent<Canvas>().GetComponentInChildren<Inventory>().createHoverIcon(slot);
        GameObject.Find("HoverIcon").GetComponentInChildren<Text>().text = counter > 1 ? counter.ToString() : string.Empty; ;

        for (; counter > 0; counter--)
        {
            splitList.Add(slot.RemoveItem());
        }
        Destroy(GameObject.Find("StackSplitter"));
    }
}
