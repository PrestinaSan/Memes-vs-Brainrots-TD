using System;
using UnityEngine;

public class HealthManagerScript : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private int DPValue = 2;
    
    private bool isDestroyed = false;
    private FloatingHealthBar _floatingHealthBar;

    private void Awake()
    {
        GameObject go = Instantiate(healthBarPrefab, transform);
        go.transform.localPosition = Vector2.zero;
        _floatingHealthBar = go.GetComponent<FloatingHealthBar>();
    }
    void Start()
    {
        health = maxHealth;
        _floatingHealthBar.UpdateHealthBar(health, maxHealth);
    }


    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        _floatingHealthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0 && isDestroyed == false)
        {
            isDestroyed = true;
            if (gameObject.CompareTag("Enemy") || gameObject.CompareTag("BossEnemy"))
            {
                EnemySpawner.OnEnemyDestroy.Invoke();
                DPManager.instance.GainDP(DPValue);
                Destroy(gameObject);
            }
            else if (gameObject.CompareTag("MeleeTower"))
            {
                MeleeTower melee = gameObject.GetComponent<MeleeTower>();
                PlotScript plot = melee.plot;
                plot.TowerDies();
            }
            else if (gameObject.CompareTag("RangedTower"))
            {
                RangedTower ranged = gameObject.GetComponent<RangedTower>();
                PlotScript plot = ranged.plot;
                plot.TowerDies();
            }
            else {

                isDestroyed = true;
            }//deployment slots taken reduction
            
        }
    }
    public void Heal(float heal)
    {
        health += heal;
        if (health > maxHealth) health = maxHealth;
        _floatingHealthBar.UpdateHealthBar(health, maxHealth);
    }
    public Boolean IsHealthFull()
    {
        if (health == maxHealth) return true;
        else return false;
    }
    public float GetMaxHP()
    {
        return maxHealth;
    }
    public bool GetDestroyedState()
    {
        return isDestroyed;
    }
}
