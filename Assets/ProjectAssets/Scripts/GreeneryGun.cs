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

    [Header("Win Condition to Win")]
    public int GreeneryAdded = 0;

    public GameObject groupOfTraffic;

    public GameObject GameManager;
    public AudioManager AudioManagerScript;

    public GameObject SunObject;
    public ChangeSunColor ChangeSunColorScript;

    void Start()
    {
        AudioManagerScript = GameManager.GetComponent<AudioManager>();
        ChangeSunColorScript = SunObject.GetComponent<ChangeSunColor>();
        
        audioSource = GetComponent<AudioSource>();
        
        
    }

    void Update()
    {
        if (CAVE2.GetButton(CAVE2.Button.ButtonLeft) || Input.GetKey(KeyCode.F))
        {
            ShootGreenery();
        }

        if(GreeneryAdded == 6)
        {
            groupOfTraffic.SetActive(false);
            AudioManagerScript.WinConditionAudio();
            ChangeSunColorScript.UpdateSunColor();

            GreeneryAdded = 100;

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
                EnableGreenery(hitObject);

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
                        foreach (Renderer childRenderer in child.GetComponentsInChildren<Renderer>())
                        {
                            childRenderer.material.color = new Color(0.0f, 0.6f, 0.0f);
                        }
                    }
                }
            }
        }
    }

    void EnableGreenery(GameObject buildingCube)
    {
        Transform buildingFull = buildingCube.transform.parent;
        Transform greeneryGroup = buildingFull.Find("Greenery_group");

        if (greeneryGroup != null)
        {
            greeneryGroup.gameObject.SetActive(true);
            Debug.Log("Greenery enabled!");
            
            //LINE NEEDED FOR WIN CONDITION
            GreeneryAdded++;
        }
        else
        {
            Debug.Log("Greenery_group not found!");
        }
    }

    public void AddAmmo(int amount)
    {
        ammo += amount;
        audioSource.PlayOneShot(reloadSound);
        Debug.Log("Ammo picked up! Ammo remaining: " + ammo);
    }
}