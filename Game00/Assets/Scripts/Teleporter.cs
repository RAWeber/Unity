using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour {

    public int teleportScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject.Find("InteractText").GetComponent<Text>().text = "Press [F] to warp";
        }
    }

    //Interact with other objects
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneManager.LoadScene(teleportScene);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject.Find("InteractText").GetComponent<Text>().text = string.Empty;
        }
    }
}
