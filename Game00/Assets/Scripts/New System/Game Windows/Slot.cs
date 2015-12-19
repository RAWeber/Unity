using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    private Stack<BaseItem> items = new Stack<BaseItem>();  //Holds stack of items in slot
    public Text stackText;  //Displays how many items in stack

    private static Slot from, to;   //Used as temporary holders when swapping items

    private static GameObject toolTip;
    private static GameObject hoverIcon;    //Icon shown when holding item
    private static GameObject stackSplitter;    //Display stacksplitter when shift clicking stack

    public GameObject toolTipPrefab;
    public GameObject hoverPrefab;   //Icon prefab
    public GameObject splitterPrefab;   //StackSplitter prefab

    //Items getter/setter
    public Stack<BaseItem> Items
    {
        get { return items; }
        set { items = value; }
    }

    // Use this for initialization
    void Start()
    {
        RectTransform slotRect = GetComponent<RectTransform>();
        RectTransform txtRect = stackText.GetComponent<RectTransform>();
        RectTransform iconRect = this.transform.GetChild(0).GetComponent<RectTransform>();
        int txtScaleFactor = (int)(slotRect.sizeDelta.x * 0.60);
        stackText.fontSize = txtScaleFactor;

        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
        iconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
        iconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
    }

    //Get item type
    public BaseItem itemsInStack()
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
        return itemsInStack().MaxSize == items.Count;
    }

    //Adds item to stack/slot
    public void AddItem(BaseItem item)
    {
        if (item != null)
        {
            items.Push(item);
            this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ReturnItemIcon(item);
            if (items.Count > 1)
            {
                stackText.text = items.Count.ToString();
            }
        }
        else
        {
            Debug.Log("Added null item to slot");
        }
    }

    //Adds stack of items to slot
    public Stack<BaseItem> SetItems(Stack<BaseItem> items)
    {
        Stack<BaseItem> returnStack = items;
        if (this.items.Count != 0 && this.itemsInStack() == items.Peek())
        {
            while (this.itemsInStack().MaxSize > this.items.Count && returnStack.Count != 0)
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
            this.items = new Stack<BaseItem>(items);
        }
        stackText.text = this.items.Count > 1 ? this.items.Count.ToString() : string.Empty;
        this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ReturnItemIcon(itemsInStack());
        return returnStack;
    }

    public BaseItem RemoveItem()
    {
        BaseItem returnItem = items.Pop();
        stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
        if (items.Count == 0)
        {
            ClearSlot();
        }
        return returnItem;
    }

    //Get sprite for item in slot
    private Sprite ReturnItemIcon(BaseItem item)
    {
        return item.ReturnItemIcon();
    }

    //Uses item in slot
    private void UseItem()
    {
        if (!IsEmpty())
        {
            if (name.Equals("ResultSlot"))
            {
                if (GameObject.Find("CombinationWindow").GetComponent<CombinationWindow>().Created)
                {
                    items.Peek().use(this);
                    stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
                }
            }
            else
            {
                items.Peek().use(this);

                stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
            }

            if (IsEmpty())
            {
                this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
                RemoveToolTip();
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

    //Moves item to new slot in inventory/swap items
    public void MoveItem(Slot clickedSlot)
    {
        //If holding shift, make a splitStack window
        if (Input.GetKey(KeyCode.LeftShift) && !clickedSlot.IsEmpty() && !GameObject.Find("HoverIcon"))
        {
            from = clickedSlot;
            CreateSplitter(clickedSlot);
            return;
        }

        if (clickedSlot.transform.parent.gameObject.Equals(GameObject.Find("CombinationWindow")))
        {
            if (hoverIcon != null)
            {
                if (hoverIcon.GetComponent<Slot>().itemsInStack().Type != BaseItem.ItemType.RELIC)
                {
                    return;
                }
            }
            else
            {
                if (clickedSlot.name.Equals("ResultSlot"))
                {
                    if (GameObject.Find("CombinationWindow").GetComponent<CombinationWindow>().Created)
                    {
                        from = clickedSlot;
                        CreateHoverIcon(from);
                        while (!from.IsEmpty())
                        {
                            GameObject.Find("HoverIcon").GetComponent<Slot>().Items.Push(from.RemoveItem());
                        }
                        GameObject.Find("CombinationWindow").GetComponent<CombinationWindow>().Created = false;
                    }
                    return;
                }
            }
        }

        if (clickedSlot.transform.parent.gameObject.Equals(GameObject.Find("EquipmentWindow")))
        {
            if (hoverIcon != null)
            {
                if (hoverIcon.GetComponent<Slot>().itemsInStack().Type == BaseItem.ItemType.EQUIPMENT)
                {
                    BaseEquipment item = (BaseEquipment)(hoverIcon.GetComponent<Slot>().itemsInStack());
                    if (!item.SubType.ToString().Equals(clickedSlot.name))
                    {
                        return;
                    }
                    GameObject.FindObjectOfType<Player>().Stats.TransferStats(item.Stats);
                }
                else if (hoverIcon.GetComponent<Slot>().itemsInStack().Type == BaseItem.ItemType.WEAPON)
                {
                    if (clickedSlot.name.Equals("MAINHAND") || clickedSlot.name.Equals("OFFHAND"))
                    {
                        BaseWeapon item = (BaseWeapon)(hoverIcon.GetComponent<Slot>().itemsInStack());
                        if (item.Hand==2 && !clickedSlot.name.Equals("MAINHAND"))
                        {
                            return;
                        }
                        GameObject.FindObjectOfType<Player>().Stats.TransferStats(item.Stats);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (!clickedSlot.IsEmpty())
                {
                    GameObject.FindObjectOfType<Player>().Stats.RemoveStats(clickedSlot.GetComponent<Slot>().itemsInStack().Stats);
                }
            }
        }
        //If from is null and slot has items in it, set from equal to slicked slot
        if (hoverIcon == null)
        {
            if (!clickedSlot.IsEmpty())
            {
                from = clickedSlot;

                CreateHoverIcon(from);
                while (!from.IsEmpty())
                {
                    hoverIcon.GetComponent<Slot>().Items.Push(from.RemoveItem());
                }
            }
        }
        //If the hoverIcon exists and has items in it, run code
        else if (hoverIcon != null)
        {
            to = clickedSlot;

            //If the slot clicked has Items in it, swap the items held with the items in slot
            if (to.Items.Count != 0)
            {
                Stack<BaseItem> tmpHover = hoverIcon.GetComponent<Slot>().Items;
                CreateHoverIcon(to);
                hoverIcon.GetComponent<Slot>().Items = to.SetItems(tmpHover);
                if (hoverIcon.GetComponent<Slot>().Items.Count == 0)
                {
                    DestroyImmediate(GameObject.Find("HoverIcon"));
                }
                else
                {
                    hoverIcon.GetComponentInChildren<Text>().text = hoverIcon.GetComponent<Slot>().Items.Count > 1 ? hoverIcon.GetComponent<Slot>().Items.Count.ToString() : string.Empty;
                }
                to = null;
            }
            //If clicked slot is empty place items into slot
            else
            {
                to.SetItems(hoverIcon.GetComponent<Slot>().Items);
                CreateToolTip(to);
                to = null;
                from = null;
                Destroy(GameObject.Find("HoverIcon"));
            }
        }
    }

    //create stack splitter under slot
    private void CreateSplitter(Slot clickedSlot)
    {
        stackSplitter = (GameObject)Instantiate(splitterPrefab);
        stackSplitter.name = "StackSplitter";
        stackSplitter.transform.SetParent(GameObject.Find("Canvas").transform, true);
        stackSplitter.GetComponent<StackSplitter>().Slot = clickedSlot;

        Vector2 position;
        Vector3 slotPos = new Vector3(clickedSlot.transform.position.x + clickedSlot.GetComponent<RectTransform>().sizeDelta.x / 2, clickedSlot.transform.position.y - clickedSlot.GetComponent<RectTransform>().sizeDelta.y * 2);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.GetComponentInParent<Canvas>().transform as RectTransform, slotPos, this.GetComponentInParent<Canvas>().worldCamera, out position);
        stackSplitter.transform.position = this.GetComponentInParent<Canvas>().transform.TransformPoint(position);

        RectTransform splitRect = stackSplitter.GetComponent<RectTransform>();
        splitRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100 * this.GetComponentInParent<Canvas>().scaleFactor);
        splitRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50 * this.GetComponentInParent<Canvas>().scaleFactor);
    }

    //Creates hover icon on mouse
    public void CreateHoverIcon(Slot slot)
    {
        RemoveToolTip();
        DestroyImmediate(GameObject.Find("HoverIcon"));
        hoverIcon = (GameObject)Instantiate(hoverPrefab);
        hoverIcon.GetComponent<Image>().sprite = slot.transform.GetChild(0).GetComponent<Image>().sprite;
        hoverIcon.GetComponentInChildren<Text>().text = slot.transform.GetChild(1).GetComponent<Text>().text;
        hoverIcon.name = "HoverIcon";

        RectTransform hoverTransform = hoverIcon.GetComponent<RectTransform>();
        RectTransform clickedTransform = slot.GetComponent<RectTransform>();

        hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
        hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);
        hoverIcon.transform.SetParent(GameObject.Find("Canvas").transform, true);
        hoverIcon.transform.localScale = slot.gameObject.transform.localScale;
    }

    //create tool tip
    public void CreateToolTip(Slot itemSlot)
    {
        if (!itemSlot.IsEmpty())
        {
            toolTip = (GameObject)Instantiate(toolTipPrefab);
            toolTip.name = "ToolTip";
            toolTip.transform.SetParent(GameObject.Find("Canvas").transform, true);
            toolTip.GetComponent<ToolTip>().Slot = itemSlot;

            Vector2 position;
            Vector3 slotPos = new Vector3(itemSlot.transform.position.x + itemSlot.GetComponent<RectTransform>().sizeDelta.x, itemSlot.transform.position.y);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(this.GetComponentInParent<Canvas>().transform as RectTransform, slotPos, this.GetComponentInParent<Canvas>().worldCamera, out position);
            toolTip.transform.position = this.GetComponentInParent<Canvas>().transform.TransformPoint(position);
        }
    }

    //remove tool tip
    public void RemoveToolTip()
    {
        Destroy(GameObject.Find("ToolTip"));
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
            MoveItem(this);
        }
    }
}
