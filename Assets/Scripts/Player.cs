﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public WaveSpawner enemyWaveSpawner;

    EnemyAgent[] enemies;
    bool enemiesEncountered;
    
    // Update is called once per frame
    void Update () {
        enemies = FindObjectsOfType<EnemyAgent>();
        if (enemiesEncountered && enemies.Length == 0 && enemyWaveSpawner.wavesRemaining == 0)
        {
            UI.instance.ShowYouWin();
        }

        if (enemies.Length > 0)
        {
            enemiesEncountered = true;
        }
	}
}