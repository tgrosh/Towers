using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public Sprite icon;
    public int scrapValue;

    NavMeshAgent agent;
    GameObject destination;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        NavMeshHit closestHit;
        if (NavMesh.SamplePosition(transform.position, out closestHit, 100f, NavMesh.AllAreas))
        {
            transform.position = closestHit.position;
            agent.enabled = true;
        }

        if (CompareTag("PlayerAgent"))
        {
            agent.destination = GameObject.FindGameObjectWithTag("EnemyBase").transform.position;
        }
        else if (CompareTag("EnemyAgent"))
        {
            agent.destination = GameObject.FindGameObjectWithTag("PlayerBase").transform.position;
        }
    }
    
    private void OnDestroy()
    {
        Scrap.instance.Earn(scrapValue);
    }
}
