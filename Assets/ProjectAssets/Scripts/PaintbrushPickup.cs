using UnityEngine;

public class PaintbrushPickup : MonoBehaviour
{
    public GameObject wandObject;
    private bool collected = false;

    void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        GreeneryGun gun = other.GetComponentInChildren<GreeneryGun>();
        if (gun == null)
            gun = other.GetComponentInParent<GreeneryGun>();

        if (gun != null || other.CompareTag("Player"))
        {
            collected = true;
            
            if (wandObject != null)
            {
                wandObject.GetComponent<PaintBrush>().enabled = true;
                Debug.Log("Paintbrush picked up!");
            }

            Destroy(gameObject);
        }
    }
}