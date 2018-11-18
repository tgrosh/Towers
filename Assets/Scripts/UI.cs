using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;
    public GameObject gameOverPanel;
    public GameObject youWinPanel;
    public GameObject baseMenu;
    public GameObject buildAreaMenu;
    public GameObject buildMenu;
    public Button buildButton;
    public Button upgradeButton;
    public Button sellButton;
    public int wavesToDisplay;
    public GameObject wavePanel;
    public GameObject waveUIPrefab;
    public GameObject groupUIPrefab;

    private Base currentBase;
    private BuildArea currentBuildArea;
    private Vector3 menuPosition;
    private List<Wave> waves = new List<Wave>();

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HideBuildAreaMenu();
            HideBuildMenu();
        }

        buildButton.interactable = (currentBuildArea != null && currentBuildArea.currentBuildable == null);
        upgradeButton.interactable = (currentBuildArea != null && currentBuildArea.currentUpgradable != null && currentBuildArea.currentUpgradable.CanUpgrade());
        sellButton.interactable = (currentBuildArea != null && currentBuildArea.currentBuildable != null);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void ShowYouWin()
    {
        youWinPanel.SetActive(true);
    }

    public void ShowBuildAreaMenu(BuildArea buildArea)
    {
        currentBuildArea = buildArea;
        menuPosition = Input.mousePosition;
        ToggleMenu(buildAreaMenu, true);
    }

    public void ShowBaseMenu(Base playerBase)
    {
        currentBase = playerBase;
        menuPosition = Input.mousePosition;
        ToggleMenu(baseMenu, true);
    }

    public void HideBaseMenu()
    {
        ToggleMenu(baseMenu, false);
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

    public void OnSpawnWaveClick(Wave wave)
    {
        currentBase.SpawnWave(wave);
        HideBaseMenu();
    }

    public void OnBuildClick(Buildable buildable)
    {
        currentBuildArea.Build(buildable);
        HideBuildMenu();
    }

    public void OnUpgradeClick()
    {
        currentBuildArea.Upgrade();
        HideBuildAreaMenu();
    }

    public void OnSellClick()
    {
        currentBuildArea.Sell();
        HideBuildAreaMenu();
    }

    public void EnqueueWave(Wave wave)
    {
        GameObject waveUI = Instantiate(waveUIPrefab, wavePanel.transform);
        waveUI.GetComponent<LayoutElement>().minHeight = (waveUI.GetComponent<LayoutElement>().minHeight * wave.groups.Length) + 5;

        foreach (WaveGroup waveGroup in wave.groups)
        {
            GameObject waveGroupUI = Instantiate(groupUIPrefab, waveUI.transform.GetChild(0).transform);
            waveGroupUI.GetComponentInChildren<Image>().sprite = waveGroup.agentPrefab.GetComponent<Agent>().icon;
            waveGroupUI.GetComponentInChildren<Text>().text = "x" + waveGroup.groupSize;
        }
    }

    public void DequeueWave()
    {
        wavePanel.transform.GetChild(1).GetComponent<Animator>().SetTrigger("Exit");
        Destroy(wavePanel.transform.GetChild(1).gameObject, 1);
    }
}
