using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class CombinationWindow : MonoBehaviour {

    public int slotSize;    //Size of slot side
    public int slotPadding; //Space between slots
    public int combinationTitle;  //Title bar
    public GameObject slotPrefab;   //Slot prefab
    public Canvas canvas;   //Canvas
    //public EventSystem eventSystem; //Event System

    //private CanvasGroup canvasGroup;    //Canvas group to open/close
    private List<GameObject> allSlots;  //List of slots
    private int emptySlots; //Total number of empty slots

    public HashSet<string> comboRelics = new HashSet<string>();

    // Use this for initialization
    void Start () {

        CreateWindow();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (this.isActiveAndEnabled)
            {
                open();
            }
            else
            {
                close();
            }
        }
    }

    private void CreateWindow()
    {
        allSlots = new List<GameObject>();
        emptySlots = 4;

        RectTransform window = this.gameObject.GetComponent<RectTransform>();
        int width = 3 * slotSize + 3 * slotPadding + slotPadding;
        int height = 3 * slotSize + 3 * slotPadding + slotPadding + combinationTitle+50;
        window.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        window.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                if (x ==1 && y==0)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "ComboSlot";
                    newSlot.transform.SetParent(this.transform);
                    slotRect.localPosition = new Vector3(slotPadding * (x + 1) + slotSize * x, -slotPadding * (y + 1) - slotSize * y - combinationTitle);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);
                    allSlots.Add(newSlot);

                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerClick;
                    entry.callback.AddListener((eventData) => { updateSet(); });
                    newSlot.AddComponent<EventTrigger>().triggers.Add(entry);
                }
                else if (x % 2 == 0 && y==1)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "ComboSlot";
                    newSlot.transform.SetParent(this.transform);
                    slotRect.localPosition = new Vector3(slotPadding * (x + 1) + slotSize * x, -slotPadding * (y + 1) - slotSize * y - combinationTitle);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);
                    allSlots.Add(newSlot);
                    //newSlot.GetComponent<Button>().onClick.AddListener(() => { updateSet();});

                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerClick;
                    entry.callback.AddListener((eventData) => { updateSet(); });
                    newSlot.AddComponent<EventTrigger>().triggers.Add(entry);
                }
                else if(x % 2 == 1 && y == 1)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "ResultSlot";
                    newSlot.transform.SetParent(this.transform);
                    slotRect.localPosition = new Vector3(slotPadding * (x + 1) + slotSize * x, -slotPadding * (y + 1) - slotSize * y - combinationTitle);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);
                    allSlots.Add(newSlot);
                }else if(x !=2 && y == 2)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "ComboSlot";
                    newSlot.transform.SetParent(this.transform);
                    slotRect.localPosition = new Vector3(slotPadding * (x + 1) + slotSize * x + slotSize/2 + slotPadding/2, -slotPadding * (y + 1) - slotSize * y - combinationTitle);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);
                    allSlots.Add(newSlot);

                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerClick;
                    entry.callback.AddListener((eventData) => { updateSet(); });
                    newSlot.AddComponent<EventTrigger>().triggers.Add(entry);
                }
            }
        }
    }

    public void updateSet()
    {
        comboRelics.Clear();
        foreach(GameObject slot in allSlots)
        {
            if (!slot.GetComponent<Slot>().IsEmpty())
            {
                comboRelics.Add(slot.GetComponent<Slot>().itemsInStack().ItemName);
            }
        }
        try
        {
            GameObject.Find("ResultSlot").transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ItemDatabase.componentDatabase[comboRelics].ReturnItemIcon();
        }
        catch (KeyNotFoundException)
        {
            GameObject.Find("ResultSlot").GetComponent<Slot>().ClearSlot();
            Debug.Log(comboRelics.ToString());
        }
    }

    public void close()
    {
        this.gameObject.SetActive(false);
        //if (hoverIcon != null)
        //{
        //    if (from.itemsInStack() == hoverIcon.GetComponent<Slot>().itemsInStack() && !from.isFull())
        //    {
        //        from.SetItems(hoverIcon.GetComponent<Slot>().Items);
        //    }
        //    else
        //    {
        //        while (!hoverIcon.GetComponent<Slot>().IsEmpty())
        //        {
        //            AddItem(hoverIcon.GetComponent<Slot>().Items.Pop());
        //        }
        //        emptySlots++;
        //    }
        //    Destroy(GameObject.Find("HoverIcon"));
        //}
    }

    //open inventory
    private void open()
    {
        this.gameObject.SetActive(false);
    }

    public void mergeRelics()
    {
        try
        {
            BaseItem result = ItemDatabase.componentDatabase[comboRelics];
            foreach(GameObject s in allSlots)
            {
                s.GetComponent<Slot>().ClearSlot();
            }
            //GameObject.Find("ResultSlot").GetComponent<Slot>().ClearSlot();
            GameObject.Find("ResultSlot").GetComponent<Slot>().AddItem(result);
        }catch(KeyNotFoundException)
        {
            Debug.Log(comboRelics.ToString());
        }
    }
}
