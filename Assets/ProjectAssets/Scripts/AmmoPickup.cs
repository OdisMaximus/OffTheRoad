using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 4;
    public AmmoSpawner spawner;
    private bool collected = false;

    void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        GreeneryGun gun = other.GetComponentInChildren<GreeneryGun>();
        if (gun == null)
            gun = other.GetComponentInParent<GreeneryGun>();

        if (gun != null)
        {
            collected = true;
            gun.AddAmmo(ammoAmount);
            spawner.OnPickupCollected();
            Destroy(gameObject);
        }
    }
}