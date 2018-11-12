﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamCannon : MonoBehaviour {
    public GameObject currentTarget;
    public float lookSpeed;
    public BeamLaser laserPrefab;

    [Header("Tier 1")]
    public GameObject tier1Actor;
    public GameObject tier1Body;
    public Transform tier1ProjectileSpawnPosition;
    [Header("Tier 2")]
    public GameObject tier2Actor;
    public GameObject tier2Body;
    public Transform tier2ProjectileSpawnPosition;
    [Header("Tier 3")]
    public GameObject tier3Actor;
    public GameObject tier3Body;
    public Transform tier3ProjectileSpawnPosition;

    GameObject body;
    Transform projectileSpawnPosition;
    Upgradable upgradable;
    List<GameObject> targets = new List<GameObject>();
    float fireTimer;
    float lookTime;
    int currentTier;
    bool firing;
    BeamLaser laser;

    private void Start()
    {
        upgradable = GetComponent<Upgradable>();
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
                ResetTarget();
                return;
            }

            lookTime += Time.deltaTime;
            Vector3 direction = targets[0].transform.position - body.transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction);
            body.transform.rotation = Quaternion.Slerp(body.transform.rotation, toRotation, lookSpeed * lookTime);

            if (Vector3.Angle(body.transform.transform.forward, direction) < 1f)
            {
                Fire();
            }
        }
    }

    private void ResetTarget()
    {
        lookTime = 0f;
        if (laser != null)
        {
            Destroy(laser.gameObject);
        }
        firing = false;
    }

    private void Fire()
    {
        if (!firing)
        {            
            laser = Instantiate(laserPrefab, projectileSpawnPosition.position, projectileSpawnPosition.rotation, projectileSpawnPosition);
            laser.Fire(targets[0].transform);
        }
        
        firing = true;
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
        }
        else if (tier == 2)
        {
            tier1Actor.SetActive(false);
            tier2Actor.SetActive(true);
            tier3Actor.SetActive(false);
            body = tier2Body;
            projectileSpawnPosition = tier2ProjectileSpawnPosition;
        }
        else if (tier == 3)
        {
            tier1Actor.SetActive(false);
            tier2Actor.SetActive(false);
            tier3Actor.SetActive(true);
            body = tier3Body;
            projectileSpawnPosition = tier3ProjectileSpawnPosition;
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
            ResetTarget();
        }
    }
}
