using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
    public GameObject ammoPickupPrefab;
    public float respawnCooldown = 30f;
    public int ammoPerPickup = 4;

    private GameObject currentPickup;
    private float cooldownTimer = 0f;
    private bool onCooldown = false;

    void Start()
    {
        SpawnPickup();
    }

    void Update()
    {
        if (onCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                onCooldown = false;
                SpawnPickup();
            }
        }
    }

    void SpawnPickup()
    {
        currentPickup = Instantiate(ammoPickupPrefab, transform.position, Quaternion.identity);
        currentPickup.GetComponent<AmmoPickup>().ammoAmount = ammoPerPickup;
        currentPickup.GetComponent<AmmoPickup>().spawner = this;
    }

    public void OnPickupCollected()
    {
        onCooldown = true;
        cooldownTimer = respawnCooldown;
    }
}