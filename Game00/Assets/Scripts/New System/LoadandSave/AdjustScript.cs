using UnityEngine;
using System.Collections;

public class AdjustScript : MonoBehaviour {

	void OnGUI()
    {
        if(GUI.Button(new Rect(10,80,100,30), "Health up"))
        {
            GameControl.control.health += 10;
        }
        if (GUI.Button(new Rect(10, 120, 100, 30), "Health down"))
        {
            GameControl.control.health -= 10;
        }
        if (GUI.Button(new Rect(10, 160, 100, 30), "Exp up"))
        {
            GameControl.control.experience += 10;
        }
        if (GUI.Button(new Rect(10, 200, 100, 30), "Exp down"))
        {
            GameControl.control.experience -= 10;
        }
        if (GUI.Button(new Rect(10, 240, 100, 30), "Save"))
        {
            GameControl.control.Save();
        }
        if (GUI.Button(new Rect(10, 280, 100, 30), "Load"))
        {
            GameControl.control.Load();
        }
    }
}
