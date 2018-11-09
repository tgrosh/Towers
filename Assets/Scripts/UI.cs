using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;
    public GameObject buildAreaMenu;
    public GameObject buildMenu;
    public Funds funds;
    
    private BuildArea currentBuildArea;
    private Vector3 menuPosition;
    
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
        HideBuildAreaMenu();
        HideBuildMenu();
    }

    public void ShowBuildAreaMenu(BuildArea buildArea)
    {
        currentBuildArea = buildArea;
        menuPosition = Input.mousePosition;
        ToggleMenu(buildAreaMenu, true);
    }

    public void HideBuildAreaMenu()
    {
        ToggleMenu(buildAreaMenu, false);
    }
    
    public void ShowBuildMenu()
    {
        ToggleMenu(buildMenu, true);
    }

    public void HideBuildMenu()
    {
        ToggleMenu(buildMenu, false);
    }

    public void ToggleMenu(GameObject menu, bool show)
    {
        menu.SetActive(show);
        menu.GetComponent<RectTransform>().position = menuPosition;
    }

    public void OnBuildClick(Buildable buildable)
    {
        currentBuildArea.Build(buildable);
        HideBuildMenu();
    }    
}
