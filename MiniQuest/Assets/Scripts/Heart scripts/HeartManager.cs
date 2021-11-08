using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartManager : MonoBehaviour
{

    [SerializeField]
    GameObject heart;

    private GameObject[] heartDrops = new GameObject[3];//object pool for hearts to drop from enemies when they die

    private TransitionManager transitionManager;

    void Start()
    {
        transitionManager = FindObjectOfType<TransitionManager>();

        for (int i = 0; i < heartDrops.Length; i++)//instantiates the the hearts
        {
            heartDrops[i] = Instantiate(heart);
            heartDrops[i].transform.position = Vector2.zero;
            heartDrops[i].SetActive(false);//deactivates so they are hidden from player
        }
    }

    private void Update()
    {
        CleanUpHearts();
    }

    public void DropHeart(Vector2 position, char spawnArea)//function calls when an enemy is killed
    {
        int rand = Mathf.RoundToInt( Random.Range(0, 100));// 33% to drop a heart on death

        if (rand >= 33)
        {
            for (int i = 0; i < heartDrops.Length; i++)
            {
                if (!heartDrops[i].activeInHierarchy)//if heart is not active, then it is able to spawn in
                {
                    heartDrops[i].SetActive(true);
                    heartDrops[i].transform.position = position;//sets position of the heart to the enemy
                    heartDrops[i].GetComponent<Heart>().SetHeartSpawnArea(spawnArea);

                    break;
                }
            }
        }
    }

    private void CleanUpHearts()//recycles hearts that are used
    {
        for (int i = 0; i < heartDrops.Length; i++)
        {
            if (heartDrops[i].activeInHierarchy && heartDrops[i].GetComponent<Heart>().GetHeartUsed())//if the heart has been used by the player, then recycle
            {
                heartDrops[i].SetActive(false);
                heartDrops[i].transform.position = Vector2.zero;
                heartDrops[i].GetComponent<Heart>().SetHeartUsed(false);
                heartDrops[i].GetComponent<Heart>().SetHeartSpawnArea('M');
            }
            else if (heartDrops[i].activeInHierarchy && heartDrops[i].GetComponent<Heart>().GetHeartSpawnArea() != transitionManager.GetPlayerCurrentArea())// else if the player has moved areas despawn the heart
            {
                heartDrops[i].SetActive(false);
                heartDrops[i].transform.position = Vector2.zero;
                heartDrops[i].GetComponent<Heart>().SetHeartSpawnArea('M');
            }
        }
    }
}
