using UnityEngine;
using System.Collections;

public class SaveLoad : MonoBehaviour {

	void OnGUI()
    {
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
