using UnityEngine;

public class DPManager : MonoBehaviour
{

    public static DPManager instance;
    public int DP; // deployment points
    [SerializeField] private int initialDP = 20;

    private float timer = 0;
    private int maxDP =  300;


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        DP = initialDP;
    }
    private void FixedUpdate()
    {
        if (EnemySpawner.instance.levelStarted == false) return;
        if (timer >= 0.5)
        {
            GainDP(1);
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
    public void GainDP(int amount)
    {
        if (DP < maxDP)
        {
            DP += amount;
        }
        else if (DP > maxDP)
        {
            DP = maxDP;
        }
    }
    public bool SpendDP(int amount)
    {
        if (amount <= DP)
        {
            DP -= amount;
            return true;
        }

        else
        {
            Debug.Log("broke ahh");
            return false;
        }
    }
}
