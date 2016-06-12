using UnityEngine;
using System.Collections;

public abstract class BaseEntity : MonoBehaviour {

    protected PlayerStatCollection stats;
    protected int damageTimer = 0;

    public PlayerStatCollection Stats
    {
        get { return stats; }
        //set { stats = value; }
    }

    protected void Awake()
    {
        setStats();
        BaseInvoke();
    }

    protected virtual void setStats()
    {
        stats = new PlayerStatCollection();
    }

    protected void BaseInvoke()
    {
        InvokeRepeating("Regenerate", 0, 1);
    }

    private void Regenerate()
    {
        if(stats.GetStat<VitalStat>(StatType.HEALTH).GetPercentage() < 1)
        {
            if (damageTimer >= 10)
                stats.GetStat<VitalStat>(StatType.HEALTH).CurrentValue += 10;
            else
                damageTimer++;
        }
    }
}
