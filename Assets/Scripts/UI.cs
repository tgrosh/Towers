using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;
    public GameObject gameOverPanel;
    public GameObject buildAreaMenu;
    public GameObject buildMenu;
    public GameObject wavePanel;
    public GameObject waveUIPrefab;
    public GameObject groupUIPrefab;
    
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
    }

    private void Start()
    {
        HideBuildAreaMenu();
        HideBuildMenu();
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
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

    public void PushWave(Wave wave)
    {
        GameObject waveUI = Instantiate(waveUIPrefab, wavePanel.transform);
        waveUI.GetComponent<LayoutElement>().minHeight = (waveUI.GetComponent<LayoutElement>().minHeight * wave.groups.Length) + 5;

        foreach (WaveGroup waveGroup in wave.groups)
        {
            GameObject waveGroupUI = Instantiate(groupUIPrefab, waveUI.transform.GetChild(0).transform);
            waveGroupUI.GetComponentInChildren<Image>().sprite = waveGroup.agentPrefab.GetComponent<EnemyAgent>().icon;
            waveGroupUI.GetComponentInChildren<Text>().text = "x" + waveGroup.groupSize;
        }
    }

    public void PopWave()
    {
        wavePanel.transform.GetChild(1).GetComponent<Animator>().SetTrigger("Exit");
        Destroy(wavePanel.transform.GetChild(1).gameObject, 1);
    }
}
