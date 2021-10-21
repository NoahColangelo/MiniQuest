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

    //these hold the runtime animation controllers for the enemy types
    [SerializeField]
    private RuntimeAnimatorController MoleController;
    [SerializeField]
    private RuntimeAnimatorController TreantController;

    //timer variables for spawning in enemies
    private float spawnTimer = 5.0f;
    private float timer = 0.0f;

    void Start()
    {
        transitionManager = FindObjectOfType<TransitionManager>();

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
        if (timer >= spawnTimer)//spawn timer
        {
            spawnEnemy();
            timer = 0;
        }
        else
            timer += Time.deltaTime;

        despawnEnemy();
    }

    void spawnEnemy()
    {
        for(int i = 0; i < enemyPool.Length; i++)
        {
            if (!enemyPool[i].gameObject.activeInHierarchy)//checks if an enemy is not yet active, meaning available
            {
                enemyPool[i].gameObject.SetActive(true);
                enemyPool[i].setEnemyType(enemyType(enemyPool[i]));//sets the enemy type
                enemyPool[i].gameObject.transform.position = checkSpawnArea(enemyPool[i]);//selects the spawn position
                break;
            }
        }
    }

    void despawnEnemy()
    {
        for (int i = 0; i < enemyPool.Length; i++)
        {
            if (enemyPool[i].gameObject.activeInHierarchy && enemyPool[i].getIsDead() ||//checks if the enemy is active and is dead
                enemyPool[i].getEnemySpawnArea() != transitionManager.getPlayerCurrentArea())//checks if the player has left the area, if yes then the enemies will despawn
            {
                enemyPool[i].gameObject.transform.position = Vector2.zero;
                enemyPool[i].resetHealth();//resets the health and isDead bool on the enemy
                enemyPool[i].gameObject.SetActive(false);//deactivates enemy if they are dead
            }
        }
    }

    private char enemyType(EnemyAI enemy)
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

    private Vector2 checkSpawnArea(EnemyAI enemy)
    {
        int rand;
        switch (transitionManager.getPlayerCurrentArea())//switch that checksthe current area of the player so enemies will spawn accordingly
        {
            case 'L':
                rand = (int)Random.Range(0.0f, enemyLeftSpawns.Length);
                enemy.setEnemySpawnArea('L');
                return enemyLeftSpawns[rand].position;

            case 'R':
                rand = (int)Random.Range(0.0f, enemyRightSpawns.Length);
                enemy.setEnemySpawnArea('R');
                return enemyRightSpawns[rand].position;

            case 'U':
                rand = (int)Random.Range(0.0f, enemyTopSpawns.Length);
                enemy.setEnemySpawnArea('T');
                return enemyTopSpawns[rand].position;

            case 'B':
                rand = (int)Random.Range(0.0f, enemyBottomSpawns.Length);
                enemy.setEnemySpawnArea('B');
                return enemyBottomSpawns[rand].position;

            default://will instantly kill the enemies that spawn when player is in the middle area
                enemy.setIsDead(true);
                return Vector2.zero;
        }


        //if(transitionManager.getPlayerCurrentArea() == 'L')
        //{
        //    int rand = (int)Random.Range(0.0f, enemyLeftSpawns.Length);
        //    enemy.setEnemySpawnArea('L');
        //    return enemyLeftSpawns[rand].position;
        //}
        //else if(transitionManager.getPlayerCurrentArea() == 'R')
        //{
        //    int rand = (int)Random.Range(0.0f, enemyRightSpawns.Length);
        //    enemy.setEnemySpawnArea('R');
        //    return enemyRightSpawns[rand].position;
        //}
        //else if (transitionManager.getPlayerCurrentArea() == 'U')
        //{
        //    int rand = (int)Random.Range(0.0f, enemyTopSpawns.Length);
        //    enemy.setEnemySpawnArea('T');
        //    return enemyTopSpawns[rand].position;
        //}
        //else if(transitionManager.getPlayerCurrentArea() == 'B')
        //{
        //    int rand = (int)Random.Range(0.0f, enemyBottomSpawns.Length);
        //    enemy.setEnemySpawnArea('B');
        //    return enemyBottomSpawns[rand].position;
        //}
        //else
        //{
        //    enemy.setIsDead(true);
        //    return Vector2.zero;
        //}
    }
}
