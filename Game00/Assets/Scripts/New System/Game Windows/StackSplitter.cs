using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class StackSplitter : MonoBehaviour
{
    private int max;
    private int counter;

    private Slot slot;
    private Text splitterText;

    public Slot Slot
    {
        get { return slot; }
        set { slot = value; }
    }

    // Use this for initialization
    void Start()
    {
        max = Slot.Items.Count;
        splitterText = this.transform.GetChild(4).GetComponent<Text>();
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
            //Stack<BaseItem> splitList = new Stack<BaseItem>();
            Slot.CreateHoverIcon(Slot);
            GameObject.Find("HoverIcon").GetComponentInChildren<Text>().text = counter > 1 ? counter.ToString() : string.Empty; ;

            for (; counter > 0; counter--)
            {
                //splitList.Push(slot.GetComponent<Slot>().RemoveItem());
                GameObject.Find("HoverIcon").GetComponent<Slot>().Items.Push(Slot.RemoveItem());
            }
            //GameObject.Find("HoverIcon").GetComponent<Slot>().Items = splitList;
            if (GameObject.Find("HoverIcon").GetComponent<Slot>().Items.Count != GameObject.Find("HoverIcon").GetComponent<Slot>().Items.Peek().MaxSize)
            {
                FindObjectOfType<Inventory>().EmptySlots--;
            }
        }
        Destroy(GameObject.Find("StackSplitter"));
    }
}
