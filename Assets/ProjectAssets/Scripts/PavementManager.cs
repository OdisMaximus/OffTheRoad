using UnityEngine;

public class PavementManager : MonoBehaviour
{
    [Header("Win Condition")]
    public int tilesNeededToWin = 6; //DEFAULT: 6
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

    private ParticleSystem[] heatingParticles;
    private ParticleSystem[] coolingParticles;

    [Header("Tool Swap")]
    public GameObject wandObject;

    [Header("Audio")]
    public AudioClip winSound;

    [Header("Game Progress Manager Here")]
    public GameObject GameManagerObject;
    public GameProgressManager GameProgressManagerScript;

    void Awake()
    {
        GameProgressManagerScript = GameManagerObject.GetComponent<GameProgressManager>();

        if (heatingParticlesParent != null)
            heatingParticles = heatingParticlesParent.GetComponentsInChildren<ParticleSystem>();
        
        if (coolingParticlesParent != null)
            coolingParticles = coolingParticlesParent.GetComponentsInChildren<ParticleSystem>();


    }

    public void ReportTilePainted()
    {
        tilesPainted++;
        if (!pavementCompleteTriggered && tilesPainted >= tilesNeededToWin)
        {
            pavementCompleteTriggered = true;
            TriggerCityUpgrade();

            //INTERACT OTHER SCRIPT
            GameProgressManagerScript.UpdateGameProgressScore();


        }
    }

    public void TriggerCityUpgrade()
    {
        // 1. Play win sound
        if (winSound != null)
            AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position);

        // 2. Auto-paint and swap material
        if (SidewalkFolder != null)
        {
            PaintableChunk[] allChunks = SidewalkFolder.GetComponentsInChildren<PaintableChunk>();
            foreach (PaintableChunk chunk in allChunks) 
            { 
                chunk.ApplyPaint(); 
                MeshRenderer renderer = chunk.GetComponent<MeshRenderer>();
                if (renderer != null && solidPaintMaterial != null)
                    renderer.material = solidPaintMaterial;
            }
        }

        // 3. Particle swap
        if (heatingParticles != null)
            foreach (ParticleSystem heat in heatingParticles) { if (heat != null) heat.Stop(); }

        if (coolingParticles != null)
            foreach (ParticleSystem cool in coolingParticles) { if (cool != null) cool.Play(); }

        // 4. Disable roadblocks
        if (pavementRoadBlocks != null)
            foreach (GameObject block in pavementRoadBlocks) { if (block != null) block.SetActive(false); }

        // 5. Tool swap
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
        if (Input.GetKeyDown(KeyCode.U)) TriggerCityUpgrade();
    }

    
}