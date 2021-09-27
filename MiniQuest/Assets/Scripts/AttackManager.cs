using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [SerializeField]
    private PlayerControls playerControls;

    [SerializeField]
    private GameObject arrow;

    private GameObject[] projectileBank = new GameObject[10];

    // Start is called before the first frame update
    void Start()
    {
        if (arrow == null)
        {
            Debug.LogError("no prefab in arrow");
        }

        for(int i = 0; i < projectileBank.Length; i++)//creates the object pool for the projectiles
        {
            projectileBank[i] = Instantiate(arrow);
            projectileBank[i].transform.position = Vector2.zero;
            projectileBank[i].SetActive(false);
        }


    }

    // Update is called once per frame
    void Update()
    {
        setupProjectile();
        destroyProjectile();

    }

    void setupProjectile()//checks if there is a free projectile in the object pool for the player to use
    {
        if (playerControls.getReleaseProjectile())
        {
            GameObject temp = playerControls.checkRotation();//temp gameobject to store which projectile position and rotation the projectile will spawn out of

            playerControls.setReleaseProjectile(false);

            for (int i = 0; i < projectileBank.Length; i++)
            {
                if (!projectileBank[i].activeInHierarchy)//checks if the projectile is not yet active, meaning available
                {
                    projectileBank[i].SetActive(true);
                    projectileBank[i].GetComponent<Projectile>().setDirectionVector(playerControls.GetPrevMovementVector());//sends playerControls previous non zero movement vector to the projectile
                    projectileBank[i].transform.position = temp.transform.position;//sets position of the projectile
                    projectileBank[i].transform.rotation = temp.transform.rotation;//sets rotation of the projectile
                    break;
                }
            }
        }
    }
    void destroyProjectile()//this checks to see if the life span of the projectile has ended and will put it back in the projectile bank
    {
        
        for (int i = 0; i < projectileBank.Length; i++)
        {
            if (projectileBank[i].activeInHierarchy && projectileBank[i].GetComponent<Projectile>().getIsDead())//checks if the projectile is active and is dead
            {
                projectileBank[i].GetComponent<Projectile>().resetIsDead();//resets the isDead bool
                projectileBank[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                projectileBank[i].transform.position = Vector2.zero;
                projectileBank[i].SetActive(false);
            }
        }
    }
}
