using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour {
    public UnitOwner owner;

    WaveSpawner waveSpawner;

    private void OnMouseUpAsButton()
    {
        UI.instance.ShowBaseMenu(this);
    }

    private void Start()
    {
        waveSpawner = GetComponentInChildren<WaveSpawner>();
    }

    public void SpawnWave(Wave wave)
    {
        waveSpawner.SpawnWave(wave);
    }
}

public enum UnitOwner
{
    Player,
    Enemy
}
