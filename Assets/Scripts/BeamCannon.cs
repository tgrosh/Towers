using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamCannon : MonoBehaviour {
    public GameObject currentTarget;
    public float lookSpeed;

    [Header("Tier 1")]
    public GameObject tier1Actor;
    public GameObject tier1Body;
    public Transform tier1ProjectileSpawnPosition;
    public float tier1Range;
    public BeamLaser tier1LaserPrefab;
    [Header("Tier 2")]
    public GameObject tier2Actor;
    public GameObject tier2Body;
    public Transform tier2ProjectileSpawnPosition;
    public float tier2Range;
    public BeamLaser tier2LaserPrefab;
    [Header("Tier 3")]
    public GameObject tier3Actor;
    public GameObject tier3Body;
    public Transform tier3ProjectileSpawnPosition;
    public float tier3Range;
    public BeamLaser tier3LaserPrefab;

    BeamLaser laserPrefab;
    GameObject body;
    Transform projectileSpawnPosition;
    Upgradable upgradable;
    List<GameObject> unshieldedTargets = new List<GameObject>();
    List<GameObject> shieldedTargets = new List<GameObject>();
    float fireTimer;
    float lookTime;
    int currentTier;
    bool firing;
    BeamLaser laser;
    SphereCollider rangeCollider;
    List<GameObject> targetList;

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

        //determine which list to work from
        if (shieldedTargets.Count > 0)
        {
            targetList = shieldedTargets;
        }
        else if (unshieldedTargets.Count > 0)
        {
            targetList = unshieldedTargets;
        } else
        {
            targetList = null;
        }

        if (targetList != null)
        {
            if (currentTarget != targetList[0])
            {
                currentTarget = targetList[0];
                ResetTarget();
            }

            if (currentTarget == null)
            {
                targetList.Remove(currentTarget);
                ResetTarget();
                return;
            }
            
            lookTime += Time.deltaTime;
            Vector3 direction = currentTarget.transform.position - body.transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction);
            body.transform.rotation = Quaternion.Slerp(body.transform.rotation, toRotation, lookSpeed * lookTime);

            if (Vector3.Angle(body.transform.transform.forward, direction) < 1f)
            {
                Fire();
            }
            
            if (!HasActiveShield(currentTarget))
            {
                if (shieldedTargets.Remove(currentTarget))
                {
                    unshieldedTargets.Add(currentTarget);
                    ResetTarget();
                }
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
            laser.Fire(targetList[0].transform);
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
            rangeCollider.radius = tier1Range;
            laserPrefab = tier1LaserPrefab;
        }
        else if (tier == 2)
        {
            tier1Actor.SetActive(false);
            tier2Actor.SetActive(true);
            tier3Actor.SetActive(false);
            body = tier2Body;
            projectileSpawnPosition = tier2ProjectileSpawnPosition;
            rangeCollider.radius = tier2Range;
            laserPrefab = tier2LaserPrefab;
        }
        else if (tier == 3)
        {
            tier1Actor.SetActive(false);
            tier2Actor.SetActive(false);
            tier3Actor.SetActive(true);
            body = tier3Body;
            projectileSpawnPosition = tier3ProjectileSpawnPosition;
            rangeCollider.radius = tier3Range;
            laserPrefab = tier3LaserPrefab;
        }

        currentTier = tier;
        ResetTarget();
    }

    private bool HasActiveShield(GameObject target)
    {
        Shield shield = target.GetComponent<Shield>();

        return (shield != null && shield.currentShield > 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAgent"))
        {   
            if (HasActiveShield(other.gameObject))
            {
                shieldedTargets.Add(other.gameObject);
            } else
            {
                unshieldedTargets.Add(other.gameObject);
            }
            lookTime = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnemyAgent"))
        {
            shieldedTargets.Remove(other.gameObject);
            unshieldedTargets.Remove(other.gameObject);
            if (currentTarget == other.gameObject)
            {
                ResetTarget();
            }
        }
    }
}
