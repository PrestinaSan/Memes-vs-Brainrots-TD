using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.SceneManagement;
public class LevelStatsManager : MonoBehaviour
{
    public static LevelStatsManager Instance;


    [SerializeField] private TextMeshProUGUI dpUI;
    [SerializeField] private TextMeshProUGUI enemyCount;
    [SerializeField] private TextMeshProUGUI healthCount;
    [SerializeField] private TextMeshProUGUI deploymentLimit;
    public int enemyNumber = 0;
    public int healthNumber = 10;
    public int maxHealthNumber = 10;
    public int TotalEnemies = 70;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        healthNumber = 10;
        maxHealthNumber = 10;
        dpUI.text = "DP:" + DPManager.instance.DP.ToString();
        enemyCount.text = "Enemies: " + enemyNumber.ToString() + "/" + TotalEnemies.ToString();
        healthCount.text = "Health: " + healthNumber.ToString() +  "/" + maxHealthNumber.ToString();
    }

    private void OnGUI()
    {
        dpUI.text = "DP:" + DPManager.instance.DP.ToString();
    }

    public void SetTotalEnemies(int total)
    {
        TotalEnemies = total;
        enemyCount.text = "Enemies: " + enemyNumber.ToString() + "/" + TotalEnemies.ToString();
    }

    public void IncreaseEnemyCount()
    {
        enemyNumber++;
        enemyCount.text = "Enemies: " + enemyNumber.ToString() + "/" + TotalEnemies.ToString();
    }
    public void UpdateHealthCount(int amount)
    {
        healthNumber += amount;
        healthCount.text = "Health: " + healthNumber.ToString() + "/" + maxHealthNumber.ToString();
        if (healthNumber <= 0)
        {
            Debug.Log("lost");
            SceneManager.LoadScene("Main Menu");
        }
    }
    public void UpdateDeploymentLimit(int deployed, int limit)
    {
        deploymentLimit.text = deployed.ToString() + "/" + limit.ToString();
    }

    public void OnQuitClick()
    {
        healthNumber = 0;
        UpdateHealthCount(0);
    }
    public int GetStars()
    {
        if (healthNumber == maxHealthNumber) return 3;
        else if (healthNumber >= (maxHealthNumber * 2 / 3)) return 2;
        else return 1;
    }
}
