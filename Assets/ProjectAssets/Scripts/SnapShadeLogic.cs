using UnityEngine;

public class SnapShadeLogic : MonoBehaviour
{
    public float snapThreshold = 1.0f; // Distance to trigger snap

    void OnTriggerStay(Collider other)
    {
        // If we get close to a Ghost Shade on the GhostShade layer
        if (other.CompareTag("ShadeTag") && other.gameObject.layer != 0)
        {
            float dist = Vector3.Distance(transform.position, other.transform.position);
            
            if (dist < snapThreshold)
            {
                // 1. Reveal the Ghost by moving it to Default layer
                other.gameObject.layer = 0; 

                // 2. Destroy this grabbable version (prevents size mismatch issues) [cite: 2026-03-04]
                Destroy(this.gameObject);
                
                Debug.Log("Snapped! Ghost revealed.");
            }
        }
    }
}