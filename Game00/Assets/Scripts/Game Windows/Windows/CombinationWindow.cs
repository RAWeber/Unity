using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CombinationWindow : SlotWindow {

    private List<Slot> comboSlots;
    private bool created;
    private BaseItem result;

    public HashSet<string> comboRelics = new HashSet<string>();
    private Slot resultSlot;

    public bool Created
    {
        get { return created; }
        set { created = value; }
    }

    public List<Slot> ComboSlots
    {
        get { return comboSlots; }
        set { comboSlots = value; }
    }

    void Awake()
    {
        if (GameControl.comboWindow == null)
        {
            DontDestroyOnLoad(gameObject);
            GameControl.comboWindow = this;
        }
        else if (GameControl.comboWindow != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        Created = false;
        CreateWindow();
	}

    protected override void CreateWindow()
    {
        comboSlots = new List<Slot>();
        emptySlots = slotTotal = 4;
        windowName = "Combination";

        int width = 3 * slotSize + 3 * slotPadding + slotPadding;
        int height = 3 * slotSize + 3 * slotPadding + slotPadding + titleSize + 50;

        RectTransform window = this.gameObject.GetComponent<RectTransform>();
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
                    slotRect.localPosition = new Vector3(slotPadding * (x + 1) + slotSize * x, -slotPadding * (y + 1) - slotSize * y - titleSize);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);
                    comboSlots.Add(newSlot.GetComponent<Slot>());
                    AllSlots.Add(newSlot.GetComponent<Slot>());

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
                    slotRect.localPosition = new Vector3(slotPadding * (x + 1) + slotSize * x, -slotPadding * (y + 1) - slotSize * y - titleSize);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);
                    comboSlots.Add(newSlot.GetComponent<Slot>());
                    AllSlots.Add(newSlot.GetComponent<Slot>());

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
                    slotRect.localPosition = new Vector3(slotPadding * (x + 1) + slotSize * x, -slotPadding * (y + 1) - slotSize * y - titleSize);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);
                    AllSlots.Add(newSlot.GetComponent<Slot>());
                    resultSlot = newSlot.GetComponent<Slot>();
                }
                else if(x !=2 && y == 2)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "ComboSlot";
                    newSlot.transform.SetParent(this.transform);
                    slotRect.localPosition = new Vector3(slotPadding * (x + 1) + slotSize * x + slotSize/2 + slotPadding/2, -slotPadding * (y + 1) - slotSize * y - titleSize);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);
                    comboSlots.Add(newSlot.GetComponent<Slot>());
                    AllSlots.Add(newSlot.GetComponent<Slot>());

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
        resultSlot.ClearSlot();
        foreach (Slot slot in comboSlots)
        {
            if (!slot.IsEmpty())
            {
                if (!comboRelics.Add(slot.SlotItems().ItemName))
                {
                    Debug.Log("Duplicate Relic");
                }
            }
        }
        try
        {
            if (!Created)
                {
                switch (ItemDatabase.componentDatabase[comboRelics].Type)
                {
                    case BaseItem.ItemType.WEAPON:
                        result = new BaseWeapon((BaseWeapon)ItemDatabase.componentDatabase[comboRelics]);
                        break;
                    case BaseItem.ItemType.EQUIPMENT:
                        result = new BaseEquipment(ItemDatabase.componentDatabase[comboRelics]);
                        break;
                    case BaseItem.ItemType.RELIC:
                        result = new BaseRelic(ItemDatabase.componentDatabase[comboRelics]);
                        break;
                    case BaseItem.ItemType.CONSUMABLE:
                        result = new BaseConsumable(ItemDatabase.componentDatabase[comboRelics]);
                        break;
                }
                foreach (Slot slot in comboSlots)
                {
                    if (!slot.IsEmpty())
                    {
                        result.Stats.TransferStats(slot.SlotItems().Stats);
                    }
                }
                resultSlot.AddItem(result);
            }
        }
        catch (KeyNotFoundException)
        {
            resultSlot.ClearSlot();
        }
    }

    public void mergeRelics()
    {
        try
        {
            if (!resultSlot.IsEmpty())
            {
                foreach (Slot slot in comboSlots)
                {
                    slot.ClearSlot();
                }
                created = true;
            }
        }catch(KeyNotFoundException)
        {
            Debug.Log("Relics do not form item");
        }
    }
}
