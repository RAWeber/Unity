using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryWindow : MonoBehaviour {

    public int startPosX;
    public int startPosY;
    public int slotsPerPage;
    public int slotsPerRow;
    public GameObject itemSlotPrefab;
    public ToggleGroup itemSlotToggleGroup;

    public GameObject draggedIcon;
    public BaseItemOld itemBeingDragged;
    public bool beingDragged = false;

    private const int mousePosOffest = 30;
    private string slotName;

    private int xPos;
    private int yPos;
    private int slotCount;
    private GameObject itemSlot;
    private List<GameObject> inventorySlots;
    private List<BaseItemOld> playerInventory;

	// Use this for initialization
	void Start () {
        CreateInventorySlots();
        AddInventoryItems();
	}
	
	// Update is called once per frame
	void Update () {
        if (beingDragged)
        {
            Vector3 mousePosition = (Input.mousePosition - GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>().localPosition);
            draggedIcon.GetComponent<RectTransform>().localPosition = new Vector3(mousePosition.x + mousePosOffest, mousePosition.y - mousePosOffest, mousePosition.z);
        }
	}

    public void ShowDraggedIcon(string name)
    {
        slotName = name;
        beingDragged = true;
        draggedIcon.SetActive(true);
        itemBeingDragged = playerInventory[int.Parse(name)];
        draggedIcon.GetComponent<Image>().sprite = ReturnItemIcon(itemBeingDragged);
    }

    public string AddItemToSlot(GameObject slot)
    {
        slot.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ReturnItemIcon(playerInventory[int.Parse(slotName)]);
        draggedIcon.SetActive(false);
        itemBeingDragged = null;
        beingDragged = false;
        return slotName;
    }

    private void CreateInventorySlots()
    {
        inventorySlots = new List<GameObject>();
        xPos = startPosX;
        yPos = startPosY;
        for(int i = 0; i < slotsPerPage; i++)
        {
            itemSlot = (GameObject)Instantiate(itemSlotPrefab);
            itemSlot.name = "Empty";
            itemSlot.GetComponent<Toggle>().group = itemSlotToggleGroup;
            itemSlot.transform.SetParent(this.gameObject.transform);
            inventorySlots.Add(itemSlot);
            itemSlot.GetComponent<RectTransform>().localPosition = new Vector3(xPos, yPos, 0);
            slotCount++;
            xPos += (int)itemSlot.GetComponent<RectTransform>().rect.width+5;
            if (slotCount % slotsPerRow==0)
            {
                slotCount = 0;
                yPos -= (int)itemSlot.GetComponent<RectTransform>().rect.height+5;
                xPos = startPosX;
            }
        }
    }

    private void AddInventoryItems()
    {
        BasePlayer player = GameObject.FindGameObjectWithTag("Player").GetComponent<BasePlayer>();
        playerInventory = player.GetInventory();
        for(int i = 0; i < playerInventory.Count; i++)
        {
            if (inventorySlots[i].name == "Empty")
            {
                inventorySlots[i].name = i.ToString();
                inventorySlots[i].transform.GetChild(0).gameObject.SetActive(true);
                inventorySlots[i].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ReturnItemIcon(playerInventory[i]);
            }
        }
    }

    private Sprite ReturnItemIcon(BaseItemOld item)
    {
        Sprite icon = new Sprite();
        if (item.GetItemType() == BaseItemOld.ItemTypes.EQUIPMENT)
        {
            icon = Resources.Load<Sprite>("ItemIcons/armor");
        }
        else
        {
            icon = Resources.Load<Sprite>("ItemIcons/axe");
        }
            return icon;
    }
}
