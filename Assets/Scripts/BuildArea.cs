using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildArea : MonoBehaviour
{
    public Buildable currentBuildable;
    public Upgradable currentUpgradable;
    public Transform floor;

    private Animator animator;
    private Buildable buildablePrefab;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnMouseUpAsButton()
    {
        UI.instance.ShowBuildAreaMenu(this);        
    }
    
    public void Build(Buildable buildable)
    {
        if (Funds.instance.Spend(buildable.fundsCost) && Scrap.instance.Spend(buildable.scrapCost))
        {
            buildablePrefab = buildable;
            animator.SetTrigger("open");
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

    public void OnFloorDown()
    {
        GameObject obj = Instantiate(buildablePrefab.gameObject, floor);
        currentBuildable = obj.GetComponent<Buildable>();
        currentUpgradable = currentBuildable.GetComponent<Upgradable>();

        buildablePrefab = null;
    }

    public void OnActivate()
    {
        currentBuildable.GetComponent<Activatable>().Activate();
    }

    public void OnDeactivate()
    {
        currentBuildable.GetComponent<Activatable>().Deactivate();
    }
}
