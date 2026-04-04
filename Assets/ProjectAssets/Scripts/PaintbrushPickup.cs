using UnityEngine;

public class PaintbrushPickup : MonoBehaviour
{
    public GameObject wandObject;
    public AudioClip pickupSound;
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

            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            if (wandObject != null)
            {
                wandObject.GetComponent<PaintBrush>().enabled = true;
                Debug.Log("Paintbrush picked up!");
            }

            Destroy(gameObject);
        }
    }
}