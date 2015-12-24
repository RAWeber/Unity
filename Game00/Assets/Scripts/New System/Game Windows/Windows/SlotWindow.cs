using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class SlotWindow : MonoBehaviour{
    protected int slotSize=40;    //Size of slot side
    protected int titleSize=30;  //Title bar
    protected int slotPadding = 5; //Space between slots
    private List<Slot> allSlots = new List<Slot>();  //List of slots in inventory
    protected int slotTotal;   //Total amount of slots in inventory
    protected int emptySlots; //Total number of empty slots
    protected string windowName;

    public GameObject slotPrefab;   //Slot prefab
    public Canvas canvas;

    protected abstract void CreateWindow();

    private Vector3 posInImage;

    //EmptySlots Getter/Setter
    public int EmptySlots
    {
        get { return emptySlots; }
        set { emptySlots = value; }
    }

    protected List<Slot> AllSlots
    {
        get { return allSlots; }
        set { allSlots = value; }
    }

    public void moveWindow()
    {
        this.transform.position = Input.mousePosition-posInImage;
    }

    public void setOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 windowPos = this.transform.position;
        posInImage = mousePos - windowPos;
    }

    public virtual SlotWindowData SaveInfo()
    {
        SlotWindowData data = new SlotWindowData();
        foreach(Slot slot in allSlots)
        {
            data.allSlots.Add(slot.SaveInfo());
        }
        data.emptySlots = emptySlots;
        return data;
    }

    public virtual void LoadInfo(SlotWindowData data)
    {
        for(int i = 0; i < allSlots.Count; i++)
        {
            allSlots[i].LoadInfo(data.allSlots[i]);
        }
        emptySlots = data.emptySlots;
    }
}

[Serializable]
public class SlotWindowData
{
    public List<SlotData> allSlots = new List<SlotData>();  //List of slots in inventory
    public int emptySlots; //Total number of empty slots
}