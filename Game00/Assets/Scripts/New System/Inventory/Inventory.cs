using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    public int slotTotal;   //Total amount of slots in inventory
    public int columns;     //Total number of columns
    public int slotSize;    //Size of slot side
    public int slotPadding; //Space between slots
    public int inventoryTitle;  //Title bar
    public GameObject slotPrefab;   //Slot prefab
    public GameObject hoverPrefab;   //Icon prefab
    public GameObject splitterPrefab;   //StackSplitter prefab
    public Canvas canvas;   //Canvas
    public EventSystem eventSystem; //Event System

    private CanvasGroup canvasGroup;    //Canvas group to open/close
    private List<GameObject> allSlots;  //List of slots in inventory
    private static GameObject stackSplitter;    //Display stacksplitter when shift clicking stack
    private GameObject hoverIcon;    //Icon shown when holding item
    private Slot from, to;   //Used as temporary holders when swapping items
    private int emptySlots; //Total number of empty slots in inventory

    //EmptySlots Getter/Setter
    public int EmptySlots
    {
        get
        {
            return emptySlots;
        }

        set
        {
            emptySlots = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        CreateWindow();
        canvasGroup = this.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        //Delete inventory items by clicking off inventory
        if (Input.GetMouseButton(0))
        {
            if (!eventSystem.IsPointerOverGameObject(-1) && hoverIcon != null)
            {
                Destroy(GameObject.Find("HoverIcon"));
                emptySlots++;
            }
        }

        //Move hover icon with mouse
        if (hoverIcon != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            hoverIcon.transform.position = canvas.transform.TransformPoint(position);
        }

        //Open & close inventory with 'I'
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (canvasGroup.alpha == 0)
            {
                open();
            }
            else
            {
                close();
            }
        }
    }

    //Create inventory window with all slots
    private void CreateWindow()
    {
        allSlots = new List<GameObject>();
        emptySlots = slotTotal;

        RectTransform window = this.gameObject.GetComponent<RectTransform>();
        int width = columns * slotSize + columns * slotPadding + slotPadding;
        int height = (slotTotal / columns) * slotSize + (slotTotal / columns) * slotPadding + slotPadding + inventoryTitle;
        window.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        window.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

        for (int y = 0; y < slotTotal / columns; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                newSlot.name = "Slot";
                newSlot.transform.SetParent(this.transform);
                slotRect.localPosition = new Vector3(slotPadding * (x + 1) + slotSize * x, -slotPadding * (y + 1) - slotSize * y - inventoryTitle);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);
                allSlots.Add(newSlot);
            }
        }
    }

    //Add item to inventory
    public void AddItem(GameItem item)
    {
        if (item.Item.maxSize == 1)
        {
            PlaceEmpty(item);
        }
        else
        {
            PlaceStack(item);
        }
    }

    //Add item into empty inventory slot
    private void PlaceEmpty(GameItem item)
    {
        if (emptySlots > 0)
        {
            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();
                if (tmp.IsEmpty())
                {
                    tmp.AddItem(item);
                    emptySlots--;
                    return;
                }
            }
        }
    }

    //Add item into stack in inventory
    private void PlaceStack(GameItem item)
    {
        foreach (GameObject slot in allSlots)
        {
            Slot tmp = slot.GetComponent<Slot>();
            if (!tmp.IsEmpty())
            {
                if (tmp.itemsInStack().Item.type == item.Item.type && !tmp.isFull())
                {
                    tmp.AddItem(item);
                    return;
                }
            }
        }
        if (emptySlots > 0)
        {
            PlaceEmpty(item);
        }
    }

    //Moves item to new slot in inventory/swap items
    public void MoveItem(GameObject clicked)
    {
        Slot clickedSlot = clicked.GetComponent<Slot>();    //Create slot from clicked gameobject

        //If holding shift, make a splitStack window
        if (Input.GetKey(KeyCode.LeftShift) && !clickedSlot.IsEmpty() && !GameObject.Find("HoverIcon"))
        {
            from = clickedSlot;
            createSplitter(clickedSlot);
            return;
        }

        //If from is null and slot has items in it, set from equal to slicked slot
        if (hoverIcon == null)
        {
            if (!clickedSlot.IsEmpty())
            {
                from = clickedSlot;

                createHoverIcon(from);
                while (!from.IsEmpty())
                {
                    GameObject.Find("HoverIcon").GetComponent<Slot>().Items.Push(from.RemoveItem());
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
                Stack<GameItem> tmpHover = hoverIcon.GetComponent<Slot>().Items;
                createHoverIcon(to);
                hoverIcon.GetComponent<Slot>().Items = to.SetItems(tmpHover);
                if (hoverIcon.GetComponent<Slot>().Items.Count == 0)
                {
                    DestroyImmediate(GameObject.Find("HoverIcon"));
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
                to = null;
                from = null;
                Destroy(GameObject.Find("HoverIcon"));
            }
        }
    }

    //Creates hover icon on mouse
    public void createHoverIcon(Slot slot)
    {
        Destroy(GameObject.Find("HoverIcon"));
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

    //create stack splitter under slot
    private void createSplitter(Slot clickedSlot)
    {
        stackSplitter = (GameObject)Instantiate(splitterPrefab);
        stackSplitter.name = "StackSplitter";
        stackSplitter.transform.SetParent(GameObject.Find("Canvas").transform, true);
        stackSplitter.GetComponent<StackSplitter>().slot = clickedSlot.gameObject;

        Vector2 position;
        Vector3 slotPos = new Vector3(clickedSlot.transform.position.x + clickedSlot.GetComponent<RectTransform>().sizeDelta.x / 2, clickedSlot.transform.position.y - clickedSlot.GetComponent<RectTransform>().sizeDelta.y * 2);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.GetComponentInParent<Canvas>().transform as RectTransform, slotPos, this.GetComponentInParent<Canvas>().worldCamera, out position);
        stackSplitter.transform.position = this.GetComponentInParent<Canvas>().transform.TransformPoint(position);

        RectTransform splitRect = stackSplitter.GetComponent<RectTransform>();
        splitRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100 * this.GetComponentInParent<Canvas>().scaleFactor);
        splitRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50 * this.GetComponentInParent<Canvas>().scaleFactor);
    }

    //close inventory
    public void close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        if (hoverIcon != null)
        {
            if (from.itemsInStack() == hoverIcon.GetComponent<Slot>().itemsInStack() && !from.isFull())
            {
                from.SetItems(hoverIcon.GetComponent<Slot>().Items);
            }
            else
            {
                while (!hoverIcon.GetComponent<Slot>().IsEmpty())
                {
                    AddItem(hoverIcon.GetComponent<Slot>().Items.Pop());
                }
                emptySlots++;
            }
            Destroy(GameObject.Find("HoverIcon"));
        }
    }

    //open inventory
    private void open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
