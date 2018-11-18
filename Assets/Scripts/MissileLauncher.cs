using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour {
    public GameObject currentTarget;
    public float lookSpeed;
    public float fireInterval;

    [Header("Tier 1")]
    public GameObject tier1Actor;
    public GameObject tier1Body;
    public Transform tier1ProjectileSpawnPosition;
    public Missile tier1ProjectilePrefab;
    [Header("Tier 2")]
    public GameObject tier2Actor;
    public GameObject tier2Body;
    public Transform tier2ProjectileSpawnPosition;
    public Missile tier2ProjectilePrefab;
    [Header("Tier 3")]
    public GameObject tier3Actor;
    public GameObject tier3Body;
    public Transform tier3ProjectileSpawnPosition;
    public Missile tier3ProjectilePrefab;

    GameObject body;
    Missile projectilePrefab;
    Transform projectileSpawnPosition;
    
    List<GameObject> targets = new List<GameObject>();
    float fireTimer;
    float lookTime;
    Upgradable upgradable;
    int currentTier;

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
            Missile projectile = Instantiate(projectilePrefab, projectileSpawnPosition.position, projectileSpawnPosition.rotation);
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
            projectilePrefab = tier1ProjectilePrefab;
        }
        else if (tier == 2)
        {
            tier1Actor.SetActive(false);
            tier2Actor.SetActive(true);
            tier3Actor.SetActive(false);
            body = tier2Body;
            projectileSpawnPosition = tier2ProjectileSpawnPosition;
            projectilePrefab = tier2ProjectilePrefab;
        }
        else if (tier == 3)
        {
            tier1Actor.SetActive(false);
            tier2Actor.SetActive(false);
            tier3Actor.SetActive(true);
            body = tier3Body;
            projectileSpawnPosition = tier3ProjectileSpawnPosition;
            projectilePrefab = tier3ProjectilePrefab;
        }

        currentTier = tier;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("EnemyAgent"))
        {
            targets.Add(other.gameObject);
            lookTime = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.CompareTag("EnemyAgent"))
        {
            targets.Remove(other.gameObject);
            lookTime = 0f;
        }
    }
}
