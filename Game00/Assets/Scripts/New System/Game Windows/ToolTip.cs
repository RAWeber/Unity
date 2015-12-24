using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour {

    private Slot slot;

    public Slot Slot
    {
        get { return slot; }
        set { slot = value; }
    }

    // Use this for initialization
    void Start () {
        SetText();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void SetText()
    {
        Text itemInfo = this.transform.GetChild(0).GetComponent<Text>();
        itemInfo.text=slot.GetComponent<Slot>().SlotItems().getToolTip();
    }
}
