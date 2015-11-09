using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class SelectedItem : MonoBehaviour, IDragHandler, IPointerDownHandler ,IPointerUpHandler{

    private Text selectedText;
    private List<BaseItemOld> playerInventory;
    private InventoryWindow inventoryWindow;

	// Use this for initialization
	void Start () {
        selectedText = GameObject.Find("SelectedItemText").GetComponent<Text>();
        BasePlayer player = GameObject.FindGameObjectWithTag("Player").GetComponent<BasePlayer>();
        playerInventory = player.GetInventory();
        inventoryWindow = GameObject.Find("InventoryWindow").GetComponent<InventoryWindow>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowSelectedItemText()
    {
        if (this.gameObject.GetComponent<Toggle>().isOn)
        {
            if (this.gameObject.name == "Empty")
            {
                selectedText.text = "This slot is empty";
            }
            else
            {
                selectedText.text = playerInventory[System.Int32.Parse(this.gameObject.name)].GetName();
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!inventoryWindow.beingDragged && this.name!="Empty")
        {
            inventoryWindow.ShowDraggedIcon(this.transform.name);
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.name = "Empty";
        }
        //if (eventData.dragging == false)
        //{
        //    this.transform.name = inventoryWindow.AddItemToSlot(this.gameObject);
        //    this.transform.GetChild(0).gameObject.SetActive(true);
        //}
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (inventoryWindow.beingDragged)
        {
            //this.transform.name = inventoryWindow.AddItemToSlot(this.gameObject);
            //this.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (inventoryWindow.beingDragged)
        {
            //this.transform.name = inventoryWindow.AddItemToSlot(this.gameObject);
            //this.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
