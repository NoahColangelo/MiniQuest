using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EnemyAI enemy;//holds the enemy prefab

    private EnemyAI[] enemyPool = new EnemyAI[10];//this array acts as an object pool for the enemies
    
    //these arrays hold the enemy spawn points for each area of the map
    [SerializeField]
    private Transform[] enemyLeftSpawns = new Transform[8];
    [SerializeField]
    private Transform[] enemyRightSpawns = new Transform[8];
    [SerializeField]
    private Transform[] enemyTopSpawns = new Transform[8];
    [SerializeField]
    private Transform[] enemyBottomSpawns = new Transform[8];

    private TransitionManager transitionManager;
    private HeartManager heartManager;

    private Transform enemyTarget;//enemy target is the players transform

    //these hold the runtime animation controllers for the enemy types
    [SerializeField]
    private RuntimeAnimatorController MoleController;
    [SerializeField]
    private RuntimeAnimatorController TreantController;

    //timer variables for spawning in enemies
    private const float spawnTimer = 6.0f;
    private float timer = 0.0f;

    private float currentEnemiesNum = 0;

    void Start()
    {
        transitionManager = FindObjectOfType<TransitionManager>();
        heartManager = FindObjectOfType<HeartManager>();

        enemyTarget = FindObjectOfType<PlayerControls>().transform;

        if (enemy == null)
        {
            Debug.LogError("no prefab in enemy");
        }

        for (int i = 0; i < enemyPool.Length; i++)//adds the enemies to the object pool
        {
            enemyPool[i] = Instantiate(enemy);
            enemyPool[i].transform.position = Vector2.zero;
            enemyPool[i].gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (timer >= spawnTimer + currentEnemiesNum)//spawn timer
        {
            SpawnEnemy();
            timer = 0;
        }
        else
            timer += Time.deltaTime;

        DespawnEnemy();
    }

    void SpawnEnemy()
    {
        for(int i = 0; i < enemyPool.Length; i++)
        {
            if (!enemyPool[i].gameObject.activeInHierarchy)//checks if an enemy is not yet active, meaning available
            {
                enemyPool[i].gameObject.SetActive(true);
                enemyPool[i].SetEnemyType(EnemyType(enemyPool[i]));//sets the enemy type
                enemyPool[i].gameObject.transform.position = CheckSpawnArea(enemyPool[i]);//selects the spawn position

                currentEnemiesNum++;
                break;
            }
        }
    }

    void DespawnEnemy()
    {
        for (int i = 0; i < enemyPool.Length; i++)
        {
            if (enemyPool[i].gameObject.activeInHierarchy && enemyPool[i].GetIsDead() && enemyPool[i].GetEnemySpawnArea() == transitionManager.GetPlayerCurrentArea())//checks if the enemy is active and is dead
            {
                heartManager.DropHeart(enemyPool[i].gameObject.transform.position, enemyPool[i].GetEnemySpawnArea());// roll to see if a heart will drop where the enemy has died

                enemyPool[i].gameObject.transform.position = Vector2.zero;
                enemyPool[i].ResetHealth();//resets the health and isDead bool on the enemy
                enemyPool[i].gameObject.SetActive(false);//deactivates enemy if they are dead

                currentEnemiesNum--;
            }
            else if(enemyPool[i].gameObject.activeInHierarchy &&
                enemyPool[i].GetEnemySpawnArea() != transitionManager.GetPlayerCurrentArea())//checks if the player has left the area, if yes then the enemies will force despawn
            {
                enemyPool[i].gameObject.transform.position = Vector2.zero;
                enemyPool[i].ResetHealth();//resets the health and isDead bool on the enemy
                enemyPool[i].gameObject.SetActive(false);//deactivates enemy if they are dead

                currentEnemiesNum--;
            }
        }
    }

    private char EnemyType(EnemyAI enemy)
    {
        float rand = Random.Range(0.0f, 10.0f);//random number between 0 and 10

        if (rand < 5.0f)//if lower then 5 then itll be a mole
        {
            enemy.GetComponent<Animator>().runtimeAnimatorController = MoleController;//changes the animator controller
            return 'M';
        }
        else//if higher itll be a treant
        {
            enemy.GetComponent<Animator>().runtimeAnimatorController = TreantController;
            return 'T';
        }
    }

    private Vector2 CheckSpawnArea(EnemyAI enemy)
    {
        int rand;
        switch (transitionManager.GetPlayerCurrentArea())//switch that checks the current area of the player so enemies will spawn accordingly
        {
            case 'L':
                enemy.SetEnemySpawnArea('L');
                while (true)
                {
                    rand = (int)Random.Range(0.0f, enemyLeftSpawns.Length);

                    if(Vector2.Distance(enemyLeftSpawns[rand].position, enemyTarget.position) > 5.0f)
                    {
                        break;
                    }
                }
                return enemyLeftSpawns[rand].position;

            case 'R':
                enemy.SetEnemySpawnArea('R');
                while (true)
                {
                    rand = (int)Random.Range(0.0f, enemyRightSpawns.Length);

                    if (Vector2.Distance(enemyRightSpawns[rand].position, enemyTarget.position) > 5.0f)
                    {
                        break;
                    }
                }
                return enemyRightSpawns[rand].position;

            case 'U':
                enemy.SetEnemySpawnArea('U');
                while (true)
                {
                    rand = (int)Random.Range(0.0f, enemyTopSpawns.Length);

                    if (Vector2.Distance(enemyTopSpawns[rand].position, enemyTarget.position) > 5.0f)
                    {
                        break;
                    }
                }
                return enemyTopSpawns[rand].position;

            case 'B':
                enemy.SetEnemySpawnArea('B');
                while (true)
                {
                    rand = (int)Random.Range(0.0f, enemyBottomSpawns.Length);

                    if (Vector2.Distance(enemyBottomSpawns[rand].position, enemyTarget.position) > 5.0f)
                    {
                        break;
                    }
                }
                return enemyBottomSpawns[rand].position;

            default://will instantly kill the enemies that spawn when player is in the middle area
                enemy.SetIsDead(true);
                return Vector2.zero;
        }
    }
}
