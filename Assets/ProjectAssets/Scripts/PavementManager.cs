using UnityEngine;

public class PavementManager : MonoBehaviour
{
    [Header("Win Condition")]
    public int tilesNeededToWin = 10; // Count how many tiles you placed and type it here
    private int tilesPainted = 0;

    [Header("The 'After' State")]
    public GameObject[] otherSidewalks; // Drag the other 3 sidewalk objects here
    public Material reflectivePaintMaterial;
    public GameObject trafficCongestion; // Drag your "group_of_congestion" here
    public GameObject extraPedestrians;  // Drag your new people group here

    public void ReportTilePainted()
    {
        tilesPainted++;
        if (tilesPainted >= tilesNeededToWin)
        {
            TriggerCityUpgrade();
        }
    }

    void TriggerCityUpgrade()
    {
        // 1. Swap the remaining sidewalks
        foreach (GameObject sidewalk in otherSidewalks)
        {
            if (sidewalk != null)
            {
                sidewalk.GetComponent<Renderer>().material = reflectivePaintMaterial;
            }
        }

        // 2. Remove cars, add people
        if (trafficCongestion) trafficCongestion.SetActive(false);
        if (extraPedestrians) extraPedestrians.SetActive(true);
    }
}