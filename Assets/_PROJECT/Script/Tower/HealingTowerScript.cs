using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HealingTower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private GameObject towerProjectile;
    [SerializeField] private LayerMask towerMask;
    [SerializeField] private Transform sprite;
    [SerializeField] private TowerStunHandler stunHandler;
    public PlotScript plot = null;
    [Header("Attributes")]
    [SerializeField] private float attackRange;
    [SerializeField] private float bps; //bullets per second
    [SerializeField] private float timeUntilFire;
    //[SerializeField] private float rotationSpeed = 500f;

    void Start()
    {
        stunHandler = GetComponent<TowerStunHandler>();

    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position,attackRange,Vector2.zero,0f,towerMask);

        foreach (var hit in hits)
        {
            HealthManagerScript health = hit.transform.GetComponent<HealthManagerScript>();

            if (health != null && !health.IsHealthFull())
            {
                target = hit.transform;
                return;
            }
        }
        target = null;
    }


    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= attackRange;
    }
    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg + 180f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        //transform.rotation = Quaternion.RotateTowards(transform.rotation,target.rotation,rotationSpeed * Time.deltaTime);
        sprite.rotation = targetRotation;

    }

    private void HealTarget()
    {
        GameObject bulletObj = Instantiate(towerProjectile, sprite.position, Quaternion.identity);
        BulletScript bulletScript = bulletObj.GetComponent<BulletScript>();
        bulletScript.SetTarget(target);
        bulletObj.transform.rotation = sprite.rotation;

    }
    void Update()
    {
        if (stunHandler != null && stunHandler.IsStunned())
            return;
        if (target == null)
        {
            sprite.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            FindTarget();
            return;
        }
        if (!CheckTargetIsInRange())
        {
            sprite.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            target = null;
        }
        else
        {
            HealthManagerScript targetHealth = target.GetComponent<HealthManagerScript>();
            if (targetHealth.IsHealthFull())
            {
                sprite.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                target = null;
                return;
            }
            RotateTowardsTarget();
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1 / bps)
            {
                HealTarget();
                timeUntilFire = 0;
            }
        }

    }

    //private void OnDrawGizmosSelected()
    //{
    //    Handles.color = Color.green;
    //    Handles.DrawWireDisc(transform.position, transform.forward, attackRange);
    //}
    public float GetBPS()
    {
        return bps;
    }
    public GameObject GetProjectile()
    {
        return towerProjectile;
    }
    public void SetBPS(float newBPS)
    {
        bps = newBPS;
    }
    public void ResetHealTimer()
    {
        timeUntilFire = 0f;
    }

}
