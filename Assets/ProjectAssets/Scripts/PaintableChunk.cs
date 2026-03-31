using UnityEngine;

public class PaintableChunk : MonoBehaviour
{
    public float requiredPaintTime = 1.0f; 
    public Material reflectivePaintMaterial;
    public PavementManager manager; 
    
    private float currentPaintTime = 0f;
    private bool isPainted = false;
    private MeshRenderer meshRenderer;
    private Material instanceMaterial; 

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        
        if (meshRenderer != null)
        {
            // Create unique instance so we don't paint ALL tiles at once
            instanceMaterial = new Material(reflectivePaintMaterial);
            meshRenderer.material = instanceMaterial;
            
            // Start invisible
            SetMaterialAlpha(0f);
            meshRenderer.enabled = false; 
        }
    }

    public void ReceivePaint(float amount)
    {
        if (isPainted) return;

        if (currentPaintTime == 0f && meshRenderer != null)
        {
            meshRenderer.enabled = true;
        }

        currentPaintTime += amount;
        float progress = Mathf.Clamp01(currentPaintTime / requiredPaintTime);
        SetMaterialAlpha(progress);

        if (currentPaintTime >= requiredPaintTime)
        {
            ApplyPaint();
        }
    }

    void SetMaterialAlpha(float alpha)
    {
        if (instanceMaterial != null)
        {
            Color color = instanceMaterial.color;
            color.a = alpha;
            instanceMaterial.color = color;
        }
    }

    public void ApplyPaint()
    {
        isPainted = true;
        SetMaterialAlpha(1f); 
        if (meshRenderer != null) meshRenderer.enabled = true;
        
        if (manager != null) manager.ReportTilePainted(); 
    }
}