using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God : MonoBehaviour
{
    public static God instance;

    void Awake()
    {
        if(instance != null)
        {
            instance = this;
        }
    }
}
