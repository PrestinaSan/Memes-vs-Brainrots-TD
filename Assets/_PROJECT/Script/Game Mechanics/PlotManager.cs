using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlotManager : MonoBehaviour
{
    public GameObject plotManager;
    public GameObject[] plots;
    [SerializeField] private Button cancelButton;
    //public bool towerSelected = false;
    private void Start()
    {
        plots = new GameObject[plotManager.transform.childCount];

        for (int i = 0; i < plots.Length; i++)
        {
            plots[i] = plotManager.transform.GetChild(i).gameObject;
        }
        DisablePlots();
    }
    public void EnablePlots()
    {
        cancelButton.gameObject.SetActive(true);
        for (int i = 0; i < plots.Length; i++)
        {
            PlotScript plot = plots[i].GetComponent<PlotScript>();
            plots[i].SetActive(true);
            if (plot.towerObj == null)
                plot.ResetColor();
                
        }
    }
    public void DisablePlots()
    {
        cancelButton.gameObject.SetActive(false);
        for (int i = 0; i < plots.Length; i++)
        {
            PlotScript plot = plots[i].GetComponent<PlotScript>();
            if (plot.towerObj != null)
            {
                plot.TurnInvisible();
                continue;
            }
            plots[i].SetActive(false);
        }
        BuildManager.instance.towerSelected = false;
    }
}
