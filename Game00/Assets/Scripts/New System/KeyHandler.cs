using UnityEngine;
using System.Collections;

public class KeyHandler : MonoBehaviour {

    private GameObject comboWindow;
    private GameObject equipWindow;
    private GameObject inventoryWindow;

    private bool initialClose;

    // Use this for initialization
    void Start () {
        initialClose = true;
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
            if (GameControl.inventory.gameObject.activeSelf)
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
            if (GameControl.comboWindow.gameObject.activeSelf)
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
            if (GameControl.equipment.gameObject.activeSelf)
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
        GameControl.inventory.gameObject.SetActive(state);
    }

    //set state of combo window
    public void ComboSetActive(bool state)
    {
        GameControl.comboWindow.gameObject.SetActive(state);
    }

    //set state of equipment window
    public void EquipSetActive(bool state)
    {
        GameControl.equipment.gameObject.SetActive(state);
    }

    public void CloseAll()
    {
        InventorySetActive(false);
        ComboSetActive(false);
        EquipSetActive(false);
    }
}
