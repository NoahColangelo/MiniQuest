using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private bool heartUsed = false;//lets game know the heart has been used by the player

    public bool GetHeartUsed()
    {
        return heartUsed;
    }
    public void SetHeartUsed(bool set)
    {
        heartUsed = set;
    }


}
