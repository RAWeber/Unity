using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    private Stack<Item> items = new Stack<Item>();  //Holds stack of items in slot
    public Text stackText;  //Displays how many items in stack

    //Items getter/setter
    public Stack<Item> Items
    {
        get
        {
            return items;
        }

        set
        {
            items = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        RectTransform slotRect = GetComponent<RectTransform>();
        RectTransform txtRect = stackText.GetComponent<RectTransform>();
        RectTransform iconRect = this.transform.GetChild(0).GetComponent<RectTransform>();
        int txtScaleFactor = (int)(slotRect.sizeDelta.x * 0.60);
        stackText.resizeTextMaxSize = txtScaleFactor;
        stackText.resizeTextMinSize = txtScaleFactor;
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
        iconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
        iconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
    }

    void Awake()
    {
        //items = new Stack<Item>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Get item type
    public Item itemsInStack()
    {
        if(items.Count!=0)
            return items.Peek();
        return null;
    }

    //Check if slot is empty
    public bool IsEmpty()
    {
        if (items == null) return true;
        return items.Count == 0;
    }

    //Check if slot is full
    public bool isFull()
    {
        return itemsInStack().maxSize == items.Count;
    }

    //Adds item to stack/slot
    public void AddItem(Item item)
    {
        items.Push(item);
        this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ReturnItemIcon(item);
        if (items.Count > 1)
        {
            stackText.text = items.Count.ToString();
        }
    }

    //Adds stack of items to slot
    public Stack<Item> SetItems(Stack<Item> items)
    {
        Stack<Item> returnStack = items;
        if (this.items.Count != 0 && this.itemsInStack() == items.Peek())
        {
            while (this.itemsInStack().maxSize > this.items.Count && returnStack.Count != 0)
            {
                this.items.Push(returnStack.Pop());
            }
            if (returnStack.Count == 0)
            {
                FindObjectOfType<Inventory>().EmptySlots++;
            }
        }
        else
        {
            returnStack = this.items;
            this.items = new Stack<Item>(items);
        }
        stackText.text = this.items.Count > 1 ? this.items.Count.ToString() : string.Empty;
        this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ReturnItemIcon(itemsInStack());
        return returnStack;
    }

    public Item RemoveItem()
    {
        Item returnItem = items.Pop();
        stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
        if (items.Count == 0)
        {
            ClearSlot();
        }
        return returnItem;
    }

    //Get sprite for item in slot
    private Sprite ReturnItemIcon(Item item)
    {
        Sprite icon = new Sprite();
        if (item.type == Item.ItemType.HEALTH)
        {
            icon = Resources.Load<Sprite>("ItemIcons/potionRed");
        }
        else if (item.type == Item.ItemType.MANA)
        {
            icon = Resources.Load<Sprite>("ItemIcons/potionBlue");
        }
        return icon;
    }

    //Uses item in slot
    private void UseItem()
    {
        if (!IsEmpty())
        {
            items.Pop().use();

            stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

            if (IsEmpty())
            {
                this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = null;
                FindObjectOfType<Inventory>().EmptySlots++;
            }
        }
    }

    //Clears items in slot
    public void ClearSlot()
    {
        items.Clear();
        this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
        stackText.text = string.Empty;
    }

    //Right click to use
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("HoverIcon"))
        {
            UseItem();
        }
        else
        {
            GetComponentInParent<Canvas>().GetComponentInChildren<Inventory>().MoveItem(this.gameObject);
        }
    }
}
