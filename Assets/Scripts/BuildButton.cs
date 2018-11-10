using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour {
    public Buildable buildable;

    Button button;

	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
        button.interactable = Funds.instance.HasFunds(buildable.fundsCost) && Scrap.instance.HasScrap(buildable.scrapCost);
	}

    public void OnClick()
    {
        UI.instance.OnBuildClick(buildable);
    }
}
