using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private bool heartUsed = false;//lets game know the heart has been used by the player
    private char heartSpawnArea;//area where the heart is spawned in

    public bool GetHeartUsed()
    {
        return heartUsed;
    }
    public void SetHeartUsed(bool set)
    {
        heartUsed = set;
    }

    public char GetHeartSpawnArea()
    {
        return heartSpawnArea;
    }
    public void SetHeartSpawnArea(char set)
    {
        heartSpawnArea = set;
    }
}
