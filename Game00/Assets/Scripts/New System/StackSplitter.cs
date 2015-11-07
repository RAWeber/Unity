using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class StackSplitter : MonoBehaviour
{
    private int max;
    private int counter;

    public GameObject slot;
    public Text splitterText;

    // Use this for initialization
    void Start()
    {
        max = slot.GetComponent<Slot>().Items.Count;
        counter = Int32.Parse(splitterText.text);
    }

    // Update is called once per frame
    void Update()
    {

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
        if (counter != 0)
        {
            Stack<Item> splitList = new Stack<Item>();
            GetComponentInParent<Canvas>().GetComponentInChildren<Inventory>().createHoverIcon(slot.GetComponent<Slot>());
            GameObject.Find("HoverIcon").GetComponentInChildren<Text>().text = counter > 1 ? counter.ToString() : string.Empty; ;

            for (; counter > 0; counter--)
            {
                //splitList.Push(slot.GetComponent<Slot>().RemoveItem());
                GameObject.Find("HoverIcon").GetComponent<Slot>().Items.Push(slot.GetComponent<Slot>().RemoveItem());
            }
            //GameObject.Find("HoverIcon").GetComponent<Slot>().Items = splitList;
            if (GameObject.Find("HoverIcon").GetComponent<Slot>().Items.Count != GameObject.Find("HoverIcon").GetComponent<Slot>().Items.Peek().maxSize)
            {
                FindObjectOfType<Inventory>().EmptySlots--;
            }
        }
        Destroy(GameObject.Find("StackSplitter"));
    }
}
