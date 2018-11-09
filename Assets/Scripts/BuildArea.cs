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
        if (Funds.instance.Spend(buildable.fundsCost))
        {
            Instantiate(buildable.gameObject, transform);
        }
    }
}
