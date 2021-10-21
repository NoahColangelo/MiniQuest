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
        SetupProjectile();
        DestroyProjectile();

    }

    void SetupProjectile()//checks if there is a free projectile in the object pool for the player to use
    {
        if (playerControls.GetReleaseProjectile())
        {
            GameObject temp = playerControls.CheckRotation();//temp gameobject to store which projectile position and rotation the projectile will spawn out of

            playerControls.SetReleaseProjectile(false);

            for (int i = 0; i < projectileBank.Length; i++)
            {
                if (!projectileBank[i].activeInHierarchy)//checks if the projectile is not yet active, meaning available
                {
                    projectileBank[i].SetActive(true);
                    projectileBank[i].GetComponent<Projectile>().SetDirectionVector(temp.GetComponent<ProjectileTragection>().GetDirection());//gets the direction vector from the gameobject it is shooting from
                    projectileBank[i].transform.position = temp.transform.position;//sets position of the projectile
                    projectileBank[i].transform.rotation = temp.transform.rotation;//sets rotation of the projectile
                    break;
                }
            }
        }
    }
    void DestroyProjectile()//this checks to see if the life span of the projectile has ended and will put it back in the projectile bank
    {
        
        for (int i = 0; i < projectileBank.Length; i++)
        {
            if (projectileBank[i].activeInHierarchy && projectileBank[i].GetComponent<Projectile>().GetIsDead())//checks if the projectile is active and is dead
            {
                projectileBank[i].GetComponent<Projectile>().ResetIsDead();//resets the isDead bool
                projectileBank[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                projectileBank[i].transform.position = Vector2.zero;
                projectileBank[i].SetActive(false);
            }
        }
    }
}
