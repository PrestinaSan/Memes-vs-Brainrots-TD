using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private bool isHoveringUI = false;

    private void Awake()
    {
        instance = this;
    }
    public void SetHoveringState(bool state)
    {
        isHoveringUI = state;
    }
    public bool IsHoveringUI()
    {
        return isHoveringUI;
    }
}
