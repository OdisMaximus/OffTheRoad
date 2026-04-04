using UnityEngine;

public class GreeneryGun : MonoBehaviour
{
    public float range = 50f;
    public Texture2D greeneryTexture;
    public AudioClip shootSound;
    public AudioClip winSound;
    public int totalBuildings = 6;
    public int buildingsGreened = 0;
    public TrafficManager trafficManager;
    private AudioSource audioSource;


    [Header("Connection: Game Progress Manager Here")]
    public GameObject GameManagerObject;
    public GameProgressManager GameProgressManagerScript;

    void Start()
    {
        GameProgressManagerScript = GameManagerObject.GetComponent<GameProgressManager>();

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

                buildingsGreened++;
                if (buildingsGreened == totalBuildings) //WIN CONDITION: 6
                {
                    TriggerWin();
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
        }
    }

    void TriggerWin()
    {
        GameProgressManagerScript.UpdateGameProgressScore();
        audioSource.PlayOneShot(winSound);
        if (trafficManager != null)
        {
            trafficManager.ReduceCongestion(trafficManager.maxCongestionLevel);
        }
    }
}