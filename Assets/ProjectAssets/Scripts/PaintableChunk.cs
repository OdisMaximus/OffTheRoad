using UnityEngine;

public class PaintableChunk : MonoBehaviour
{
    public float requiredPaintTime = 1.0f; 
    public Material reflectivePaintMaterial;
    public PavementManager manager; 
    
    private float currentPaintTime = 0f;
    private bool isPainted = false;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        // Automatically hide the quad when the game starts
        if(meshRenderer != null) meshRenderer.enabled = false; 
    }

    public void ReceivePaint(float amount)
    {
        if (isPainted) return;

        currentPaintTime += amount;

        if (currentPaintTime >= requiredPaintTime)
        {
            ApplyPaint();
        }
    }

    void ApplyPaint()
    {
        isPainted = true;
        if(meshRenderer != null)
        {
            meshRenderer.material = reflectivePaintMaterial;
            meshRenderer.enabled = true; 
        }
        
        if (manager != null) manager.ReportTilePainted(); 
    }
}