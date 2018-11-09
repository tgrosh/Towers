using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Funds : MonoBehaviour
{
    public static Funds instance;
    public int currentFunds;
    public int incomePerInterval;
    public int incomeInterval;
    public Text fundsText;

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
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        fundsText.text = currentFunds.ToString("N0");
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

    private void UpdateFundsText()
    {
        fundsText.text = currentFunds.ToString("N0");
    }

    public void Earn(int amount)
    {
        currentFunds += amount;
        UpdateFundsText();
    }

    public bool Spend(int amount)
    {
        if (currentFunds >= amount)
        {
            currentFunds -= amount;
            UpdateFundsText();
            return true;
        }

        return false;
    }

    public bool HasFunds(int amount)
    {
        return (currentFunds >= amount);
    }
}
