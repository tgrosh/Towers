using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activatable : MonoBehaviour {
    public bool isActive;

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
    }
}
