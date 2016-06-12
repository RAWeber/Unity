using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour {

    public int sceneToLoad;

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height - 80, 150, 30), "Current Scene: " +(Application.loadedLevel+1));
        if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height - 50, 150, 40), "Load Scene " + (sceneToLoad + 1)))
            SceneManager.LoadScene(sceneToLoad);
    }
}
