using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IPointerClickHandler {

    private Stack<Item> items;
    public Text stackText;

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
        int txtScaleFactor = (int)(slotRect.sizeDelta.x * 0.60);
        stackText.resizeTextMaxSize = txtScaleFactor;
        stackText.resizeTextMinSize = txtScaleFactor;
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public Item itemsInStack()
    {
        return items.Peek();
    }

    public bool IsEmpty()
    {
        return items.Count == 0;
    }

    public bool isFull()
    {
        return itemsInStack().maxSize == items.Count;
    }

    public void AddItem(Item item)
    {
        items.Push(item);
        this.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = ReturnItemIcon(item);
        if (items.Count>1)
        {
            stackText.text = items.Count.ToString();
        }
    }

    public void AddItems(Stack<Item> items)
    {
        this.items = new Stack<Item>(items);
        stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
        this.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = ReturnItemIcon(itemsInStack());

    }

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

    private void UseItem()
    {
        if (!IsEmpty())
        {
            items.Pop().use();

            stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

            if (IsEmpty())
            {
                this.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = null;
                FindObjectOfType<Inventory>().EmptySlots++;
            }
        }
    }

    public void ClearSlot()
    {
        items.Clear();
        this.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = null;
        stackText.text = string.Empty;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
    }
}
