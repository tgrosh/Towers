using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaPulse : MonoBehaviour
{
    public float speed;
    public GameObject sparksEffect;
    public DamageType damageType;

    Transform target;    

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.LookAt(target);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);

            if (Vector3.Distance(transform.position, target.position) < 1f)
            {
                Explode();
            }
        }
    }

    public void Fire(Transform target)
    {
        this.target = target;
    }

    private void Explode()
    {
        GameObject obj = Instantiate(sparksEffect, transform.position, transform.rotation, transform.parent);
        Destroy(obj, 2);
        Destroy(gameObject);
    }
}
