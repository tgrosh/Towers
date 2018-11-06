using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseCannon : MonoBehaviour
{
    public GameObject currentTarget;
    public GameObject body;
    public float lookSpeed;
    public float fireRate; //seconds per shot
    public PlasmaPulse projectilePrefab;
    public Transform projectileSpawnPosition;

    List<GameObject> targets = new List<GameObject>();
    float fireTimer;
    float lookTime;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (targets.Count > 0)
        {
            lookTime += Time.deltaTime;            
            Vector3 direction = targets[0].transform.position - body.transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction);
            body.transform.rotation = Quaternion.Slerp(body.transform.rotation, toRotation, lookSpeed * lookTime);

            Fire();
        }
    }

    private void Fire()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer > fireRate)
        {
            PlasmaPulse projectile = Instantiate(projectilePrefab, projectileSpawnPosition.position, projectileSpawnPosition.rotation);
            projectile.Fire(targets[0].transform);
            fireTimer = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAgent"))
        {
            targets.Add(other.gameObject);
            lookTime = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnemyAgent"))
        {
            targets.Remove(other.gameObject);
            lookTime = 0f;
        }
    }
}
