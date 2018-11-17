using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
    public Sprite icon;
    public int scrapValue;

    protected NavMeshAgent agent;

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
        agent.destination = GameObject.FindGameObjectWithTag("PlayerBase").transform.position;
    }
    
    private void OnDestroy()
    {
        Scrap.instance.Earn(scrapValue);
    }
}
