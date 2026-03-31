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

    [Header("Shadow Fix (Interaction 2)")]
    public Material solidPaintMaterial; 

    [Header("Roadblocks to Remove")]
    public GameObject[] pavementRoadBlocks;

    [Header("Particle Swap System")]
    [Tooltip("Drag the PARENT GameObject holding all heating particles here")]
    public GameObject heatingParticlesParent; 
    [Tooltip("Drag the PARENT GameObject holding all cooling particles here")]
    public GameObject coolingParticlesParent; 

    // Hidden arrays that the script will populate automatically
    private ParticleSystem[] heatingParticles;
    private ParticleSystem[] coolingParticles;

    [Header("Tool Swap")]
    public GameObject wandObject;

    void Awake()
    {
        // Automatically find every particle system inside the assigned parent folders
        if (heatingParticlesParent != null)
        {
            heatingParticles = heatingParticlesParent.GetComponentsInChildren<ParticleSystem>();
        }
        
        if (coolingParticlesParent != null)
        {
            coolingParticles = coolingParticlesParent.GetComponentsInChildren<ParticleSystem>();
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

    public void TriggerCityUpgrade()
    {
        // 1. Auto-paint and SWAP MATERIAL
        if (SidewalkFolder != null)
        {
            PaintableChunk[] allChunks = SidewalkFolder.GetComponentsInChildren<PaintableChunk>();
            foreach (PaintableChunk chunk in allChunks) 
            { 
                chunk.ApplyPaint(); 

                MeshRenderer renderer = chunk.GetComponent<MeshRenderer>();
                if (renderer != null && solidPaintMaterial != null)
                {
                    renderer.material = solidPaintMaterial;
                }
            }
        }

        // 2. PARTICLE SWAP (Stop Heat, Start Cooling)
        if (heatingParticles != null)
        {
            foreach (ParticleSystem heat in heatingParticles) { if (heat != null) heat.Stop(); }
        }

        if (coolingParticles != null)
        {
            foreach (ParticleSystem cool in coolingParticles) { if (cool != null) cool.Play(); }
        }

        // 3. Disable Roadblocks
        if (pavementRoadBlocks != null)
        {
            foreach (GameObject block in pavementRoadBlocks) { if (block != null) block.SetActive(false); }
        }

        // 4. Tool Swap
        if (wandObject != null)
        {
            PaintBrush pb = wandObject.GetComponent<PaintBrush>();
            GreeneryGun gg = wandObject.GetComponent<GreeneryGun>();
            if (pb != null) pb.FinalizeWinState(); 
            if (gg != null) gg.enabled = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) TriggerCityUpgrade();
    }
}