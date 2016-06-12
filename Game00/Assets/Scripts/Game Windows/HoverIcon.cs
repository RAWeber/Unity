using UnityEngine;
using UnityEngine.EventSystems;

public class HoverIcon : MonoBehaviour {

    public static HoverIcon hoverIcon;

    private EventSystem eventSystem; //Event System

    void Awake()
    {
        if (hoverIcon == null)
        {
            DontDestroyOnLoad(gameObject);
            hoverIcon = this;
        }
        else if (hoverIcon != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        eventSystem = GameObject.FindObjectOfType<EventSystem>();
	}
	
	// Update is called once per frame
	void Update () {

        //Move hover icon with mouse
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GameControl.canvas.transform as RectTransform, Input.mousePosition, GameControl.canvas.worldCamera, out position);
        this.transform.position = GameControl.canvas.transform.TransformPoint(position);

        //Delete inventory items by clicking off inventory
        if (Input.GetMouseButton(0))
        {
            if (!eventSystem.IsPointerOverGameObject(-1))
            {
                Debug.Log("Item Deleted");
                GameControl.inventory.EmptySlots++;
                Destroy(this.gameObject);
            }
        }
    }

}
