using UnityEngine;

public class PavementManager : MonoBehaviour
{
    [Header("Win Condition")]
    public int tilesNeededToWin = 10; 
    private int tilesPainted = 0;

    [Header("The 'After' State")]
    public Material reflectivePaintMaterial;
    public GameObject trafficCongestion; //
    public GameObject SidewalkFolder;

    // EMERGENCY SAFETY BUTTON FUNCTION [cite: 2026-03-04]
    public void RevealAllGhostShades()
    {
        Debug.Log("SAFETY TRIGGER: Forcing all ghosts to Default layer.");
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("ShadeTag");
        foreach (GameObject ghost in ghosts)
        {
            // Force them to the visible 'Default' layer (0)
            ghost.layer = 0; 
        }
    }

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
        // 1. Remove Traffic (Hard Disable)
        if (trafficCongestion != null) 
        {
            trafficCongestion.SetActive(false); 
            Debug.Log("Traffic Removed!");
        }

        // 2. Reveal all Ghost Shades automatically on win [cite: 2026-03-04]
        RevealAllGhostShades();

        // 3. Paint the sidewalks
        if (SidewalkFolder != null)
        {
            MeshRenderer[] allRenderers = SidewalkFolder.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer tr in allRenderers)
            {
                tr.enabled = true; 
                tr.material = reflectivePaintMaterial;
            }
        }
    }

    // Optional: Press 'K' in the CAVE2 to trigger safety reveal [cite: 2026-03-04]
    void Update() {
        if (Input.GetKeyDown(KeyCode.B)) RevealAllGhostShades();
    }
}