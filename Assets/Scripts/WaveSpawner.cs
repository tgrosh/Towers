using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {
    public Transform spawnPoint;
    public float spawnDelay;
    public Wave[] waves;
    public Color color = Color.green;
    public int wavesRemaining;

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

    void Start ()
    {
        wavesRemaining = waves.Length;
        int wavesDisplayed = 0;
        foreach (Wave wave in waves)
        {
            if (wavesDisplayed < UI.instance.wavesToDisplay)
            {
                UI.instance.EnqueueWave(wave);
            }
            wavesDisplayed++;
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
            SpawnNextWave();
        }
	}

    void SpawnNextWave()
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

                wavesRemaining--;
                UI.instance.DequeueWave();
                if (waves.Length >= currentWave + UI.instance.wavesToDisplay)
                {
                    UI.instance.EnqueueWave(waves[currentWave + UI.instance.wavesToDisplay - 1]);
                }
            }
        }
    }

    public void SpawnWave(Wave wave)
    {
        foreach (GameObject agent in wave.GetAllAgents())
        {
            Spawn(agent);
        }
    }
    
    void Spawn(GameObject prefab)
    {
        Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }
}
