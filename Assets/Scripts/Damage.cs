using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {
    Health health;
    Shield shield;

	// Use this for initialization
	void Start () {
        health = GetComponent<Health>();
        shield = GetComponent<Shield>();
	}
	
    public void Harm(float amount, DamageType damageType)
    {
        float damage = amount;
        float energyShieldRedirect = .8f;

        if (shield != null && shield.currentShield > 0)
        {
            if (damageType == DamageType.Energy)
            {
                shield.Harm(damage * energyShieldRedirect);
            }
            damage *= (1 - energyShieldRedirect);
        }        

        if (health != null)
        {
            if (damageType == DamageType.Energy)
            {
                damage = Mathf.Min(damage, amount * (1 - energyShieldRedirect));
            }
            health.Harm(damage);
        }
    }

    public void Heal(float amount, DamageType damageType)
    {
        if (health != null)
        {
            health.Heal(amount);
        }
    }
}
