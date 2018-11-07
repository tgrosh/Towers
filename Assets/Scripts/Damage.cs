using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {
    Health health;

	// Use this for initialization
	void Start () {
        health = GetComponent<Health>();
	}
	
    public void Harm(int amount, DamageType damageType)
    {
        if (health != null)
        {
            health.Harm(amount);
        }
    }

    public void Heal(int amount, DamageType damageType)
    {
        if (health != null)
        {
            health.Heal(amount);
        }
    }
}
