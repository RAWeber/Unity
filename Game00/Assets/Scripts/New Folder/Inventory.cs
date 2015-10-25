using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour {

    public int slotTotal;
    public int columns;
    public int slotSize;
    public int slotPadding;
    public GameObject slotPrefab;
    public GameObject iconPrefab;
    public Canvas canvas;
    public EventSystem eventSystem;

    private List<GameObject> allSlots;
    private static GameObject hoverIcon;
    private static Slot from, to;
    private static int emptySlots;

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
    void Start () {
        CreateWindow();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            if(!eventSystem.IsPointerOverGameObject(-1) && from != null)
            {
                from.GetComponent<Image>().color = Color.white;
                from.ClearSlot();
                Destroy(GameObject.Find("HoverIcon"));
                from = null;
            }
        }
        if (hoverIcon != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            hoverIcon.transform.position = canvas.transform.TransformPoint(position);
        }
    }

    private void CreateWindow()
    {
        allSlots = new List<GameObject>();
        emptySlots = slotTotal;

        RectTransform window = this.gameObject.GetComponent<RectTransform>();
        int width = columns * slotSize + columns * slotPadding + slotPadding;
        int height = (slotTotal / columns) * slotSize + (slotTotal / columns) * slotPadding + slotPadding;
        window.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        window.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

        for(int y = 0; y<slotTotal/ columns; y++)
        {
            for(int x = 0; x < columns; x++)
            {
                GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                newSlot.name = "Slot";
                newSlot.transform.SetParent(this.transform.parent);
                slotRect.localPosition = window.localPosition + new Vector3(slotPadding * (x + 1) + slotSize * x, -slotPadding * (y + 1) - slotSize * y);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);
                allSlots.Add(newSlot);
            }
        }
    }

    public void AddItem(Item item)
    {
        if(item.maxSize == 1)
        {
            PlaceEmpty(item);
        }
        else
        {
            PlaceStack(item);
        }
    }

    private void PlaceEmpty(Item item)
    {
        if (emptySlots > 0)
        {
            foreach(GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();
                if(tmp.IsEmpty())
                {
                    tmp.AddItem(item);
                    emptySlots--;
                    return;
                }
            }
        }
    }

    private void PlaceStack(Item item)
    {
        foreach (GameObject slot in allSlots)
        {
            Slot tmp = slot.GetComponent<Slot>();
            if (!tmp.IsEmpty())
            {
                if (tmp.itemsInStack().type == item.type && !tmp.isFull())
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

    public void MoveItem(GameObject clicked)
    {
        if (from == null)
        {
            if (!clicked.GetComponent<Slot>().IsEmpty())
            {
                from = clicked.GetComponent<Slot>();
                from.GetComponent<Image>().color = Color.grey;

                hoverIcon = (GameObject)Instantiate(iconPrefab);
                hoverIcon.GetComponent<Image>().sprite = clicked.transform.GetChild(1).GetComponent<Image>().sprite;
                hoverIcon.name = "HoverIcon";

                RectTransform hoverTransform = hoverIcon.GetComponent<RectTransform>();
                RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);
                hoverIcon.transform.SetParent(GameObject.Find("Canvas").transform, true);
                hoverIcon.transform.localScale = from.gameObject.transform.localScale;
            }
        }else if(to == null)
        {
            to = clicked.GetComponent<Slot>();
            Destroy(GameObject.Find("HoverIcon"));
        }
        if(from!=null && to!=null)
        {
            Stack<Item> tmpTo = new Stack<Item>(to.Items);
            to.AddItems(from.Items);
            if (tmpTo.Count == 0)
            {
                from.ClearSlot();
            }
            else
            {
                from.AddItems(tmpTo);
            }
            from.GetComponent<Image>().color = Color.white;
            to = null;
            from = null;
            hoverIcon = null;
        }
    }
}
