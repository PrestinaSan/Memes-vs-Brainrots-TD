using UnityEngine;

public class FreddySelfBuff : MonoBehaviour
{
    private MeleeTower melee;
    private AnimationHandler animationHandler;

    private float timer = 0f;
    private bool isBuffing = false;

    [SerializeField] private float buffSpeed = 5f;     // time between buffs
    [SerializeField] private float buffDuration = 5f;  // how long buff lasts
    [SerializeField] private float buffMultiplier = 1.5f;

    // store original damage so we can restore it exactly
    private float baseDamage = -1f;

    private void Start()
    {
        melee = GetComponent<MeleeTower>();
        animationHandler = GetComponentInChildren<AnimationHandler>();
        isBuffing = false;
        timer = 0f;
    }

    private void Update()
    {
        if (!isBuffing)
        {
            timer += Time.deltaTime;
            if (timer >= buffSpeed)
            {
                timer = 0f;
                StartBuff();
            }
        }
        else
        {
            // buff is active — do NOT reapply it every frame
            timer += Time.deltaTime;
            if (timer >= buffDuration)
            {
                timer = 0f;
                EndBuff();
            }
        }
    }

    private void StartBuff()
    {
        if (melee == null) return;

        // cache base damage once
        if (baseDamage < 0f)
            baseDamage = melee.GetDamage();

        // apply buff once
        melee.SetDamage(baseDamage * buffMultiplier);

        // play eat animation
        if (animationHandler != null)
            animationHandler.OnPizzaEatFreddy();

        isBuffing = true;
    }

    private void EndBuff()
    {
        // restore original damage (use cached baseDamage to avoid float drift)
        if (melee != null && baseDamage >= 0f)
            melee.SetDamage(baseDamage);

        // ensure animator returns to idle (uses your existing handler)
        if (animationHandler != null)
            animationHandler.OnAttackAnimationEndFreddy();

        isBuffing = false;
    }
}
