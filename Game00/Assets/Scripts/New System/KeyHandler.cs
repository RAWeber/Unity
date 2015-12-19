using UnityEngine;
using System.Collections;

public class KeyHandler : MonoBehaviour {

    private GameObject comboWindow;
    private GameObject equipWindow;
    private GameObject inventoryWindow;

    private bool initialClose = true;

    // Use this for initialization
    void Start () {
        inventoryWindow = GameObject.Find("InventoryWindow");
        comboWindow = GameObject.Find("CombinationWindow");
        equipWindow = GameObject.Find("EquipmentWindow");
    }
	
	// Update is called once per frame
	void Update () {
        if (initialClose)
        {
            CloseAll();
            initialClose = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAll();
        }

        //Open & close inventory with 'I'
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryWindow.activeSelf)
            {
                InventorySetActive(false);
            }
            else
            {
                InventorySetActive(true);
            }
        }

        //Open & close combo window
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (comboWindow.activeSelf)
            {
                ComboSetActive(false);
            }
            else
            {
                ComboSetActive(true);
            }
        }

        //Open & close equipment window
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (equipWindow.activeSelf)
            {
                EquipSetActive(false);
            }
            else
            {
                EquipSetActive(true);
            }
        }
    }

    //set state of inventory window
    public void InventorySetActive(bool state)
    {
        inventoryWindow.SetActive(state);
    }

    //set state of combo window
    public void ComboSetActive(bool state)
    {
        comboWindow.SetActive(state);
    }

    //set state of equipment window
    public void EquipSetActive(bool state)
    {
        equipWindow.SetActive(state);
    }

    public void CloseAll()
    {
        InventorySetActive(false);
        ComboSetActive(false);
        EquipSetActive(false);
    }
}
