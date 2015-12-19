using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerWindow : MonoBehaviour {

    Player player;

	// Use this for initialization
	void Start () {

        player = FindObjectOfType<Player>();

        //Vector2 position;
        //GameObject equipWindow = GameObject.Find("EquipmentWindow");
        //Vector3 slotPos = new Vector3(equipWindow.transform.position.x);
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(this.GetComponentInParent<Canvas>().transform as RectTransform, slotPos, this.GetComponentInParent<Canvas>().worldCamera, out position);
        //this.transform.position = this.GetComponentInParent<Canvas>().transform.TransformPoint(position);
    }
	
	// Update is called once per frame
	void Update () {
        SetText();
    }

    private void SetText()
    {
        Text itemInfo = this.transform.GetChild(0).GetComponent<Text>();
        itemInfo.text = "<size=24>Player Stats</size><size=20>"+player.Stats.StatList()+"</size>";
    }
}
