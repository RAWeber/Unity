using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public abstract class SlotWindow : MonoBehaviour{
    protected int slotSize=40;    //Size of slot side
    protected int titleSize=30;  //Title bar
    protected int slotPadding = 5; //Space between slots
    protected List<GameObject> allSlots = new List<GameObject>();  //List of slots in inventory
    protected int slotTotal;   //Total amount of slots in inventory
    protected int emptySlots; //Total number of empty slots

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
}
