using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {
    public float currentShield;
    public float maxShield;
    public GameObject shield;

    Animator animator;

    private void Start()
    {
        animator = shield.GetComponent<Animator>();
    }

    private void Update()
    {
        shield.SetActive(currentShield > 0);
        animator.SetBool("takingDamage", false);
    }

    public void Harm(float amount)
    {
        currentShield -= amount;
        animator.SetBool("takingDamage", true);
        if (currentShield < 0)
        {
            currentShield = 0;            
        }
    }

    public void Heal(float amount)
    {
        currentShield -= amount;
        if (currentShield > maxShield)
        {
            currentShield = maxShield;
        }
    }
}
