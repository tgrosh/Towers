using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
    public float currentHealth;
    public float maxHealth;
    public Image healthBar;

    private void Start()
    {
        UpdateHealthBar();
    }

    public void Harm(float amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            Die();
        }
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.transform.parent.gameObject.SetActive(currentHealth != maxHealth);
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }

    public void Heal(float amount)
    {
        currentHealth -= amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthBar();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
