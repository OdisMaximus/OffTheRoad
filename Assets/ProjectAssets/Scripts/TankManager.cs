using UnityEngine;

public class TankManager : MonoBehaviour
{
    [Header("Water Visuals")]
    [Tooltip("Drag the WaterPivot here, NOT the cylinder mesh")]
    public Transform waterMesh; 
    public float emptyScaleY = 0f; 
    public float fullScaleY = 1f;

    [Header("Settings")]
    public float maxWater = 100f; 
    public float currentWater = 0f;
    
    [Header("Victory Feedback")]
    public GameObject mistingSystem; 
    public AudioSource winAudio; // Drag your audio source here

    private bool rewardActivated = false;

    void Start() {
        if (mistingSystem == null) Debug.LogError("TANK: Misting System is NOT assigned!");
    }

    void Update() {
        if (waterMesh) {
            // Calculate percentage and scale the Y axis smoothly
            float fillPct = Mathf.Clamp01(currentWater / maxWater);
            float curScaleY = Mathf.Lerp(emptyScaleY, fullScaleY, fillPct);
            
            waterMesh.localScale = new Vector3(waterMesh.localScale.x, curScaleY, waterMesh.localScale.z);
            waterMesh.gameObject.SetActive(currentWater > 0.001f);
        }

        // BRUTE FORCE KEY CHECK
        if (Input.GetKeyDown(KeyCode.V)) TriggerVictoryManually();
        //if (CAVE2.GetButtonDown(CAVE2.Button.Button7)) TriggerVictoryManually();
    }

    // Called by the ValveController script
    public void AddWaterFromValve(float amount)
    {
        if (currentWater < maxWater)
        {
            currentWater += amount;
            if (currentWater >= maxWater && !rewardActivated) ActivateVictory();
        }
    }

    void TriggerVictoryManually() {
        currentWater = maxWater;
        if (!rewardActivated) ActivateVictory();
    }

    void ActivateVictory() {
        rewardActivated = true;
        
        // 1. Play Audio
        if (winAudio != null) {
            winAudio.Play();
        }

        // 2. Turn on Misting Particles
        if (mistingSystem) {
            mistingSystem.SetActive(true);
            
            ParticleSystem[] pSystems = mistingSystem.GetComponentsInChildren<ParticleSystem>(true);
            foreach (var ps in pSystems) {
                ps.gameObject.SetActive(true);
                ps.Clear(); 
                ps.Play();
            }
        }
    }
}