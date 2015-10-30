using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IPointerClickHandler {

    private Stack<Item> items;  //Holds stack of items in slot
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
    void Start () {

        items = new Stack<Item>();
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
	
	// Update is called once per frame
	void Update () {
	
	}

    //Get item type
    public Item itemsInStack()
    {
        return items.Peek();
    }

    //Check if slot is empty
    public bool IsEmpty()
    {
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
        if (items.Count>1)
        {
            stackText.text = items.Count.ToString();
        }
    }

    //Adds stack of items to slot
    public void AddItems(Stack<Item> items)
    {
        this.items = new Stack<Item>(items);
        stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
        this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ReturnItemIcon(itemsInStack());

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
        else if(item.type == Item.ItemType.MANA)
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
