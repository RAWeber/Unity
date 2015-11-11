using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

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
                if (x % 2 == 0 && y!=1)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "ComboSlot";
                    newSlot.transform.SetParent(this.transform);
                    slotRect.localPosition = new Vector3(slotPadding * (x + 1) + slotSize * x, -slotPadding * (y + 1) - slotSize * y - combinationTitle);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);
                    allSlots.Add(newSlot);
                }
                if(x % 2 == 1 && y == 1)
                {
                    GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                    RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                    newSlot.name = "ResultSlot";
                    newSlot.transform.SetParent(this.transform);
                    slotRect.localPosition = new Vector3(slotPadding * (x + 1) + slotSize * x, -slotPadding * (y + 1) - slotSize * y - combinationTitle);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                    slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);
                    allSlots.Add(newSlot);
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
                comboRelics.Add(slot.GetComponent<Slot>().GetComponent<BaseItem>().ItemName);
            }
        }
    }

    public void mergeRelics()
    {

    }
}
