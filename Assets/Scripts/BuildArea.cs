using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildArea : MonoBehaviour
{
    public GameObject cannonToSpawn;

    // Use this for initialization
    void Start()
    {
        Instantiate(cannonToSpawn, transform);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
