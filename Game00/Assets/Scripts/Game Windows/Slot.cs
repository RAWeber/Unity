using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

[Serializable]
public class Slot : MonoBehaviour, IPointerClickHandler
{
    private Stack<BaseItem> items = new Stack<BaseItem>();  //Holds stack of items in slot
    private Text itemText;  //Displays how many items in stack
    private Image itemIcon;

    private static Slot from, to;   //Used as temporary holders when swapping items

    private static GameObject toolTip;
    //private static GameObject hoverIcon;    //Icon shown when holding item
    private static GameObject stackSplitter;    //Display stacksplitter when shift clicking stack

    public GameObject toolTipPrefab;
    public GameObject hoverPrefab;   //Icon prefab
    public GameObject splitterPrefab;   //StackSplitter prefab

    //Items getter/setter
    public Stack<BaseItem> Items
    {
        get { return items; }
        set {
            items = value;
            SetSlot();
        }
    }

    void Awake()
    {
        itemText = this.transform.GetChild(1).GetComponent<Text>();
        itemIcon = this.transform.GetChild(0).GetComponent<Image>();
    }

    // Use this for initialization
    void Start()
    {
        RectTransform slotRect = GetComponent<RectTransform>();
        RectTransform iconRect = itemIcon.GetComponent<RectTransform>();
        RectTransform txtRect = itemText.GetComponent<RectTransform>();
        int txtScaleFactor = (int)(slotRect.sizeDelta.x * 0.60);
        itemText.fontSize = txtScaleFactor;

        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
        iconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
        iconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
    }

    //Get item type
    public BaseItem SlotItems()
    {
        return items.Count != 0 ? items.Peek() : null;
    }

    //Check if slot is empty
    public bool IsEmpty()
    {
        return items.Count == 0;
    }

    //Check if slot is full
    public bool isFull()
    {
        return !IsEmpty() ? SlotItems().MaxSize == items.Count : false;
    }

    private void SetSlot()
    {
        SetItemIcon();
        SetItemText();
        if (IsEmpty())
        {
            GameControl.inventory.EmptySlots++;
            RemoveToolTip();
        }
    }

    //Get sprite for item in slot
    private void SetItemIcon()
    {
        itemIcon.sprite = !IsEmpty() ? SlotItems().ReturnItemIcon() : this.GetComponent<Image>().sprite;
    }

    private void SetItemText()
    {
        itemText.text = this.items.Count > 1 ? this.items.Count.ToString() : string.Empty;
    }

    //Clears items in slot
    public void ClearSlot()
    {
        if (!IsEmpty())
        {
            items.Clear();
            SetSlot();
        }
    }

    //Uses item in slot
    private void UseItem()
    {
        if (!IsEmpty())
        {
            if (!name.Equals("ResultSlot") || GameControl.comboWindow.Created)
            {
                SlotItems().use(this);
                SetSlot();
            }
        }
    }

    public BaseItem RemoveItem()
    {
        BaseItem returnItem = items.Pop();
        SetSlot();
        return returnItem;
    }

    //Adds item to stack/slot
    public void AddItem(BaseItem item)
    {
        if (item != null)
        {
            items.Push(item);
            SetSlot();
        }
    }

    //Adds stack of items to slot
    private Stack<BaseItem> AddItemStack(Stack<BaseItem> items)
    {
        Stack<BaseItem> returnStack = items;
        if (returnStack.Peek().Equals(this.SlotItems()) && !this.isFull())
        {
            while (this.SlotItems().MaxSize > this.items.Count && returnStack.Count != 0)
            {
                this.items.Push(returnStack.Pop());
            }
            if (returnStack.Count == 0)
            {
                GameControl.inventory.EmptySlots++;
            }
        }
        else
        {
            returnStack = this.items;
            this.items = new Stack<BaseItem>(items);
        }
        SetSlot();
        return returnStack;
    }

