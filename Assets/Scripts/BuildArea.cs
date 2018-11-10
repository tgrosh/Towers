using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildArea : MonoBehaviour
{
    Buildable currentBuildable;
    Upgradable currentUpgradable;

    private void OnMouseUpAsButton()
    {
        UI.instance.ShowBuildAreaMenu(this);
        UI.instance.ToggleBuildButton(currentBuildable == null);
        UI.instance.ToggleUpgradeButton(currentUpgradable != null && currentUpgradable.CanUpgrade());
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
}
