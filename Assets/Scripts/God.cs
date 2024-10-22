using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God : MonoBehaviour
{
    public static God instance;

    public LevelUIManager levelUIManager;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
}
