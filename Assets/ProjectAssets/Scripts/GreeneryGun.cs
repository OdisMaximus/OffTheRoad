using UnityEngine;

public class GreeneryGun : MonoBehaviour
{
    public float range = 50f;
    public Texture2D greeneryTexture;
    public int ammo = 0;
    public AudioClip shootSound;
    public AudioClip emptyClickSound;
    public AudioClip reloadSound;
    private AudioSource audioSource;
    private float nextClickTime = 0f;
    public float clickCooldown = 0.3f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (CAVE2.GetButton(CAVE2.Button.ButtonLeft) || Input.GetKey(KeyCode.F))
        {
            ShootGreenery();
        }

    }

    void ShootGreenery()
    {
        if (ammo <= 0)
        {
            if (Time.time >= nextClickTime)
            {
                audioSource.PlayOneShot(emptyClickSound);
                nextClickTime = Time.time + clickCooldown;
            }
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Building"))
            {
                if (hitObject.GetComponent<Renderer>().material.mainTexture == greeneryTexture)
                {
                    return;
                }

                ammo--;
                audioSource.PlayOneShot(shootSound);

                hitObject.GetComponent<Renderer>().material.color = Color.white;
                hitObject.GetComponent<Renderer>().material.mainTexture = greeneryTexture;

                Transform buildingFull = hitObject.transform.parent;
                foreach (Transform child in buildingFull.GetComponentsInChildren<Transform>())
                {
                    if (child.CompareTag("Type"))
                    {
                        Renderer r = child.GetComponent<Renderer>();
                        if (r != null)
                        {
                            r.material.color = new Color(0.0f, 0.6f, 0.0f);
                        }
                    }
                }
            }
        }
    }

    public void AddAmmo(int amount)
    {
        ammo += amount;
        audioSource.PlayOneShot(reloadSound);
        Debug.Log("Ammo picked up! Ammo remaining: " + ammo);
    }
}