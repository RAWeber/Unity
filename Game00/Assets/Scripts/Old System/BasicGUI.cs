using UnityEngine;
using System.Collections;

public class BasicGUI : MonoBehaviour {

    BaseWarrior warrior = new BaseWarrior();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI(){
        GUILayout.Label(warrior.Name);
        GUILayout.Label(warrior.Description);
        GUILayout.Label("Strength: "+warrior.Strength.ToString());
        GUILayout.Label("Intelligence: " + warrior.Intelligence.ToString());
        GUILayout.Label("Health: " + warrior.Health.ToString());
        GUILayout.Label("Level: " + warrior.Level.ToString());
    }
}
