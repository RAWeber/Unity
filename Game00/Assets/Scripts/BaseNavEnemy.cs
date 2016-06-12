using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class BaseNavEnemy : BaseEntity
{
    public float speed;
    public float sight;
    public string enemyName;
    private bool dead;

    public int armor;
    public int attackPower;
    public int attackSpeed;
    public int attackRange;

    private float idleTimer;
    private int randTimer;

    protected Vector3 origin;
    private Vector3 destination;

    private Transform player;
    public CharacterController controller;

    public Animator anim;
    public NavMeshAgent agent;

    private GameObject healthPanel;
    private float healthPanelOffset;
    private Slider healthSlider;
    public GameObject healthPrefab;
    public Renderer selfRenderer;

    private BossNavEnemy boss;

    public void SetBoss(BossNavEnemy boss)
    {
        this.boss = boss;
    }
    // Use this for initialization
    protected void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;

        healthPanel = Instantiate(healthPrefab);
        healthPanel.SetActive(false);
        healthPanel.transform.SetParent(FindObjectOfType<Canvas>().transform, false);

        healthPanel.GetComponentInChildren<Text>().text = enemyName;

        healthSlider = healthPanel.GetComponentInChildren<Slider>();
        healthPanelOffset = selfRenderer.bounds.size.y - 0.5f;

        origin = this.transform.position;

        idleTimer = 0;
        randTimer = UnityEngine.Random.Range(10, 20);
        //destination = origin;
        agent = gameObject.AddComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;
        agent.speed = speed;
        agent.acceleration = 1000;
        agent.angularSpeed = 1000;
    }

    protected override void setStats()
    {
        stats = new PlayerStatCollection(100, 100, armor, attackPower, attackSpeed, attackRange);
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!dead)
        {
            if (inSight())
            {
                idleTimer = 0;
                if (anim.GetBool("wander"))
                {
                    anim.SetBool("wander", false);
                    agent.ResetPath();
                }
                if (!inRange())
                {
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("run"))
                    {
                        agent.speed = speed;
                        chase();
                    }
                }
                else
                {
                    lookAt();
                }
            }
            else
            {
                if (!anim.GetBool("wander"))
                {
                    agent.ResetPath();
                    idleTimer += Time.deltaTime;
                    if(idleTimer > randTimer)
                    {
                        NavMeshHit hit;
                        SphericalCoordinates.SphericalToCartesian(UnityEngine.Random.Range(10, 15), UnityEngine.Random.Range(0, 360), 0, out destination);
                        destination += origin;
                        //Debug.Log("Origin: " + origin);
                        //Debug.Log("Current: " + transform.position);
                        //Debug.Log(" Destination: " + destination);
                        if(NavMesh.SamplePosition(destination, out hit, 5, NavMesh.AllAreas))
                        {
                            agent.speed = speed / 2;
                            //Debug.Log("Hit: " + hit.position);
                            agent.SetDestination(hit.position);
                            anim.SetBool("wander", true);
                            idleTimer = 0;
                            randTimer = UnityEngine.Random.Range(10, 20);
                        }
                    }
                }
                else
                {
                    if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance * 2)
                        //&& (!agent.hasPath || agent.velocity.sqrMagnitude <= 0.5f)
                    {
                       anim.SetBool("wander", false);
                    }
                }

                //if (Math.Abs((transform.position - destination).magnitude) > 1)
                //    wander(destination);
                //else
                //{
                //    anim.SetBool("wander", false);
                //    idleTimer += Time.deltaTime;

                //    if (idleTimer > randTimer)
                //    {
                //        SphericalCoordinates.SphericalToCartesian(UnityEngine.Random.Range(5, 10), UnityEngine.Random.Range(0, 360), 0, out destination);
                //        destination += origin;

                //        idleTimer = 0;
                //        randTimer = UnityEngine.Random.Range(10, 20);
                //    }
                //}
            }

            if (selfRenderer.isVisible && Camera.main.WorldToViewportPoint(transform.position).z > 0)
            {
                Vector3 worldPos = new Vector3(transform.position.x, transform.position.y + healthPanelOffset, transform.position.z);
                Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
                healthPanel.transform.position = screenPos;
                float distance = (worldPos - Camera.main.transform.position).magnitude;

                if (distance < 20)
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

        if (stats.GetStat<VitalStat>(StatType.HEALTH).CurrentValue <= 0 && !dead)
        {
            stats.GetStat<VitalStat>(StatType.HEALTH).CurrentValue = 0;
            anim.SetTrigger("isDead");
            dead = true;
        }
        else if (!dead)
        {
            if (UnityEngine.Random.Range(1, 100) > 75)
                anim.SetTrigger("hit");
        }
    }

    public bool inRange()
    {
        bool inRange = Vector3.Distance(transform.position, player.position) <= stats.GetStat(StatType.ATTACKRANGE).TotalStatValue;
        //anim.SetBool("inRange", Vector3.Distance(transform.position, player.position) <= stats.GetStat(StatType.ATTACKRANGE).TotalStatValue);
        anim.SetBool("inRange", inRange);
        if (inRange)
            agent.ResetPath();
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
        controller.Move(transform.forward * speed / 2 * Time.deltaTime);
    }

    public void chase()
    {
        agent.SetDestination(player.position);
    }

    public void attack()
    {
        if (inRange())
            player.GetComponent<Player>().getHit(stats.GetStat(StatType.ATTACKPOWER).TotalStatValue);
    }

    public void Die()
    {
        GetComponent<DropManager>().CreateDrop(this.transform.position + new Vector3(0, 2, 0));
        if (boss != null)
            boss.MinionDie(this.gameObject);
        Destroy(this.gameObject);
    }
}
