using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RangedBuffTower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform sprite;
    [SerializeField] private LayerMask allyMask;
    private AnimationHandler animator;

    [Header("Attributes")]
    [SerializeField] private float buffRange = 3f;
    [SerializeField] private float buffMultiplier = 1.1f;
    [SerializeField] private bool isBuffing = false;
    [SerializeField] private float buffSpeed = 5f;
    [SerializeField] private float buffDuration = 5f;
    private float timer = 0f;

    private List<RangedTower> buffedRanged = new();
    private List<MeleeTower> buffedMelee = new();
    private List<HealingTower> buffedHealing = new();

    private void Start()
    {
        animator = GetComponentInChildren<AnimationHandler>();
        isBuffing = false;
    }
    private void Update()
    {
        animator.OnBuffAnimationStartBongo(isBuffing);
        if (!isBuffing)
        {
            timer += Time.deltaTime;
            if (timer >= buffSpeed)
            {
                timer = 0f;
                isBuffing = true;
            }
        }
        else
        {
            ApplyBuffs();
            timer += Time.deltaTime;
            if (timer >= buffDuration)
            {
                timer = 0f;
                isBuffing = false;
                RemoveAllBuffs();
            }
        }

    }
    private void ApplyBuffs()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, buffRange, allyMask);
        // --- REMOVE BUFFS FROM RANGED ---
        for (int i = buffedRanged.Count - 1; i >= 0; i--)
        {
            if (!ContainsCollider(hits, buffedRanged[i]))
            {
                RemoveRangedBuff(buffedRanged[i]);
                buffedRanged.RemoveAt(i);
            }
        }

        // --- REMOVE BUFFS FROM MELEE ---
        for (int i = buffedMelee.Count - 1; i >= 0; i--)
        {
            if (!ContainsCollider(hits, buffedMelee[i]))
            {
                RemoveMeleeBuff(buffedMelee[i]);
                buffedMelee.RemoveAt(i);
            }
        }
        // --- REMOVE BUFFS FROM HEALING ---
        for (int i = buffedHealing.Count - 1; i >= 0; i--)
        {
            if (!ContainsCollider(hits, buffedHealing[i]))
            {
                RemoveHealBuff(buffedHealing[i]);
                buffedHealing.RemoveAt(i);
            }
        }

        // --- ADD NEW BUFFS ---
        foreach (var hit in hits)
        {
            // RANGED
            RangedTower ranged = hit.GetComponent<RangedTower>();
            if (ranged != null && !buffedRanged.Contains(ranged))
            {
                ApplyRangedBuff(ranged);
                buffedRanged.Add(ranged);
            }

            // MELEE
            MeleeTower melee = hit.GetComponent<MeleeTower>();
            if (melee != null && !buffedMelee.Contains(melee))
            {
                ApplyMeleeBuff(melee);
                buffedMelee.Add(melee);
            }
            // HEALING
            HealingTower heal = hit.GetComponent<HealingTower>();
            if (heal != null && !buffedHealing.Contains(heal))
            {
                ApplyHealBuff(heal);
                buffedHealing.Add(heal);
            }
        }
    }
    private void RemoveAllBuffs()
    {
        // Remove Ranged buffs
        foreach (var r in buffedRanged)
            if (r != null) RemoveRangedBuff(r);
        buffedRanged.Clear();

        // Remove Melee buffs
        foreach (var m in buffedMelee)
            if (m != null) RemoveMeleeBuff(m);
        buffedMelee.Clear();

        // Remove Healing buffs
        foreach (var h in buffedHealing)
            if (h != null) RemoveHealBuff(h);
        buffedHealing.Clear();
    }

    private bool ContainsCollider(Collider2D[] arr, MonoBehaviour tower)
    {
        if (tower == null) return false;

        Collider2D col = tower.GetComponentInChildren<Collider2D>();
        if (col == null) return false;

        foreach (var h in arr)
            if (h == col)
                return true;

        return false;
    }


    // ------- RANGED BUFFS -------

    private void ApplyRangedBuff(RangedTower ranged)
    {
        ranged.SetBPS(ranged.GetBPS() * buffMultiplier);
    }

    private void RemoveRangedBuff(RangedTower ranged)
    {
        ranged.SetBPS(ranged.GetBPS() / buffMultiplier);
    }

    // ------- MELEE BUFFS -------

    private void ApplyMeleeBuff(MeleeTower melee)
    {
        // Lower hitrate = faster attacks
        melee.SetHitRate(melee.GetHitRate() / buffMultiplier);
    }

    private void RemoveMeleeBuff(MeleeTower melee)
    {
        melee.SetHitRate(melee.GetHitRate() * buffMultiplier);

    }
    // ------- HEAL BUFFS -------

    private void ApplyHealBuff(HealingTower heal)
    {
        heal.SetBPS(heal.GetBPS() * buffMultiplier);
    }

    private void RemoveHealBuff(HealingTower heal)
    {
        heal.SetBPS(heal.GetBPS() / buffMultiplier);
    }
    private void OnDisable()
    {
        // Remove all buffs when destroyed
        foreach (var r in buffedRanged)
            if (r != null) RemoveRangedBuff(r);

        foreach (var m in buffedMelee)
            if (m != null) RemoveMeleeBuff(m);
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Handles.color = Color.green;
    //    Handles.DrawWireDisc(transform.position, transform.forward, buffRange);
    //}
}
