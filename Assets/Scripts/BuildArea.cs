using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildArea : MonoBehaviour
{    
    private void OnMouseDown()
    {
        UI.instance.ShowBuildAreaMenu(this);
    }

    public void Build(Buildable buildable)
    {
        if (Funds.instance.Spend(buildable.fundsCost) && Scrap.instance.Spend(buildable.scrapCost))
        {
            Instantiate(buildable.gameObject, transform.position, transform.rotation, transform);
        }
    }
}
