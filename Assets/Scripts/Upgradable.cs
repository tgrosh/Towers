using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgradable : MonoBehaviour {    
    public UpgradeTier[] tiers = new UpgradeTier[3];
    public int currentTierIndex = 0;

    private void Start()
    {
        currentTierIndex = 0;
    }

    public bool CanUpgrade()
    {
        UpgradeTier nextTier = Next();

        return nextTier != null && Funds.instance.HasFunds(nextTier.fundsCost) && Scrap.instance.HasScrap(nextTier.scrapCost);
    }

    public UpgradeTier Next()
    {
        if (currentTierIndex < tiers.Length - 1)
        {
            return tiers[currentTierIndex + 1];
        }
        return null;
    }

    public void Upgrade()
    {
        UpgradeTier nextTier = Next();

        if (nextTier != null && Funds.instance.Spend(nextTier.fundsCost) && Scrap.instance.Spend(nextTier.scrapCost))
        {
            currentTierIndex++;
        }
    }
}
