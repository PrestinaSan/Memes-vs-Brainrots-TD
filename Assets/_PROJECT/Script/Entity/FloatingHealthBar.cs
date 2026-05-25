using UnityEngine;
using UnityEngine.UI;
public class FloatingHealthBar : MonoBehaviour
{

    [SerializeField] private Slider slider;
    [SerializeField] private float offset = 80;

    private void Start()
    {
        slider.transform.localPosition += new Vector3(0, offset);
    }

    public void UpdateHealthBar(float currentvalue, float maxValue)
    {
        slider.value = currentvalue / maxValue;
    }
}
