
using UnityEngine;

public class PlotScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    PlotManager plotsManager = null;
    public GameObject towerObj;
    public RangedTower rangedTower;
    public MeleeTower meleeTower;

    [Header("Attributes")]
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color startColor;
    private string typeHolder = null;
    private int costHolder = 0;


    void Start()
    {
        startColor = sr.color;
        plotsManager = GetComponentInParent<PlotManager>();

    }
    private void OnMouseEnter()
    {
        if (towerObj == null)
            sr.color = hoverColor;
        if (rangedTower != null)
        {
            rangedTower.ShowRange();
        }
    }
    private void OnMouseExit()
    {
        if (towerObj == null)
        ResetColor();
        if (rangedTower != null)
        {
            rangedTower.HideRange();
        }
    }
    private void OnMouseDown()
    {
        if (UIManager.instance.IsHoveringUI()) return;
        if (towerObj != null)
        {
            if (typeHolder == "ranged")
            {
                rangedTower.OpenSellUI();
            }
            else if (typeHolder == "melee")
            {
                meleeTower.OpenSellUI();
            }
            return;
        }
        Tower towerToBuild = null;
        if (gameObject.CompareTag("Melee Tiles"))
        {
            towerToBuild = BuildManager.instance.GetMeleeTower();
        }
        else if (gameObject.CompareTag("Ranged Tiles"))
        {
            towerToBuild = BuildManager.instance.GetRangedTower();
        }
        if (DPManager.instance.SpendDP(towerToBuild.cost) == false)
        {
            BuildManager.instance.towerSelected = false;
            plotsManager.DisablePlots();
            return;
        }
        towerObj = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
        BuildManager.instance.UpdateDeployment(1);
        if (towerToBuild.type == "ranged")
        {
            towerObj.GetComponent<RangedTower>().plot = this;
            rangedTower = towerObj.GetComponent<RangedTower>();
        }

        else if (towerToBuild.type == "melee")
        {
            towerObj.GetComponent<MeleeTower>().plot = this;
            meleeTower = towerObj.GetComponent<MeleeTower>();
        }
        BuildManager.instance.towerSelected = false;
        typeHolder = towerToBuild.type;
        costHolder = towerToBuild.cost;
        plotsManager.DisablePlots();
    }
    public void ResetColor()
    {
        sr.color = startColor;
    }
    public void TurnInvisible()
    {
        sr.color = new Color(1f, 1f, 1f, 0f);
    }
    public void SellTower()
    {
        DPManager.instance.GainDP(costHolder/2);
        Destroy(towerObj);
        towerObj = null;
        UIManager.instance.SetHoveringState(false);
        BuildManager.instance.UpdateDeployment(-1);
    }
    public void TowerDies()
    {
        Destroy(towerObj);
        towerObj = null;
        gameObject.SetActive(false);
        BuildManager.instance.UpdateDeployment(-1);
    }
}