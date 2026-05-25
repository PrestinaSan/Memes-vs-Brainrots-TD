using UnityEngine;

public class TowerButton : MonoBehaviour
{
    public int index;
    public string type;
    public int buttonSlot;
    public void SetValues(int newIndex, string newType)
    {
        index = newIndex;
        type = newType;
    }

    public void OnClickUpgradeMenu()
    {
        UpgradeUIManager.instance.GetButtonSlot(buttonSlot);
        UpgradeUIManager.instance.GetTowerIndex(index);
        UpgradeUIManager.instance.GetTowerType(type);
        UpgradeUIManager.instance.OpenUpgradeMenu();
    }
    public void OnClickTowerMenu()
    {
        BuildManager.instance.SetSelectedTower(index);
        BuildManager.instance.TriggerPlots(type);
        TowersMenu.instance.ToggleTowers();
    }
}
