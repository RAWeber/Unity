using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    public int slotTotal;
    public int columns;
    public int slotSize;
    public int slotPadding;
    public GameObject slotPrefab;

    private List<GameObject> allSlots;
    private int emptySlots;

	// Use this for initialization
	void Start () {
        CreateWindow();
	}
	
	// Update is called once per frame
	void Update () {
	
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

    public bool AddItem(Item item)
    {
        if(item.maxSize == 1)
        {
            PlaceEmpty(item);
            return true;
        }
        return false;
    }

    private bool PlaceEmpty(Item item)
    {
        if (emptySlots > 0)
        {
            foreach(GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();
                if (tmp == null) {
                    Debug.Log("WHY");
                }
                else if(tmp.IsEmpty())
                {
                    tmp.AddItem(item);
                    emptySlots--;
                    return true;
                }
            }
        }
        return false;
    }
}
