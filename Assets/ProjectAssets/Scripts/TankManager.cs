using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankManager : MonoBehaviour
{
    [Header("Water Visuals")]
    public Transform waterMesh; 
    public float emptyZ; 
    public float fullZ;  

    [Header("Settings")]
    public float maxWater = 10f; 
    public float currentWater = 0f;
    public float fillPerParticle = 0.05f; 
    public GameObject mistingSystem; 

    private bool rewardActivated = false;

    void Start() {
        GetComponent<Rigidbody>().isKinematic = true; 
        if (mistingSystem == null) Debug.LogError("TANK: Misting System is NOT assigned!");
    }

    void Update() {
        if (waterMesh) {
            float fillPct = Mathf.Clamp01(currentWater / maxWater);
            float curZ = Mathf.Lerp(emptyZ, fullZ, fillPct);
            waterMesh.localPosition = new Vector3(waterMesh.localPosition.x, waterMesh.localPosition.y, curZ);
            waterMesh.gameObject.SetActive(currentWater > 0.001f);
        }

        // BRUTE FORCE KEY CHECK
        if (Input.GetKeyDown(KeyCode.V)) {
            Debug.Log("TANK: V Key Pressed");
            TriggerVictoryManually();
        }

        // CAVE2 BUTTON CHECK (Checking both Button2 just in case)
        if (CAVE2.GetButtonDown(CAVE2.Button.Button7)) {
            Debug.Log("TANK: CAVE2 Button Pressed");
            TriggerVictoryManually();
        }
    }

    void TriggerVictoryManually() {
        currentWater = maxWater;
        if (!rewardActivated) {
            Debug.Log("TANK: Activating Victory...");
            ActivateVictory();
        }
    }

    void OnParticleCollision(GameObject other) {
        if (currentWater < maxWater) {
            currentWater += fillPerParticle;
            if (currentWater >= maxWater && !rewardActivated) ActivateVictory();
        }
    }

    void ActivateVictory() {
        rewardActivated = true;
        if (mistingSystem) {
            mistingSystem.SetActive(true);
            
            // Get all particles including hidden ones
            ParticleSystem[] pSystems = mistingSystem.GetComponentsInChildren<ParticleSystem>(true);
            Debug.Log("TANK: Found " + pSystems.Length + " particle systems to play.");

            foreach (var ps in pSystems) {
                ps.gameObject.SetActive(true);
                ps.Clear(); // Clear any old stuck particles
                ps.Play();
            }
        } else {
            Debug.LogError("TANK: Cannot activate victory - Misting System missing!");
        }
    }
}