using UnityEngine;

public class StrawberryElephantAbility : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform sprite;
    [SerializeField] private LayerMask towerMask;
    [SerializeField] private GameObject enemyBulletPrefab;

    [Header("Attributes")]
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float shotsPerSecond = 1f;

    private float attackTimer = 0f;

    private void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer < 1f / shotsPerSecond) return;

        attackTimer = 0f;
        ShootAllTowersInRange();
    }

    private void ShootAllTowersInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, towerMask);

        if (hits.Length == 0) return;

        foreach (Collider2D hit in hits)
        {
            Transform tower = hit.transform;

            FireBullet(tower);
        }
    }

    private void FireBullet(Transform tower)
    {
        GameObject bulletObj = Instantiate(enemyBulletPrefab, sprite.position, Quaternion.identity);
        EnemyBullet bullet = bulletObj.GetComponent<EnemyBullet>();

        if (bullet != null)
            bullet.SetTarget(tower);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
