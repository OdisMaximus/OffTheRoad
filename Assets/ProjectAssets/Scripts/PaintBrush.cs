using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    public float brushRange = 10f;
    public LayerMask paintableLayer; // To ensure we only paint the chunks

    void Update()
    {
        // For testing on your laptop trackpad, we use Fire1 (Left Click). 
        // In CAVE2, you'll map this to the wand trigger.
        if (Input.GetButton("Fire1")) 
        {
            CastPaintRay();
        }
    }

    void CastPaintRay()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Cast ray. If it hits an object on the Paintable layer...
        if (Physics.Raycast(ray, out hit, brushRange, paintableLayer))
        {
            PaintableChunk chunk = hit.collider.GetComponent<PaintableChunk>();
            if (chunk != null)
            {
                // Send the time passed since last frame to track swipe duration
                chunk.ReceivePaint(Time.deltaTime); 
            }
        }
    }
}