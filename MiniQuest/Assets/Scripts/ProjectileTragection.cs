using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTragection : MonoBehaviour
{
    [SerializeField]//holds a vector2 (filled in the game inspector) that is unique to each of the 4 game objects for proper direction of the projectile
    private Vector2 direction;

    public Vector2 getDirection()
    {
        return direction;
    }
}
