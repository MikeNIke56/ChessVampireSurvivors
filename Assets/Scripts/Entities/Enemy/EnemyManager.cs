using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

/**
* manager class for all enemies spawned in
*/
public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Spawn Variables")]
    public List<GameObject> enemyGameObjectList;
    [SerializeField] protected int maxNumOfEnemies;
    public float spawnDelay;
    public float minSpawnDistance;
    public float maxSpawnDistance;

    //list of enemy class objects
    public List<EnemyBaseClass> enemyList;
    public List<EnemyBaseClass> pendingRemoveEnemyList;

    private PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        StartCoroutine(SpawnInEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAllEnemiesBehavior();
    }

    private void FixedUpdate()
    {
        UpdateAllEnemiesMovement();
    }

    /**
     * updates enemy behavior logic (state changes, triggers, etc.)- to be ran in Update
     */
    private void UpdateAllEnemiesBehavior()
    {
        foreach (EnemyBaseClass enemy in enemyList)
        {
            if (enemy && enemy.GetHealth() > 0)
                enemy.RunBehavior();
            else
                pendingRemoveEnemyList.Add(enemy);
        }

        foreach (EnemyBaseClass enemy in pendingRemoveEnemyList)
        {
            enemyList.Remove(enemy);
            enemy.Die();
        }

        pendingRemoveEnemyList.Clear();
    }

    /**
     * updates enemy movement, because physics-based- to be ran in FixedUpdate
     */
    private void UpdateAllEnemiesMovement()
    {
        foreach (EnemyBaseClass enemy in enemyList)
        {
            if (enemy && enemy.GetHealth() > 0)
                enemy.HandleMovement();
        }
    }

    /**
     * spawns in appropriate enemies from list with delay between each spawn
     */
    protected IEnumerator SpawnInEnemies()
    {
        while (enemyList.Count < maxNumOfEnemies)
        {
            GameObject enemyGameObjectCopy = Instantiate<GameObject>(enemyGameObjectList[0],
            FindRandomSpawnPoint(), Quaternion.identity);

            EnemyBaseClass enemy = enemyGameObjectCopy.GetComponent<EnemyBaseClass>();
            enemy.Setup(player);
            enemyList.Add(enemy);
            yield return new WaitForSecondsRealtime(spawnDelay);
        }
    }

    /**
     * finds random point around character to spawn the enemy
     */
    private Vector3 FindRandomSpawnPoint()
    {
        int[] infrontOrBehind = new int[2];
        infrontOrBehind[0] = 1;
        infrontOrBehind[1] = -1;

        //grab a random point within range
        //then run a 50/50 for whether to make it negative
        Vector3 randSpawnPoint = new Vector3(
            Random.Range(minSpawnDistance, maxSpawnDistance) *
            infrontOrBehind[Random.Range(0, infrontOrBehind.Length)],

            Random.Range(minSpawnDistance, maxSpawnDistance) *
            infrontOrBehind[Random.Range(0, infrontOrBehind.Length)], 

            0);

        return randSpawnPoint;
    }
}

