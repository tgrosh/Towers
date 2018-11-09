using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildArea : MonoBehaviour
{
    public GameObject pulseCannonPrefab;
    public UI ui;
    
    private void OnMouseDown()
    {
        ui.ShowBuildAreaMenu(this);
    }

    public void Build(Buildable buildable)
    {
        if (Funds.instance.Spend(buildable.fundsCost))
        {
            Instantiate(buildable.gameObject, transform);
        }
    }
}
