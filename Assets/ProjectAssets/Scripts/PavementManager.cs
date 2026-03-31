using UnityEngine;

public class PavementManager : MonoBehaviour
{
    [Header("Win Condition")]
    public int tilesNeededToWin = 10;
    private int tilesPainted = 0;
    private bool pavementCompleteTriggered = false;

    [Header("The 'After' State")]
    public Material reflectivePaintMaterial;
    public GameObject SidewalkFolder;

    [Header("Pavement Roadblocks / Congestion")]
    [Tooltip("Drag the red 'CONGESTION' text objects or roadblocks here to make them disappear on win.")]
    public GameObject[] pavementRoadBlocks;

    [Header("Particles & Misting")]
    public ParticleSystem[] coolingParticles; 

    [Header("Tool Swap")]
    public GameObject wandObject;

    public void ReportTilePainted()
    {
        tilesPainted++;

        if (!pavementCompleteTriggered && tilesPainted >= tilesNeededToWin)
        {
            pavementCompleteTriggered = true;
            TriggerCityUpgrade();
        }
    }

    public void TriggerCityUpgrade()
    {
        // 1. Auto-paint all remaining tiles in the folder
        if (SidewalkFolder != null)
        {
            PaintableChunk[] allChunks = SidewalkFolder.GetComponentsInChildren<PaintableChunk>();
            foreach (PaintableChunk chunk in allChunks)
            {
                chunk.ApplyPaint(); 
            }
        }

        // 2. Trigger Cooling Particles
        if (coolingParticles != null)
        {
            foreach (ParticleSystem ps in coolingParticles)
            {
                if (ps != null)
                {
                    ps.Play();
                    Debug.Log("Cooling System Activated: " + ps.gameObject.name);
                }
            }
        }

        // 3. Disable Roadblocks and Congestion
        // Since we removed TrafficManager, you must drag the objects you want 
        // to disappear into the 'Pavement Road Blocks' list in the Inspector.
        if (pavementRoadBlocks != null)
        {
            foreach (GameObject block in pavementRoadBlocks)
            {
                if (block != null) 
                {
                    block.SetActive(false);
                    Debug.Log("Roadblock/Congestion Removed: " + block.name);
                }
            }
        }

        // 4. Clean Tool Swap
        if (wandObject != null)
        {
            PaintBrush pb = wandObject.GetComponent<PaintBrush>();
            GreeneryGun gg = wandObject.GetComponent<GreeneryGun>();
            
            if (pb != null) pb.FinalizeWinState(); 
            if (gg != null) gg.enabled = true;
            
            Debug.Log("Interaction Complete: Brush Hidden, Particles Playing.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) TriggerCityUpgrade();
    }
}