using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverIcon : MonoBehaviour {

    private Canvas canvas;
    private EventSystem eventSystem; //Event System

    // Use this for initialization
    void Start () {
        canvas = GameObject.FindObjectOfType<Canvas>();
        eventSystem = GameObject.FindObjectOfType<EventSystem>();
	}
	
	// Update is called once per frame
	void Update () {

        //Move hover icon with mouse
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
        this.transform.position = canvas.transform.TransformPoint(position);

        //Delete inventory items by clicking off inventory
        if (Input.GetMouseButton(0))
        {
            if (!eventSystem.IsPointerOverGameObject(-1))
            {
                GameObject.FindObjectOfType<Inventory>().EmptySlots++;
                Destroy(this.gameObject);
            }
        }
    }

}
