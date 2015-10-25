using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Slot : MonoBehaviour {

    private Stack<Item> items;
    public Text stackText;

	// Use this for initialization
	void Start () {

        items = new Stack<Item>();
        RectTransform slotRect = GetComponent<RectTransform>();
        RectTransform txtRect = stackText.GetComponent<RectTransform>();
        int txtScaleFactor = (int)(slotRect.sizeDelta.x * 0.60);
        stackText.resizeTextMaxSize = txtScaleFactor;
        stackText.resizeTextMinSize = txtScaleFactor;
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);

        //GameObject icon = this.transform.GetChild(1).gameObject;
        //icon.GetComponent<Image>().sprite = null;
        //icon.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
        //icon.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool IsEmpty()
    {
        return items.Count == 0;
    }

    public void AddItem(Item item)
    {
        items.Push(item);
        GameObject icon = this.transform.GetChild(1).gameObject;
        icon.GetComponent<Image>().sprite = ReturnItemIcon(item);
        if (items.Count>1)
        {
            stackText.text = items.Count.ToString();
        }
    }

    private Sprite ReturnItemIcon(Item item)
    {
        Sprite icon = new Sprite();
        if (item.type == Item.ItemType.HEALTH)
        {
            icon = Resources.Load<Sprite>("ItemIcons/potionRed");
        }
        else
        {
            icon = Resources.Load<Sprite>("ItemIcons/potionBlue");
        }
        return icon;
    }
}
