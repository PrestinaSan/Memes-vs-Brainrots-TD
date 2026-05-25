using System.Threading;
using UnityEngine;

public class ChimpaniziniSelfHeal : MonoBehaviour
{
    private HealthManagerScript healthManagerScript;
    [SerializeField] private float healCooldown = 15f;
    [SerializeField] private int healAmount = 1000;
    private float timer;
    private void Start()
    {
        healthManagerScript = GetComponent<HealthManagerScript>();
        //animationHandler = GetComponentInChildren<AnimationHandler>();
        timer = 0f;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= healCooldown)
        {
            timer = 0f;
            healthManagerScript.Heal(healAmount);
        }
    }
}
