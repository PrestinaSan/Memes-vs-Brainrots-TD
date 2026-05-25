
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool mouseOver = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        UIManager.instance.SetHoveringState(true);
    }
    public void OnPointerExit(PointerEventData eventData) 
    {
        mouseOver = false;
        UIManager.instance.SetHoveringState(false);
    }
}
