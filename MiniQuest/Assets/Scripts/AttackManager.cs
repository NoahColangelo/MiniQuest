using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [SerializeField]
    private PlayerControls playerControls;

    [SerializeField]
    private GameObject shootUp;//0
    [SerializeField]
    private GameObject shootLeft;//90
    [SerializeField]
    private GameObject shootDown;//180
    [SerializeField]
    private GameObject shootRight;//270

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

        if(playerControls.getReleaseProjectile())
        {
            GameObject temp = checkRotation();//temp gameobject to store which projectile position and rotation the projectile will spawn out of

            playerControls.setReleaseProjectile(false);

            for(int i = 0; i < projectileBank.Length; i++)
            {
                if(!projectileBank[i].activeInHierarchy)//checks if the projectile is not yet active, meaning available
                {
                    projectileBank[i].SetActive(true);
                    projectileBank[i].GetComponent<Projectile>().setDirectionVector(playerControls.GetPrevMovementVector());//sends playerControls previous non zero movement vector to the projectile
                    projectileBank[i].transform.position = temp.transform.position;//sets position of the projectile
                    projectileBank[i].transform.rotation = temp.transform.rotation;//sets rotation of the projectile
                    break;
                }
            }
        }

        //this checks to see if the life span of the projectile has ended and will put it back in the projectile bank
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

    GameObject checkRotation()//checks which projectile position and rotation the projectile will spawn out of
    {
        switch (playerControls.getRotation())
        {
            case 0:
                return shootUp;
            case 90:
                return shootLeft;
            case 180:
                return shootDown;
            case 270:
                return shootRight;
        }
        return null;
    }

}