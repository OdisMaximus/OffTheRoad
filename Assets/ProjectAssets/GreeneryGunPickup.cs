using UnityEngine;

public class GreeneryGunPickup : MonoBehaviour
{
    public GameObject wandObject;
    public GameObject gunModel;
    public AudioClip pickupSound;
    private bool collected = false;

    void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            if (wandObject != null)
            {
                wandObject.GetComponent<GreeneryGun>().enabled = true;
                if (gunModel != null) gunModel.SetActive(true);
                Debug.Log("Greenery Gun picked up!");
            }

            Destroy(gameObject);
        }
    }
}