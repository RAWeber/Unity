using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossEnemy : BaseEnemy {

    private List<GameObject> minions = new List<GameObject>();
    private int minionCount = 0;
    private float timer = 0;
    int tempTime;

    public int maxMinions;
    public float spawnTimer;
    public GameObject minionPrefab;

	// Use this for initialization
	new protected void Start () {
        tempTime = UnityEngine.Random.Range(-15, 15);
        base.Start();
    }

    protected override void setStats()
    {
        stats = new PlayerStatCollection(150, 100);
    }

    // Update is called once per frame
    new void Update () {
        base.Update();
        timer += Time.deltaTime;

        if (!inRange() && timer > spawnTimer + tempTime)
        {
            if(minionCount < maxMinions)
            {
                GameObject minion = Instantiate(minionPrefab);
                //Vector3 spawnPoint = new Vector3(transform.position.x + UnityEngine.Random.Range(-10, 10), transform.position.y, transform.position.z + UnityEngine.Random.Range(-10, 10));
                //Vector3 spawnPoint = UnityEngine.Random.insideUnitCircle;
                //spawnPoint = spawnPoint.normalized * UnityEngine.Random.Range(10, 25);
                //spawnPoint = new Vector3(spawnPoint.x, 0, spawnPoint.y) + this.transform.position;
                Vector3 spawnPoint;
                SphericalCoordinates.SphericalToCartesian(UnityEngine.Random.Range(15, 30), minionCount * 360 / maxMinions, 0, out spawnPoint);
                spawnPoint += origin;
                minion.GetComponent<Transform>().position = spawnPoint;
                minion.name = enemyName + "Minion";

                //minion.transform.SetParent(this.transform);
                minion.GetComponent<BaseEnemy>().SetBoss(this);

                minionCount++;
                minions.Add(minion);

                tempTime = UnityEngine.Random.Range(-15, 15);
                Debug.Log(spawnPoint);
            }
            timer = 0;
        }
    }

    public void MinionDie(GameObject minion)
    {
        minions.Remove(minion);
        minionCount--;
    }

    new public void Die()
    {
        GetComponent<DropManager>().CreateDrop(this.transform.position + new Vector3(0, 2, 0));
        //while(minions.Count != 0)
        //    minions[0].GetComponent<BaseEnemy>().Die();
        Destroy(this.gameObject);
    }
}
