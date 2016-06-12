using UnityEngine;

public class Inventory : SlotWindow
{
    // Use this for initialization
    void Awake()
    {
        if (GameControl.inventory == null)
        {
            GameControl.inventory = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (GameControl.inventory != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CreateWindow();
    }

    //Create inventory window with aeftl slots
    protected override void CreateWindow()
    {
        emptySlots = slotTotal = 25;

        int columns = 5;
        int width = columns * slotSize + columns * slotPadding + slotPadding;
        int height = (slotTotal / columns) * slotSize + (slotTotal / columns) * slotPadding + slotPadding + titleSize;


        RectTransform window = this.gameObject.GetComponent<RectTransform>();
        window.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        window.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        for (int y = 0; y < slotTotal / columns; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                newSlot.name = "Slot";
                newSlot.transform.SetParent(this.transform, true);
                slotRect.localPosition = new Vector3(slotPadding * (x + 1) + slotSize * x, -slotPadding * (y + 1) - slotSize * y - titleSize);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);

                AllSlots.Add(newSlot.GetComponent<Slot>());
            }
        }
    }

    //Add item to inventory
    public void AddItemToInventory(BaseItem item)
    {
        if (item != null)
        {
            if (item.MaxSize == 1)
            {
                PlaceEmpty(item);
            }
            else
            {
                PlaceStack(item);
            }
        }
    }

    //Add item into empty inventory slot
    private void PlaceEmpty(BaseItem item)
    {
        if (emptySlots > 0)
        {
            foreach (Slot slot in AllSlots)
            {
                if (slot.IsEmpty())
                {
                    slot.AddItem(item);
                    emptySlots--;
                    return;
                }
            }
        }
        else
        {
            Debug.Log("Inventory full");
        }
    }

    //Add item into stack in inventory
    private void PlaceStack(BaseItem item)
    {
        foreach (Slot slot in AllSlots)
        {
            if (!slot.IsEmpty())
            {
                if (slot.SlotItems().Equals(item) && !slot.isFull())
                {
                    slot.AddItem(item);
                    return;
                }
            }
        }
        if (emptySlots > 0)
        {
            PlaceEmpty(item);
        }
    }
}