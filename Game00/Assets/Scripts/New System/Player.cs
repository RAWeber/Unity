using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public int speed;   //Movement speed
    //public Inventory inventory; //Player inventory
    private Inventory inventory;

    // Use this for initialization
    void Start()
    {
        inventory = GameObject.Find("InventoryWindow").GetComponent<Inventory>();
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

    //Interact with other objects
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            inventory.AddItem(other.GetComponent<GameItem>().Item);
        }else if( other.tag == "Enemy")
        {
            other.GetComponent<BaseEnemy>().damaged(10);
        }
    }
}
