using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseCannon : MonoBehaviour
{
    public GameObject currentTarget;
    public float lookSpeed;
    public PlasmaPulse projectilePrefab;

    [Header("Tier 1")]
    public GameObject tier1Actor;
    public GameObject tier1Body;
    public Transform tier1ProjectileSpawnPosition;
    public float tier1Range;
    public float tier1FireInterval;
    [Header("Tier 2")]
    public GameObject tier2Actor;
    public GameObject tier2Body;
    public Transform tier2ProjectileSpawnPosition;
    public float tier2Range;
    public float tier2FireInterval;
    [Header("Tier 3")]
    public GameObject tier3Actor;
    public GameObject tier3Body;
    public Transform tier3ProjectileSpawnPosition;
    public float tier3Range;
    public float tier3FireInterval;

    float fireInterval;
    GameObject body;
    Transform projectileSpawnPosition;
    Upgradable upgradable;
    List<GameObject> targets = new List<GameObject>();
    float fireTimer;
    float lookTime;
    int currentTier;
    SphereCollider rangeCollider;

    private void Start()
    {        
        upgradable = GetComponent<Upgradable>();
        rangeCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTier != upgradable.currentTierIndex + 1)
        {
            ApplyTier(upgradable.currentTierIndex + 1);
        }

        if (targets.Count > 0)
        {
            if (targets[0] == null)
            {
                targets.RemoveAt(0);
                lookTime = 0f;
                return;
            }

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

        if (fireTimer > fireInterval)
        {
            PlasmaPulse projectile = Instantiate(projectilePrefab, projectileSpawnPosition.position, projectileSpawnPosition.rotation);
            projectile.Fire(targets[0].transform);
            fireTimer = 0f;
        }
    }

    private void ApplyTier(int tier)
    {
        if (tier == 1)
        {
            tier1Actor.SetActive(true);
            tier2Actor.SetActive(false);
            tier3Actor.SetActive(false);
            body = tier1Body;
            projectileSpawnPosition = tier1ProjectileSpawnPosition;
            rangeCollider.radius = tier1Range;
            fireInterval = tier1FireInterval;
        } else if (tier == 2)
        {
            tier1Actor.SetActive(false);
            tier2Actor.SetActive(true);
            tier3Actor.SetActive(false);
            body = tier2Body;
            projectileSpawnPosition = tier2ProjectileSpawnPosition;
            rangeCollider.radius = tier2Range;
            fireInterval = tier2FireInterval;
        } else if (tier == 3)
        {
            tier1Actor.SetActive(false);
            tier2Actor.SetActive(false);
            tier3Actor.SetActive(true);
            body = tier3Body;
            projectileSpawnPosition = tier3ProjectileSpawnPosition;
            rangeCollider.radius = tier3Range;
            fireInterval = tier3FireInterval;
        }

        currentTier = tier;
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
