using UnityEngine;
using System.Collections;

public class GameItem : MonoBehaviour {

    public string itemName;
    private BaseItem item;

    public BaseItem Item
    {
        get
        {
            return item;
        }

        set
        {
            item = value;
        }
    }

    // Use this for initialization
    void Start () {
        item = ItemDatabase.itemDatabase.Find(x => x.ItemName == itemName);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
