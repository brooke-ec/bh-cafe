using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SpillSpawnerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform[] SpillSpawns;
    [SerializeField] private GameObject SpillPrefab;
    [SerializeField] private int amountOfSpills;

    void Start()
    {
        
        SpillSpawns = transform.GetComponentsInChildren<Transform>();
        for( int i =0; i<amountOfSpills; i++)
        {
            Transform spill = Util.PickRandom(SpillSpawns);
            Instantiate(SpillPrefab, spill.transform.position,spill.transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
