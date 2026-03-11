using UnityEngine;

public class TankManager : MonoBehaviour
{
    [Header("Water Visuals")]
    public Transform waterMesh; 
    public float emptyY; 
    public float fullY;  

    [Header("Tank Settings")]
    public float maxWater = 10f; 
    public float currentWater = 0f;

    [Header("Reward")]
    public GameObject mistingSystem; 

    void Update()
    {
        UpdateWaterVisuals();
    }

    void OnValidate()
    {
        if (waterMesh == null) return;
        float fillPercentage = Mathf.Clamp01(currentWater / maxWater);
        float currentY = Mathf.Lerp(emptyY, fullY, fillPercentage);
        waterMesh.localPosition = new Vector3(waterMesh.localPosition.x, currentY, waterMesh.localPosition.z);
    }

    void UpdateWaterVisuals()
    {
        if (waterMesh == null) return;

        float fillPercentage = Mathf.Clamp01(currentWater / maxWater);
        float currentY = Mathf.Lerp(emptyY, fullY, fillPercentage);
        waterMesh.localPosition = new Vector3(waterMesh.localPosition.x, currentY, waterMesh.localPosition.z);

        // This only runs while the game is actually playing
        if (Application.isPlaying)
        {
            waterMesh.gameObject.SetActive(currentWater > 0.001f);
        }
    }

    public void ReceiveWater(float amount)
    {
        if (currentWater >= maxWater) return; 
        currentWater += amount;
        
        if (currentWater >= maxWater && mistingSystem != null)
        {
            mistingSystem.SetActive(true);
        }
    }
}