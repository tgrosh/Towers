using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MIUController : MonoBehaviour {
    public UnitOwner owner;
    public GameObject actor;
    [Range(0, 1)]
    public float speedAnimatorFactor;
    public float lookSpeed;
    public float fireInterval;
    public PlasmaPulse projectilePrefab;
    public Transform projectileSpawnPosition;

    NavMeshAgent agent;
    Animator actorAnimator;
    List<GameObject> targets = new List<GameObject>();
    Transform currentTarget;
    float lookTime;
    float fireTimer;
    string targetTag;

    void Start () {
        agent = GetComponent<NavMeshAgent>();
        actorAnimator = actor.GetComponent<Animator>();

        if (CompareTag("PlayerAgent"))
        {
            targetTag = "EnemyAgent";
        } else if (CompareTag("EnemyAgent"))
        {
            targetTag = "PlayerAgent";
        }
    }

    private void Update()
    {
        actorAnimator.SetFloat("speed", agent.velocity.magnitude * speedAnimatorFactor);

        if (targets.Count > 0)
        {
            if (targets[0] == null)
            {
                targets.RemoveAt(0);
                lookTime = 0f;
                return;
            }

            agent.isStopped = true;

            lookTime += Time.deltaTime;
            Vector3 direction = targets[0].transform.position - transform.position;
            direction = new Vector3(direction.x, 0f, direction.z); //take out y
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, lookSpeed * lookTime);

            Fire(targets[0].transform);
        } else
        {
            agent.isStopped = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HitTarget") && other.transform.root.CompareTag(targetTag))
        {
            targets.Add(other.gameObject);
            lookTime = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("HitTarget") && other.transform.root.CompareTag(targetTag))
        {
            targets.Remove(other.gameObject);
            lookTime = 0f;
        }
    }

    private void Fire(Transform target)
    {
        fireTimer += Time.deltaTime;

        if (fireTimer > fireInterval)
        {
            PlasmaPulse projectile = Instantiate(projectilePrefab, projectileSpawnPosition.position, projectileSpawnPosition.rotation);
            projectile.Fire(target);
            fireTimer = 0f;
        }
    }
}
