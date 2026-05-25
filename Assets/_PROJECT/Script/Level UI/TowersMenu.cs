using UnityEngine;
using TMPro;

public class TowersMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;

    [Header("Tower Selection Buttons")]
    [SerializeField] private TextMeshProUGUI[] buttonTexts;
    [SerializeField] private TowerButton[] buttonScripts;

    public static TowersMenu instance;
    private bool isTowersOpen = true;

    private void Awake()
    {
        instance = this;
    }

    public bool IsTowersOpen()
    {
        return isTowersOpen;
    }

    public void ToggleTowers()
    {
        // First refresh UpgradeManager buttons
        UpgradeManager.instance.RefreshButtons();

        // Then sync tower selection UI from UpgradeManager
        SyncFromUpgradeManager();

        // Close Upgrade Menu if open
        if (UpgradeMenu.instance.IsMenuOpen())
        {
            UpgradeMenu.instance.ToggleMenu();
        }

        isTowersOpen = !isTowersOpen;
        animator.SetBool("Tower Open", isTowersOpen);
    }
    private void SyncFromUpgradeManager()
    {
        for (int i = 0; i < buttonTexts.Length; i++)
        {
            Tower t = UpgradeManager.instance.GetTowerForSlot(i);

            buttonTexts[i].text = t.name;
            buttonScripts[i].SetValues(t.index, t.type);
        }
    }
}
