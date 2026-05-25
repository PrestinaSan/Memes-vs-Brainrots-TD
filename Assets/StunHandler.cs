using UnityEngine;

public class TowerStunHandler : MonoBehaviour
{
    private float stunTimer = 0f;
    private bool isStunned = false;

    private MeleeTower melee;
    private RangedTower ranged;
    private HealingTower heal;

    private void Awake()
    {
        melee = GetComponent<MeleeTower>();
        ranged = GetComponent<RangedTower>();
        heal = GetComponent<HealingTower>();
    }

    public void Stun(float duration)
    {
        if (melee != null) melee.ResetAttackTimer();
        if (ranged != null) ranged.ResetAttackTimer();
        if (heal != null) heal.ResetHealTimer();

        stunTimer = duration;
        isStunned = true;
    }

    private void Update()
    {
        if (!isStunned) return;

        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0f)
            isStunned = false;
    }

    public bool IsStunned()
    {
        return isStunned;
    }
}
