using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {
    public Transform spawnPoint;
    public float spawnDelay;
    public Wave[] waves;
    public Color color = Color.green;

    float waveTimer;
    float groupTimer;
    float spawnTimer;
    int currentWave;
    List<GameObject> allAgents = new List<GameObject>();

    private void OnDrawGizmos()
    {
        color.a = 0.5f;
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, transform.localScale.x);
    }

    void Start () {
		foreach (Wave wave in waves)
        {
            UI.instance.PushWave(wave);
        }
	}
	
	void Update () {
        if (currentWave >= waves.Length)
        {
            return;
        }

        waveTimer += Time.deltaTime;

        if (waveTimer > waves[currentWave].waveDelay)
        {
            SpawnWave(waves[currentWave]);
        }
	}

    void SpawnWave(Wave wave)
    {
        if (allAgents.Count == 0)
        {
            allAgents = waves[currentWave].GetAllAgents();
        }
        int currentAgent;

        spawnTimer += Time.deltaTime;

        if (spawnTimer > spawnDelay)
        {
            spawnTimer = 0f;
            currentAgent = Random.Range(0, allAgents.Count - 1);
            Spawn(allAgents[currentAgent]);
            allAgents.RemoveAt(currentAgent);
            
            if (allAgents.Count == 0)
            {
                currentWave++;
                waveTimer = 0;
                UI.instance.PopWave();
            }
        }
    }
    
    void Spawn(GameObject prefab)
    {
        Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }
}
