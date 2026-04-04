using UnityEngine;

public class ValveController : MonoBehaviour
{
    [Header("Dependencies")]
    public TankManager tank;
    public LayerMask valveLayer; 

    [Header("Twist Settings")]
    public float maxRotationsToFill = 1f; 
    public float fillMultiplier = 4f;

    [Header("Audio")]
    public AudioClip fillingSound;
    private AudioSource audioSource;

    private bool isGrabbingValve = false;
    private Transform grabbedValve;
    private float previousWandRoll;

    

    void Start()
    {
        
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        bool grabInputDown = Input.GetButtonDown("Fire1") || CAVE2.GetButtonDown(CAVE2.Button.ButtonUp);
        bool grabInputHeld = Input.GetButton("Fire1") || CAVE2.GetButton(CAVE2.Button.ButtonUp);
        bool grabInputUp = Input.GetButtonUp("Fire1") || CAVE2.GetButtonUp(CAVE2.Button.ButtonUp);

        if (grabInputDown)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10f, valveLayer))
            {
                isGrabbingValve = true;
                grabbedValve = hit.transform;
                previousWandRoll = transform.eulerAngles.z;
                Debug.Log("VALVE: Caught " + grabbedValve.name);
            }
        }

        if (isGrabbingValve && grabInputHeld && grabbedValve != null)
        {
            float deltaRoll = 0;

            float currentWandRoll = transform.eulerAngles.z;
            deltaRoll = Mathf.DeltaAngle(previousWandRoll, currentWandRoll);
            previousWandRoll = currentWandRoll;

            if (Input.GetKey(KeyCode.T)) deltaRoll = 5f; 
            if (Input.GetKey(KeyCode.Y)) deltaRoll = -2f;

            if (Mathf.Abs(deltaRoll) > 0.01f)
            {
                grabbedValve.Rotate(0, 0, deltaRoll, Space.Self);

                if (deltaRoll > 0)
                {
                    float totalDegreesNeeded = 360f * maxRotationsToFill;
                    float fillAmount = (deltaRoll / totalDegreesNeeded) * tank.maxWater;
                    tank.AddWaterFromValve(fillAmount * fillMultiplier);

                    // Play filling sound while turning
                    if (audioSource != null && fillingSound != null && !audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(fillingSound);
                    }
                }
            }
        }

        if (grabInputUp)
        {
            isGrabbingValve = false;
            grabbedValve = null;
        }
    }
}