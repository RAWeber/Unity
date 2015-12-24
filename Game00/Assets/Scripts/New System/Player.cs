using UnityEngine;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour
{
    private string playerName;
    private int experience = 0;
    private int level = 1;
    private PlayerStatCollection stats = new PlayerStatCollection();

    public PlayerStatCollection Stats
    {
        get { return stats; }
        //set { stats = value; }
    }

    public string PlayerName
    {
        get { return playerName; }
        set { playerName = value; }
    }

    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    // Use this for initialization
    void Awake()
    {
        //Set player inventory
        if (GameControl.player== null)
        {
            DontDestroyOnLoad(gameObject);
            GameControl.player = this;
        }
        else if (GameControl.player != this)
        {
            GameControl.player.transform.position = this.transform.position;
            Destroy(gameObject);
        }
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
                GameControl.inventory.AddItemToInventory(other.GetComponent<GameItem>().Item);
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

    public PlayerData SaveInfo()
    {
        PlayerData data = new PlayerData();
        data.playerName = playerName;
        data.level = level;
        data.experience = experience;
        data.stats = stats;
        return data;
    }

    public void LoadInfo(PlayerData data)
    {
        playerName = data.playerName;
        level = data.level;
        experience = data.experience;
        stats = data.stats;
    }
}

[Serializable]
public class PlayerData
{
    public string playerName;
    public int level;
    public int experience;
    public PlayerStatCollection stats;
}
