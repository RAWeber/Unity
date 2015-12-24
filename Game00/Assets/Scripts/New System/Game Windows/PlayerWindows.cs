using UnityEngine;
using System.Collections;

public class PlayerWindows : MonoBehaviour {

    void Awake()
    {
        if (GameControl.canvas == null)
        {
            DontDestroyOnLoad(gameObject);
            GameControl.canvas = this.GetComponent<Canvas>();
        }
        else if (GameControl.canvas != this.GetComponent<Canvas>())
        {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        if (GameControl.currentPlayer.loadCharacter)
        {
            GameControl.control.Load();
        }
        else
        {
            GameControl.player.PlayerName = GameControl.currentPlayer.playerName;
            GameControl.control.Save();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