    //Moves item to new slot in inventory/swap items
    public void MoveItem(Slot clickedSlot)
    {
        //If holding shift, make a splitStack window
        if (Input.GetKey(KeyCode.LeftShift) && !clickedSlot.IsEmpty() && !HoverIcon.hoverIcon)
        {
            from = clickedSlot;
            CreateSplitter();
            return;
        }

        if (clickedSlot.transform.parent.gameObject.Equals(GameControl.comboWindow.gameObject))
        {
            if (HoverIcon.hoverIcon != null)
            {
                if (HoverIcon.hoverIcon.GetComponent<Slot>().SlotItems().Type != BaseItem.ItemType.RELIC)
                {
                    return;
                }
            }
            else
            {
                if (clickedSlot.name.Equals("ResultSlot"))
                {
                    if (GameControl.comboWindow.Created)
                    {
                        from = clickedSlot;
                        CreateHoverIcon();
                        while (!from.IsEmpty())
                        {
                            HoverIcon.hoverIcon.GetComponent<Slot>().Items.Push(from.RemoveItem());
                        }
                        GameControl.comboWindow.Created = false;
                    }
                    return;
                }
            }
        }

        if (clickedSlot.transform.parent.gameObject.Equals(GameControl.equipment.gameObject))
        {
            if (HoverIcon.hoverIcon != null)
            {
                if (HoverIcon.hoverIcon.GetComponent<Slot>().SlotItems().Type == BaseItem.ItemType.EQUIPMENT)
                {
                    BaseEquipment item = (BaseEquipment)(HoverIcon.hoverIcon.GetComponent<Slot>().SlotItems());
                    if (!item.SubType.ToString().Equals(clickedSlot.name))
                    {
                        return;
                    }
                    GameControl.player.Stats.TransferStats(item.Stats);
                }
                else if (HoverIcon.hoverIcon.GetComponent<Slot>().SlotItems().Type == BaseItem.ItemType.WEAPON)
                {
                    if (clickedSlot.name.Equals("MAINHAND") || clickedSlot.name.Equals("OFFHAND"))
                    {
                        BaseWeapon item = (BaseWeapon)(HoverIcon.hoverIcon.GetComponent<Slot>().SlotItems());
                        if (item.Hand==2 && !clickedSlot.name.Equals("MAINHAND"))
                        {
                            return;
                        }
                        GameControl.player.Stats.TransferStats(item.Stats);
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
                    GameControl.player.Stats.RemoveStats(clickedSlot.GetComponent<Slot>().SlotItems().Stats);
                }
            }
        }
        //If from is null and slot has items in it, set from equal to slicked slot
        if (HoverIcon.hoverIcon == null)
        {
            if (!clickedSlot.IsEmpty())
            {
                from = clickedSlot;

                CreateHoverIcon();
                while (!from.IsEmpty())
                {
                    HoverIcon.hoverIcon.GetComponent<Slot>().Items.Push(from.RemoveItem());
                }
            }
        }
        //If the hoverIcon exists and has items in it, run code
        else if (HoverIcon.hoverIcon != null)
        {
            to = clickedSlot;

            //If the slot clicked has Items in it, swap the items held with the items in slot
            if (!to.IsEmpty())
            {
                Stack<BaseItem> tmpHover = HoverIcon.hoverIcon.GetComponent<Slot>().Items;
                CreateHoverIcon();
                Stack<BaseItem> leftOvers = to.AddItemStack(tmpHover);
                if (leftOvers.Count == 0)
                {
                    DestroyImmediate(HoverIcon.hoverIcon.gameObject);
                }
                else
                {
                    HoverIcon.hoverIcon.GetComponent<Slot>().Items = leftOvers;
                    HoverIcon.hoverIcon.GetComponentInChildren<Text>().text = HoverIcon.hoverIcon.GetComponent<Slot>().Items.Count > 1 ? HoverIcon.hoverIcon.GetComponent<Slot>().Items.Count.ToString() : string.Empty;
                }
                to = null;
            }
            //If clicked slot is empty place items into slot
            else
            {
                to.AddItemStack(HoverIcon.hoverIcon.GetComponent<Slot>().Items);
                CreateToolTip();
                to = null;
                from = null;
                Destroy(HoverIcon.hoverIcon.gameObject);
            }
        }
    }

    //create stack splitter under slot
    private void CreateSplitter()
    {
        Destroy(GameObject.Find("StackSplitter"));

        stackSplitter = (GameObject)Instantiate(splitterPrefab);
        stackSplitter.name = "StackSplitter";
        stackSplitter.transform.SetParent(GameControl.canvas.transform, true);
        stackSplitter.GetComponent<StackSplitter>().Slot = this;

        Vector2 position;
        Vector3 slotPos = new Vector3(this.transform.position.x + this.GetComponent<RectTransform>().sizeDelta.x / 2, this.transform.position.y - this.GetComponent<RectTransform>().sizeDelta.y * 2);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.GetComponentInParent<Canvas>().transform as RectTransform, slotPos, this.GetComponentInParent<Canvas>().worldCamera, out position);
        stackSplitter.transform.position = this.GetComponentInParent<Canvas>().transform.TransformPoint(position);

        RectTransform splitRect = stackSplitter.GetComponent<RectTransform>();
        splitRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100 * this.GetComponentInParent<Canvas>().scaleFactor);
        splitRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50 * this.GetComponentInParent<Canvas>().scaleFactor);
    }

    //Creates hover icon on mouse
    public void CreateHoverIcon()
    {
        RemoveToolTip();

        Instantiate(hoverPrefab);
        HoverIcon.hoverIcon.transform.GetChild(0).GetComponent<Image>().sprite = this.transform.GetChild(0).GetComponent<Image>().sprite;
        HoverIcon.hoverIcon.GetComponentInChildren<Text>().text = this.transform.GetChild(1).GetComponent<Text>().text;
        HoverIcon.hoverIcon.name = "HoverIcon";

        RectTransform hoverTransform = HoverIcon.hoverIcon.GetComponent<RectTransform>();
        RectTransform clickedTransform = this.GetComponent<RectTransform>();

        hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
        hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);
        HoverIcon.hoverIcon.transform.SetParent(GameControl.canvas.transform, true);
        HoverIcon.hoverIcon.transform.localScale = this.gameObject.transform.localScale;
    }

    //create tool tip
    public void CreateToolTip()
    {
        if (!IsEmpty())
        {
            toolTip = Instantiate(toolTipPrefab);
            toolTip.name = "ToolTip";
            toolTip.transform.SetParent(GameControl.canvas.transform, true);
            toolTip.GetComponent<ToolTip>().Slot = this;

            Vector2 position;
            Vector3 slotPos = new Vector3(this.transform.position.x + this.GetComponent<RectTransform>().sizeDelta.x, this.transform.position.y);
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

    public SlotData SaveInfo()
    {
        SlotData data = new SlotData();
        data.items = items;
        return data;
    }

    public void LoadInfo(SlotData data)
    {
        Items = data.items;
    }
}

[Serializable]
public class SlotData
{
    public Stack<BaseItem> items;  //Holds stack of items in slot
}