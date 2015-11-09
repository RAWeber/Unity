using UnityEngine;
using System.Collections;

public class BaseEnemy : MonoBehaviour {

    public int health;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (health == 0)
        {
            GameObject.Find("DropManager").GetComponent<DropManager>().CreateDrop(this.transform.position);
            Destroy(this.gameObject);
        }
	}

    public void damaged(int damage)
    {
        health -= damage;
        Debug.Log(health);
    }
}
