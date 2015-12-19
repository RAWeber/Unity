using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int speed;   //Movement speed
    private Inventory inventory; //Player inventory
    private PlayerStatCollection stats = new PlayerStatCollection();

    public PlayerStatCollection Stats
    {
        get { return stats; }
        //set { stats = value; }
    }

    // Use this for initialization
    void Start()
    {
        //Set player inventory
        inventory = GameObject.FindObjectOfType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    //Control player movement
    private void HandleMovement()
    {
        float translation = speed * Time.deltaTime;
        transform.Translate(new Vector3(Input.GetAxis("Horizontal") * translation, Input.GetAxis("Jump") * translation, Input.GetAxis("Vertical") * translation));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            GameObject.Find("InteractText").GetComponent<Text>().text = "Press [F] to loot";
        }
    }

    //Interact with other objects
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Item")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                inventory.AddItemToInventory(other.GetComponent<GameItem>().Item);
                Destroy(other.gameObject);
                GameObject.Find("InteractText").GetComponent<Text>().text = string.Empty;
            }
        }else if( other.tag == "Enemy")
        {
            other.GetComponent<BaseEnemy>().damaged(10);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Item")
        {
            GameObject.Find("InteractText").GetComponent<Text>().text = string.Empty;
        }
    }
}
