using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Wave", fileName = "Wave")]
public class Wave: ScriptableObject
{
    public WaveGroup[] groups;
    public float waveDelay;
    
    public List<GameObject> GetAllAgents()
    {
        List<GameObject> agents = new List<GameObject>();

        foreach (WaveGroup group in groups)
        {
            for (int x = 0; x < group.groupSize; x++)
            {
                agents.Add(group.agentPrefab);
            }
        }

        return agents;
    }
}
