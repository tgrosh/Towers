using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamLaser : MonoBehaviour {
    public GameObject impactEffect;
    public int damagePerSecond;
    public DamageType damageType;

    Transform target;
    Damage damageTarget;
    LineRenderer line;
    float damageDealt;
    float damageTimer;
    GameObject impact;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            line.SetPosition(1, transform.InverseTransformPoint(target.position));
            DealDamage();
            Impact();
        }
    }

    public void Fire(Transform target)
    {
        this.target = target;
        damageTarget = this.target.transform.root.GetComponent<Damage>();
        line = GetComponentInChildren<LineRenderer>();   
    }

    private void DealDamage()
    {        
        if (damageTarget != null && damagePerSecond > 0)
        {
            damageTimer += Time.deltaTime;

            damageDealt += (damagePerSecond) * Time.deltaTime;
            if (damageDealt >= 1)
            {
                int intDamage = Mathf.RoundToInt(damageDealt);
                damageTarget.Harm(intDamage, damageType);
                damageDealt = damageDealt % 1;
            }
            
        }
    }

    private void Impact()
    {
        if (impact == null)
        {
            impact = Instantiate(impactEffect, target.position, transform.rotation, transform);
        }

        impact.transform.position = target.position;
        impact.transform.LookAt(transform.position);
    }
}
