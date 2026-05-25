using Unity.Multiplayer.Center.Common;
using Unity.VisualScripting;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    [Header("References")]
    [SerializeField] private Tower[] meleeTowers;
    [SerializeField] private Tower[] rangedTowers;

    private int selectedTower = 0;
    public bool towerSelected = false;
    private int deploymentLimit = 6;
    private int deployedAmount = 0;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        LevelStatsManager.Instance.UpdateDeploymentLimit(deployedAmount, deploymentLimit);
    }

    public Tower TowerForUI(int index, string type)
    {
        if (type == "melee") return meleeTowers[index];
        else if (type == "ranged") return rangedTowers[index];
        else return null;
    }

    public Tower GetMeleeTower()
    {
        if (selectedTower <= meleeTowers.Length)
        {
            return meleeTowers[selectedTower];
        }

        else return null;
    }
    public Tower GetRangedTower()
    {
        if (selectedTower <= rangedTowers.Length)
        {
            return rangedTowers[selectedTower];
        }
        else return null;
    }
    public void SetSelectedTower(int _selectedTower)
    {
        if (deployedAmount == deploymentLimit)
        {
            Debug.Log("max towers");
            return;
        }
        if (towerSelected) return;
        selectedTower = _selectedTower;
    }
    public void TriggerPlots(string type)
    {
        if (deployedAmount == deploymentLimit) return;
        if (towerSelected) return;
        if (type == "melee")
        {
            GameObject plots = GameObject.Find("Melee Plots");
            PlotManager plotsManager = plots.GetComponent<PlotManager>();
            plotsManager.EnablePlots();
        }
        else if (type == "ranged")
        {
            GameObject plots = GameObject.Find("Ranged Plots");
            PlotManager plotsManager = plots.GetComponent<PlotManager>();
            plotsManager.EnablePlots();
        }
        towerSelected = true;
    }
    public void UpdateDeployment(int change)
    {
        deployedAmount += change;
        LevelStatsManager.Instance.UpdateDeploymentLimit(deployedAmount, deploymentLimit);
    }
}
