using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MeleeTower : MonoBehaviour
{
    private float attackTimer = 0;
    private List<GameObject> enemiesInside = new List<GameObject>();
    [SerializeField] private int maxBlockCount = 3;
    [SerializeField] private float hitRate = 2f;
    [SerializeField] private float damage;
    [SerializeField] private GameObject sellUI;
    [SerializeField] private Button sellButton;
    [SerializeField] private Transform sprite;
    [SerializeField] private TowerStunHandler stunHandler;
    public PlotScript plot = null;


    void Start()
    {
        stunHandler = GetComponent<TowerStunHandler>();

    }

    private void Update()
    {
        if (enemiesInside.Count == 0)
        {
            attackTimer = 0;
            return;
        }
        if (stunHandler != null && stunHandler.IsStunned())
            return; 

        GameObject currentTarget = enemiesInside[0];

        if (currentTarget == null)
        {
            enemiesInside.RemoveAt(0);
            return;
        }
        if (currentTarget.transform.position.x > sprite.position.x)
        {
            sprite.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }
        else if (currentTarget.transform.position.x < sprite.position.x)
        {
            sprite.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
        Attack(currentTarget);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy"))
        {
            Movement enemyMovement = collision.GetComponent<Movement>();
            if (enemiesInside.Count < maxBlockCount)
            {
                enemiesInside.Add(collision.gameObject);

                if (enemyMovement != null)
                {
                    enemyMovement.blocked = true;
                    enemyMovement.blockingTower = this;
                }
            }
            else
            {
                if (enemyMovement != null)
                {
                    enemyMovement.blocked = false;
                    enemyMovement.blockingTower = null;
                }
            }
        }
        else if (collision.CompareTag("BossEnemy")) 
        {
            BossMovement enemyMovement = collision.GetComponent<BossMovement>();
            if (enemiesInside.Count < maxBlockCount)
            {
                enemiesInside.Add(collision.gameObject);

                if (enemyMovement != null)
                {
                    enemyMovement.blocked = true;
                    enemyMovement.blockingTower = this;
                }
            }
            else
            {
                if (enemyMovement != null)
                {
                    enemyMovement.blocked = false;
                    enemyMovement.blockingTower = null;
                }
            }
        }

            


       
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Movement enemyMovement = collision.GetComponent<Movement>();
            if (enemiesInside.Contains(collision.gameObject))
                enemiesInside.Remove(collision.gameObject);

            if (enemyMovement != null)
            {
                enemyMovement.blocked = false;
                enemyMovement.blockingTower = null;
            }
        }
        else if (collision.CompareTag("BossEnemy"))
        {

            BossMovement enemyMovement = collision.GetComponent<BossMovement>();
            if (enemiesInside.Contains(collision.gameObject))
                enemiesInside.Remove(collision.gameObject);

            if (enemyMovement != null)
            {
                enemyMovement.blocked = false;
                enemyMovement.blockingTower = null;
            }
        }

    }
    private void Attack(GameObject currentTarget)
    {
        if (attackTimer < hitRate)
        {
            attackTimer += Time.deltaTime;
            return;
        }
        AnimationHandler animator = gameObject.GetComponentInChildren<AnimationHandler>();
        animator.OnAttackAnimationStartFreddy();
        HealthManagerScript EnemyHealthManager = currentTarget.GetComponent<HealthManagerScript>();
        if (EnemyHealthManager != null)
            EnemyHealthManager.TakeDamage(damage);
        attackTimer = 0f;
    }
    public void OpenSellUI()
    {
        sellUI.SetActive(true);
    }
    public void CloseSellUI()
    {
        sellUI.SetActive(false);
    }
    public void OnSellButtonPressed()
    {
        plot.SellTower();
        plot.gameObject.SetActive(false);
    }
    public float GetDamage()
    {
        return damage;
    }
    public void SetDamage(float _damage)
    {
        damage = _damage;
    }
    public float GetHitRate()
    {
        return hitRate;
    }
    public void SetHitRate(float _hitRate)
    {
        hitRate = _hitRate;
    }
    public void ResetAttackTimer()
    {
        attackTimer = 0f;
    }
}
