using UnityEngine;
using System.Collections.Generic;

public class EquipmentWindow : SlotWindow {

    void Awake()
    {
        if (GameControl.equipment == null)
        {
            DontDestroyOnLoad(gameObject);
            GameControl.equipment = this;
        }
        else if (GameControl.equipment != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        CreateWindow();
    }

    //Create inventory window with aeftl slots
    protected override void CreateWindow()
    {
        emptySlots = slotTotal = 10;
        windowName = "Equipment";

        int columns = 2;
        int width = columns * slotSize + columns * slotPadding + slotPadding*25;
        int height = (slotTotal / columns) * slotSize + (slotTotal / columns) * slotPadding + slotPadding + titleSize;

        RectTransform window = this.gameObject.GetComponent<RectTransform>();
        window.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        window.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

        List<string> names = new List<string>() { "HELM", "SHOULDERS", "CHEST", "GLOVES", "LEGS", "BOOTS", "BACK", "ACCESSORY", "MAINHAND", "OFFHAND"};

        for (int y = 0; y < slotTotal / columns; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                newSlot.name = names[x+y*2];
                newSlot.transform.SetParent(this.transform);
                slotRect.localPosition = new Vector3(slotPadding * (x*25 + 1) + slotSize * x, -slotPadding * (y + 1) - slotSize * y - titleSize);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);

                AllSlots.Add(newSlot.GetComponent<Slot>());
            }
        }
    }
}
