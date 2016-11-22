using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Player : BaseEntity
{
    private string playerName;
    private int experience = 0;
    private int level = 1;

    public Slider slider;
    private CombatScript combat;


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

    protected override void setStats()
    {
        stats = new PlayerStatCollection(250, 100, 10, 20, 10, 2);
    }

    // Use this for initialization
    void Awake()
    {
        //Set player inventory
        if (GameControl.player== null)
        {
            DontDestroyOnLoad(gameObject);
            GameControl.player = this;
            combat = GetComponentInChildren<CombatScript>();
            //Cursor.visible = false;
            base.Awake();
        }
        else if (GameControl.player != this)
        {
            GameControl.player.transform.position = this.transform.position;
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if(slider != null)
        {
            slider.value = stats.GetStat<VitalStat>(StatType.HEALTH).GetPercentage();
        }

        //Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider.tag == "Enemy")
        {
            combat.target = hit.collider.gameObject;
        }
        else
        {
            combat.target = null;
        }
        //Debug.DrawRay(ray.origin, ray.direction * 1000, new Color(1f, 0.922f, 0.016f, 1f));
        //Debug.DrawLine(ray.origin, ray.direction * 1000, new Color(1f, 0.922f, 0.016f, 1f));
    }

    public void getHit(int damage)
    {
        damageTimer = 0;
        stats.GetStat<VitalStat>(StatType.HEALTH).CurrentValue -= damage;
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
