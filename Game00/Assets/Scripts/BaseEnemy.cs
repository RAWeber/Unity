using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class BaseEnemy : BaseEntity {

    public float speed;
    public float sight;
    public string enemyName;
    private bool dead;
    //private bool waited;
    private float idleTimer;
    private int randTimer;

    protected Vector3 origin;
    private Vector3 destination;

    private Transform player;
    public CharacterController controller;

    public Animator anim;

    private GameObject healthPanel;
    private float healthPanelOffset;
    private Slider healthSlider;
    public GameObject healthPrefab;
    public Renderer selfRenderer;

    private BossEnemy boss;

    public void SetBoss(BossEnemy boss)
    {
        this.boss = boss;
    }
	// Use this for initialization
	protected void Start () {

        //waited = false;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        healthPanel = Instantiate(healthPrefab);
        healthPanel.SetActive(false);
        healthPanel.transform.SetParent(FindObjectOfType<Canvas>().transform, false);

        healthPanel.GetComponentInChildren<Text>().text = enemyName;

        healthSlider = healthPanel.GetComponentInChildren<Slider>();
        healthPanelOffset = selfRenderer.bounds.size.y - 0.5f;

        origin = this.transform.position;
        Debug.Log(origin);

        idleTimer = 0;
        randTimer = UnityEngine.Random.Range(10, 20);
        destination = origin;
    }

    protected override void setStats()
    {
        stats = new PlayerStatCollection(100, 100);
    }

    // Update is called once per frame
    protected void Update () {
        if (!dead)
        {
            if (inSight())
            {
                lookAt();
                if (!inRange())
                {
                    //if (waited && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
                    if(anim.GetCurrentAnimatorStateInfo(0).IsName("run"))
                    {
                        chase();
                    }
                }
            }
            else
            {
                //waited = false;

                if(Math.Abs((transform.position - destination).magnitude) > 1)
                    wander(destination);
                else
                {
                    anim.SetBool("wander", false);
                    idleTimer += Time.deltaTime;

                    if (idleTimer > randTimer)
                    {
                        //destination = UnityEngine.Random.insideUnitCircle;
                        //destination = destination.normalized * UnityEngine.Random.Range(5, 10);
                        //destination = new Vector3(destination.x, transform.position.y, destination.y);

                        SphericalCoordinates.SphericalToCartesian(UnityEngine.Random.Range(5, 10), UnityEngine.Random.Range(0, 360), 0, out destination);
                        destination += origin;

                        idleTimer = 0;
                        randTimer = UnityEngine.Random.Range(10, 20);
                    }
                }
            }

            if (selfRenderer.isVisible && Camera.main.WorldToViewportPoint(transform.position).z > 0)
            {
                Vector3 worldPos = new Vector3(transform.position.x, transform.position.y + healthPanelOffset, transform.position.z);
                Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
                healthPanel.transform.position = screenPos;
                float distance = (worldPos - Camera.main.transform.position).magnitude;

                if(distance < 20)
                {
                    healthPanel.SetActive(true);
                    healthSlider.value = stats.GetStat<VitalStat>(StatType.HEALTH).GetPercentage();
                    float scale = 3f / (distance);
                    healthPanel.transform.localScale = new Vector3(scale, scale, scale);
                }
                else
                {
                    healthPanel.SetActive(false);
                }
            }
            else
            {
                healthPanel.SetActive(false);
            }
        }
        else
        {
            Destroy(healthPanel);
        }
	}

    public void getHit(int damage)
    {
        stats.GetStat<VitalStat>(StatType.HEALTH).CurrentValue -= damage;
        damageTimer = 0;
        //Debug.Log(health);

        if (stats.GetStat<VitalStat>(StatType.HEALTH).CurrentValue <= 0 && !dead)
        {
            //GameObject.Find("DropManager").GetComponent<DropManager>().CreateDrop(this.transform.position);
            stats.GetStat<VitalStat>(StatType.HEALTH).CurrentValue = 0;
            anim.SetTrigger("isDead");
            dead = true;
        }
        else if(!dead)
        {
            if(UnityEngine.Random.Range(1, 100) > 75)
                anim.SetTrigger("hit");
        }
    }

    public bool inRange()
    {
        anim.SetBool("inRange", Vector3.Distance(transform.position, player.position) <= stats.GetStat(StatType.ATTACKRANGE).TotalStatValue);
        return anim.GetBool("inRange");
    }

    public void lookAt()
    {
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    public bool inSight()
    {
        anim.SetBool("inSight", Vector3.Distance(transform.position, player.position) <= sight);
        return anim.GetBool("inSight");
    }

    public void wander(Vector3 destination)
    {
        anim.SetBool("wander", true);
        transform.LookAt(destination);
        controller.Move(transform.forward * speed/2 * Time.deltaTime);
    }

    public void chase()
    {
        //transform.LookAt(player.position);
        //Debug.Log(player.position.y + "and " + transform.position.y);
        //transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        //controller.SimpleMove(transform.forward * speed);
        controller.Move(transform.forward * speed * Time.deltaTime);
    }

    public void attack()
    {
        if(inRange())
            player.GetComponent<Player>().getHit(stats.GetStat(StatType.ATTACKPOWER).TotalStatValue);
    }

    //public void Waited()
    //{
    //    waited = true;
    //}

    public void Die()
    {
        GetComponent<DropManager>().CreateDrop(this.transform.position + new Vector3(0, 2, 0));
        if(boss != null)
            boss.MinionDie(this.gameObject);
        Destroy(this.gameObject);
    }

    /*public void OnMouseOver()
    {
        player.GetComponent<CombatScript>().target = this.gameObject;
    }

    public void OnMouseExit()
    {
        player.GetComponent<CombatScript>().target = null;
    }*/
}
