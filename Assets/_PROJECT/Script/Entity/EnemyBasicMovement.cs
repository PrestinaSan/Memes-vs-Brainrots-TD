
using UnityEngine;

public class Movement : MonoBehaviour
{


    [Header("References")]
    private KeypointManager keypoint_manager;
    private bool _reachedTarget = false;
    private int _currentKeypointIndex = 0;
    private Transform _currentKeyPoint;
    public MeleeTower blockingTower = null;
    [SerializeField] private Transform sprite;

    [Header("Attributes")]
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float hitrate = 2f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private int damageToPlayer = 1;
    private float timer = 0f;
    public bool blocked = false;

    private void Start()
    {
        keypoint_manager = FindAnyObjectByType<KeypointManager>();
        _reachedTarget = false;
        _currentKeypointIndex = 0;
        _currentKeyPoint = keypoint_manager.GetNextWaypoint(_currentKeypointIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (keypoint_manager == null)
            return;

        if (blocked)
        {
            if (blockingTower != null)
            {
                HealthManagerScript MeleeTowerHealthManager = blockingTower.GetComponent<HealthManagerScript>();
                if (timer < hitrate)
                    timer += Time.deltaTime;
                else
                {
                    MeleeTowerHealthManager.TakeDamage(damage);
                    timer = 0f;
                }
            }
            return;
        }

        if (_currentKeyPoint == null)
            return;

        if (_reachedTarget)
            return;
        if (_currentKeyPoint.transform.position.x > sprite.position.x)
        {
            sprite.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
        else if (_currentKeyPoint.transform.position.x < sprite.position.x)
        {
            sprite.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }
        transform.position = Vector2.MoveTowards(transform.position, _currentKeyPoint.position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, _currentKeyPoint.position) > 0.01f)
            return;

        _reachedTarget = true;
        _currentKeypointIndex++;
        // not reached player base
        if (!keypoint_manager.CheckEnd(_currentKeypointIndex))
        {
            _currentKeyPoint = keypoint_manager.GetNextWaypoint(_currentKeypointIndex);
            _reachedTarget = false;
        }
        else // reached player base
        {
            
            EnemySpawner.OnEnemyDestroy.Invoke();
            LevelStatsManager.Instance.UpdateHealthCount(-damageToPlayer);
            Destroy(gameObject);
        }
    }
}
