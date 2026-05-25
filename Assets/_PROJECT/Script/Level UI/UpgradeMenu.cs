using UnityEngine;
using TMPro;
public class UpgradeMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator animator;
    public static UpgradeMenu instance;
    private bool isMenuOpen = false;

    private void Awake()
    {
        instance = this;
    }
    public bool IsMenuOpen()
    {
        return isMenuOpen;
    }
    public void ToggleMenu()
    {
        if (TowersMenu.instance.IsTowersOpen() == true)
        {
            TowersMenu.instance.ToggleTowers();
        }
        isMenuOpen = !isMenuOpen;
        animator.SetBool("Menu Open", isMenuOpen);
    }
    //public void SetSelected()
    //{
        
    //}
}
