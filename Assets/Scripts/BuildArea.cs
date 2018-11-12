using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildArea : MonoBehaviour
{
    public Buildable currentBuildable;
    public Upgradable currentUpgradable;

    private void OnMouseUpAsButton()
    {
        UI.instance.ShowBuildAreaMenu(this);        
    }
    
    public void Build(Buildable buildable)
    {
        if (Funds.instance.Spend(buildable.fundsCost) && Scrap.instance.Spend(buildable.scrapCost))
        {
            GameObject obj = Instantiate(buildable.gameObject, transform.position, transform.rotation, transform);
            currentBuildable = obj.GetComponent<Buildable>();
            currentUpgradable = currentBuildable.GetComponent<Upgradable>();
        }
    }

    public void Upgrade()
    {        
        if (currentUpgradable != null)
        {
            currentUpgradable.Upgrade();
        }
    }

    public void Sell()
    {
        if (currentBuildable != null)
        {
            Funds.instance.Earn(currentBuildable.fundsCost / 2);
            Destroy(currentBuildable.gameObject);
            currentBuildable = null;
            currentUpgradable = null;
        }
    }
}
