using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scrap : MonoBehaviour {
    public static Scrap instance;
    public int currentScrap;
    public int incomePerInterval;
    public int incomeInterval;
    public Text scrapText;

    float incomeTimer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        scrapText.text = currentScrap.ToString("N0");
    }

    void Update()
    {
        incomeTimer += Time.deltaTime;

        if (incomeTimer >= incomeInterval)
        {
            incomeTimer = 0;
            Earn(incomePerInterval);
        }
    }

    private void UpdateScrapText()
    {
        scrapText.text = currentScrap.ToString("N0");
    }

    public void Earn(int amount)
    {
        currentScrap += amount;
        UpdateScrapText();
    }

    public bool Spend(int amount)
    {
        if (currentScrap >= amount)
        {
            currentScrap -= amount;
            UpdateScrapText();
            return true;
        }

        return false;
    }

    public bool HasScrap(int amount)
    {
        return (currentScrap >= amount);
    }
}
