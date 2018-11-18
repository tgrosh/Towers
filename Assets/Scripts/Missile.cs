using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {
    public float speed;
    public GameObject explosionEffect;
    public int damageAmount;
    public float damageRadius;
    public DamageType damageType;

    Vector3 targetPosition;
    Damage damageTarget;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (targetPosition != null)
        {
            transform.LookAt(targetPosition);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);

            if (Vector3.Distance(transform.position, targetPosition) < 1f)
            {
                DealDamage();
                Explode();
            }
        }
    }

    public void Fire(Transform target)
    {
        targetPosition = target.position;
    }

    private void DealDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(targetPosition, damageRadius);
        foreach (Collider hit in colliders)
        {
            if (hit.gameObject.CompareTag("HitTarget"))
            {
                Damage damage = hit.transform.root.GetComponent<Damage>();

                if (damage != null)
                {
                    damage.Harm(damageAmount, damageType);
                }
            }
        }
    }

    private void Explode()
    {
        ParticleSystem parts = transform.GetComponentInChildren<ParticleSystem>();
        parts.Stop();
        parts.transform.SetParent(null, true);

        GameObject obj = Instantiate(explosionEffect, transform.position, transform.rotation, transform.parent);
        Destroy(obj, 5);
        Destroy(gameObject);
        Destroy(parts.gameObject, 5);
    }
}
