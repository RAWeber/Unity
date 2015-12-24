using UnityEngine;
using UnityEngine.UI;

public class PlayerWindow : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        SetText();
    }

    private void SetText()
    {
        Text itemInfo = this.transform.GetChild(0).GetComponent<Text>();
        itemInfo.text = "<size=24>"+GameControl.player.PlayerName+" Stats</size><size=20>"+GameControl.player.Stats.StatList()+"</size>";
    }
}