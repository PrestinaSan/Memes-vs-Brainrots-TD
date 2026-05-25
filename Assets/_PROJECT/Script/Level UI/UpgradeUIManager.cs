using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UpgradeUIManager : MonoBehaviour
{
    [Header("References")]
    private Tower displayedTower;
    public static UpgradeUIManager instance;
    [SerializeField] private GameObject towerList;
    [SerializeField] private GameObject towerInfo;
    [SerializeField] private TMP_Text towerName;
    [SerializeField] private TMP_Text abiltyDescription;
    [SerializeField] private TMP_Text towerStats;
    [SerializeField] private TMP_Text upgradeCostDisplay;
    [SerializeField] private TMP_Text upgradeButtonText;
    [Header("Attributes")]
    private float bps;
    private float damage;
    private float hp;
    private int index;
    private string type;
    private int buttonSlot;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        bps = 0; damage = 0; hp = 0;
        BackToList();
    }
    public void DisplayTower()
    {
        if (displayedTower.nextUpgrade == 0)
        {
            upgradeButtonText.text = "Max Level";
        }
        else
        {
            upgradeButtonText.text = "Upgrade";
        }
        if (type == "ranged")
        {
            if (buttonSlot == 1)
            {
                bps = displayedTower.prefab.GetComponent<HealingTower>().GetBPS();
                damage = displayedTower.prefab.GetComponent<HealingTower>().GetProjectile().GetComponent<BulletScript>().GetDamage();
                towerName.text = displayedTower.name;
                abiltyDescription.text = displayedTower.description;
                towerStats.text = "BPS: " + bps.ToString() + '\n' + "Heal: " + damage.ToString() + '\n' + "Cost: " + displayedTower.cost.ToString();
                upgradeCostDisplay.text = "Upgrade Cost: " + displayedTower.upgradeCost.ToString();
                return;
            }
            bps = displayedTower.prefab.GetComponent<RangedTower>().GetBPS();
            damage = displayedTower.prefab.GetComponent<RangedTower>().GetProjectile().GetComponent<BulletScript>().GetDamage();
            towerName.text = displayedTower.name;
            abiltyDescription.text = displayedTower.description;
            towerStats.text = "BPS: " + bps.ToString() + '\n' + "Damage: " + damage.ToString() + '\n' + "Cost: " + displayedTower.cost.ToString();
            upgradeCostDisplay.text = "Upgrade Cost: " + displayedTower.upgradeCost.ToString();
        }
        else if (type == "melee")
        {
            hp = displayedTower.prefab.GetComponent<HealthManagerScript>().GetMaxHP();
            damage = displayedTower.prefab.GetComponent<MeleeTower>().GetDamage();
            towerName.text = displayedTower.name;
            abiltyDescription.text = displayedTower.description;
            upgradeCostDisplay.text = "Upgrade Cost: " + displayedTower.upgradeCost.ToString();
            towerStats.text = "HP: " + hp.ToString() + '\n' + "Damage: " + damage.ToString() + '\n' + "Cost: " + displayedTower.cost.ToString();

        }
    }
    public void GetTowerIndex(int _index)
    {
        index = _index;
    }
    public void GetTowerType(string _type)
    {
        type = _type;
    }
    public void OpenUpgradeMenu()
    {

        displayedTower = BuildManager.instance.TowerForUI(index,type);
        DisplayTower();
        towerList.SetActive(false);
        towerInfo.SetActive(true);
    }
    public void BackToList()
    {
        towerInfo.SetActive(false);
        towerList.SetActive(true);
    }
    public void GetButtonSlot(int slot)
    {
        buttonSlot = slot;
    }
    public void OnUpgradeClick()
    {
        if (displayedTower.nextUpgrade == 0)
        {
            Debug.Log("no more upgrades");
            return;
        }
        if (!DPManager.instance.SpendDP(displayedTower.upgradeCost))
            return;
        Tower newTower = BuildManager.instance.TowerForUI(displayedTower.nextUpgrade, displayedTower.type);

        UpgradeManager.instance.UpdateButton(buttonSlot, newTower);
        UpgradeManager.instance.RefreshButtons();
        displayedTower = newTower;
        DisplayTower();
    }
}
