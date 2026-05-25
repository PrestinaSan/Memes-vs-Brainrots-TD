using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float damage = 2f;
    [SerializeField] private float stunDuration = 1f;

    private Transform target;
    private Vector2 moveDir;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        moveDir = ((Vector2)(target.position - transform.position)).normalized;
        rb.linearVelocity = moveDir * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("MeleeTower") && !collision.CompareTag("RangedTower"))
            return;

        HealthManagerScript hp = collision.GetComponent<HealthManagerScript>();
        if (hp != null)
            hp.TakeDamage(damage);

        TowerStunHandler stun = collision.GetComponent<TowerStunHandler>();
        if (stun != null)
            stun.Stun(stunDuration);

        Destroy(gameObject);
    }
}
