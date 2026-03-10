using UnityEngine;

public class PavementManager : MonoBehaviour
{
    [Header("Win Condition")]
    public int tilesNeededToWin = 10;
    private int tilesPainted = 0;
    private bool pavementCompleteTriggered = false;

    [Header("The 'After' State")]
    public Material reflectivePaintMaterial;
    public GameObject trafficCongestion;
    public GameObject SidewalkFolder;

    [Header("Pavement Roadblocks")]
    public GameObject[] pavementRoadBlocks;

    [Header("Traffic System")]
    public TrafficManager trafficManager;
    public int congestionReductionOnComplete = 2;

    public void RevealAllGhostShades()
    {
        Debug.Log("SAFETY TRIGGER: Forcing all ghosts to Default layer.");
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("ShadeTag");
        foreach (GameObject ghost in ghosts)
        {
            ghost.layer = 0;
        }
    }

    public void ReportTilePainted()
    {
        tilesPainted++;

        if (!pavementCompleteTriggered && tilesPainted >= tilesNeededToWin)
        {
            pavementCompleteTriggered = true;
            TriggerCityUpgrade();
        }
    }

    void TriggerCityUpgrade()
    {
        if (trafficCongestion != null)
        {
            trafficCongestion.SetActive(false);
            Debug.Log("Traffic Removed!");
        }

        if (trafficManager != null)
        {
            trafficManager.ReduceCongestion(congestionReductionOnComplete);
        }

        if (SidewalkFolder != null)
        {
            MeshRenderer[] allRenderers = SidewalkFolder.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer tr in allRenderers)
            {
                tr.enabled = true;
                tr.material = reflectivePaintMaterial;
            }
        }

        if (pavementRoadBlocks != null)
        {
            foreach (GameObject block in pavementRoadBlocks)
            {
                if (block != null)
                {
                    block.SetActive(false);
                }
            }
        }

        // Keep or remove this depending on whether pavement should reveal shades
        // RevealAllGhostShades();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            RevealAllGhostShades();
    }
}