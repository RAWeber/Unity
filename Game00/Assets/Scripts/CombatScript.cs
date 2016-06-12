using UnityEngine;
using System.Collections;

public class CombatScript : MonoBehaviour {

    private Player player;
    public GameObject target;
    public Animator anim;

    private PlayerStatCollection stat;
    //public int damage;
    //public float range;
    //private bool impacted;

    // Use this for initialization
    void Start () {
        player = GetComponentInParent<Player>();
        stat = player.Stats;
	}
	
	// Update is called once per frame
	void Update () {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        anim.SetFloat("NormalizedTime", state.normalizedTime);
        if (Input.GetMouseButtonDown(0))
        {
            if (state.IsName("Idle"))
                anim.SetTrigger("Swing1");
            else if (state.IsName("Swing1"))
                anim.SetTrigger("Swing2");
            else if (state.IsName("Swing2"))
                anim.SetTrigger("Swing3");
            //attack.Play("Sword_Attack");
        }

        //if (impacted && !attack.IsPlaying("Sword_Attack"))
        //{
        //    impacted = false;
        //}
    }

    public void impact(float modifier)
    {
        //if(!impacted)
        //{
        //Debug.Log("Impacting");
        //if (attack["Sword_Attack"].time > 0)
        //{
        if (target != null && inRange())
        {
            if(target.GetComponent<BaseEnemy>() != null)
            {
                target.GetComponent<BaseEnemy>().getHit((int)(stat.GetStat(StatType.ATTACKPOWER).TotalStatValue * modifier));
                Debug.Log(target.GetComponent<BaseEnemy>().Stats.GetStat<VitalStat>(StatType.HEALTH).GetPercentage());
            }
            else
            {
                target.GetComponent<BaseNavEnemy>().getHit((int)(stat.GetStat(StatType.ATTACKPOWER).TotalStatValue * modifier));
                Debug.Log(target.GetComponent<BaseNavEnemy>().Stats.GetStat<VitalStat>(StatType.HEALTH).GetPercentage());
            }
        }
                //impacted = true;
            //}
        //}
    }

    public bool inRange()
    {
        return Vector3.Distance(transform.position, target.transform.position) <= stat.GetStat(StatType.ATTACKRANGE).TotalStatValue;
    }
}
