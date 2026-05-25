using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class RangedTower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private GameObject towerProjectile;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject sellUI;
    [SerializeField] private Button sellButton;
    [SerializeField] private Transform sprite;
    [SerializeField] private TowerStunHandler stunHandler;
    public PlotScript plot = null;


    [Header("Attributes")]
    [SerializeField] private float attackRange;
    [SerializeField] private float bps; //bullets per second
    [SerializeField] private float timeUntilFire;
    [SerializeField] private GameObject rangeIndicator;

    private void Start()
    {
        UpdateRangeIndicator();
        HideRange();
        stunHandler = GetComponent<TowerStunHandler>();
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position,attackRange, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0) {
            target = hits[0].transform;
        }
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position ) <= attackRange;
    }
    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x  - transform.position.x) * Mathf.Rad2Deg + 180f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f,0f,angle));
        sprite.rotation = targetRotation;

    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(towerProjectile, sprite.position, Quaternion.identity);
        BulletScript bulletScript = bulletObj.GetComponent<BulletScript>();
        bulletScript.SetTarget(target);
        bulletObj.transform.rotation = sprite.rotation;

    }
    void Update()
    {
        if (towerProjectile == null) return;
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
            sprite.rotation = Quaternion.Euler(new Vector3(0f,0f,0f));
            target = null;
        }
        else
        {
            RotateTowardsTarget();
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1 / bps)
            {
                Shoot();
                timeUntilFire = 0;
            }
        }

    }
    //private void OnDrawGizmosSelected()
    //{
    //    Handles.color = Color.cyan;
    //    Handles.DrawWireDisc(transform.position, transform.forward, attackRange);
    //}
    public void ShowRange()
    {
        if (rangeIndicator != null)
            rangeIndicator.SetActive(true);
    }

    public void HideRange()
    {
        if (rangeIndicator != null)
            rangeIndicator.SetActive(false);
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
    public void SetBPS(float newBPS)
    {
        bps = newBPS;
    }
    public float GetBPS()
    {
        return bps;
    }
    public GameObject GetProjectile()
    {
        return towerProjectile;
    }
    private void UpdateRangeIndicator()
    {
        if (rangeIndicator == null) return;

        SpriteRenderer sr = rangeIndicator.GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null) return;

        float spriteRadius = sr.sprite.bounds.extents.x; // world units per pixel?
        float scale = attackRange / spriteRadius;

        rangeIndicator.transform.localScale = new Vector3(scale, scale, 1f);
    }
    public void ResetAttackTimer()
    {
        timeUntilFire = 0f;
    }
}
