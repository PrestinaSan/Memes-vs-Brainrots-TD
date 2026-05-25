using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI[] buttonTexts;   // size in inspector = number of buttons
    [SerializeField] private TowerButton[] buttonScripts;     // also same size

    private Tower[] currentTowers;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentTowers = new Tower[buttonTexts.Length];

        currentTowers[0] = BuildManager.instance.TowerForUI(0, "ranged");
        currentTowers[1] = BuildManager.instance.TowerForUI(1, "ranged");
        currentTowers[2] = BuildManager.instance.TowerForUI(0, "melee");

        RefreshButtons();
    }

    public void SetButton(int slot, string name, int index, string type)
    {
        buttonTexts[slot].text = name;
        buttonScripts[slot].SetValues(index, type);
    }

    public void UpdateButton(int slot, Tower newTower)
    {
        currentTowers[slot] = newTower;
        SetButton(slot, newTower.name, newTower.index, newTower.type);
    }

    public void RefreshButtons()
    {
        for (int i = 0; i < currentTowers.Length; i++)
        {
            SetButton(
                i,
                currentTowers[i].name,
                currentTowers[i].index,
                currentTowers[i].type
            );
        }
    }

    // Optional getter so other menus can access towers
    public Tower GetTowerForSlot(int slot)
    {
        return currentTowers[slot];
    }
}
