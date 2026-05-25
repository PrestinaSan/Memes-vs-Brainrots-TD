using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 15.0f;
    [SerializeField] private Transform target;
    [SerializeField] private float damage;
    [SerializeField] private bool isPiercing = false;
    [SerializeField] private float piercingLifetime = 3f;
    [SerializeField] private string type;

    private bool hasHitFirstTarget = false;
    private Vector2 moveDirection = Vector2.zero;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    void FixedUpdate()
    {
        if (!isPiercing || !hasHitFirstTarget)
        {
            if (!target)
            {
                Destroy(gameObject);
                return;
            }

            moveDirection = (target.position - transform.position).normalized;
        }
        if (type == "healing")
        {
            HealthManagerScript health = target.GetComponent<HealthManagerScript>();

            if (health == null || health.IsHealthFull())
            {
                Destroy(gameObject);
                return;
            }
        }
        rb.linearVelocity = moveDirection * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (type == "damaging")
        {
            if (collision.CompareTag("Enemy") || (collision.CompareTag("BossEnemy")))
            {
                HealthManagerScript enemyHealth = collision.GetComponent<HealthManagerScript>();
                if (enemyHealth != null)
                    enemyHealth.TakeDamage(damage);

                HandlePiercing();
            }
            return;
        }

        if (type == "healing")
        {
            if (collision.CompareTag("MeleeTower") || collision.CompareTag("RangedTower"))
            {

                HealthManagerScript towerHealth = collision.GetComponent<HealthManagerScript>();
                if (towerHealth == null)
                    return;

                if (towerHealth.IsHealthFull())
                    return;

                towerHealth.Heal(damage);

                HandlePiercing();
            }
        }
    }

    private void HandlePiercing()
    {
        if (isPiercing)
        {
            if (!hasHitFirstTarget)
            {
                hasHitFirstTarget = true;
                moveDirection = rb.linearVelocity.normalized;
                Destroy(gameObject, piercingLifetime);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float GetDamage()
    {
        return damage;
    }
}
