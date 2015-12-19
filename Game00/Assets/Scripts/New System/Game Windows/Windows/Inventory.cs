using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class Inventory : SlotWindow
{

    // Use this for initialization
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
                newSlot.transform.SetParent(this.transform);
                slotRect.localPosition = new Vector3(slotPadding * (x + 1) + slotSize * x, -slotPadding * (y + 1) - slotSize * y - titleSize);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);

                allSlots.Add(newSlot);
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
        else
        {
            Debug.Log("Adding null item to inventory");
        }

    }

    //Add item into empty inventory slot
    private void PlaceEmpty(BaseItem item)
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
        else
        {
            Debug.Log("Inventory full");
        }
    }

    //Add item into stack in inventory
    private void PlaceStack(BaseItem item)
    {
        foreach (GameObject slot in allSlots)
        {
            Slot tmp = slot.GetComponent<Slot>();
            if (!tmp.IsEmpty())
            {
                if (tmp.itemsInStack().Type == item.Type && !tmp.isFull())
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
}
