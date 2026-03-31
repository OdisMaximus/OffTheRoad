using UnityEngine;

public class ValveController : MonoBehaviour
{
    [Header("Dependencies")]
    public TankManager tank;
    public LayerMask valveLayer; 

    [Header("Twist Settings")]
    public float maxRotationsToFill = 5f; 
    public float fillMultiplier = 1f;

    private bool isGrabbingValve = false;
    private Transform grabbedValve;
    private float previousWandRoll;

    void Update()
    {
        // Fire1 is Left-Click (PC). ButtonUp is the Top button on the CAVE2 Wand.
        bool grabInputDown = Input.GetButtonDown("Fire1") || CAVE2.GetButtonDown(CAVE2.Button.ButtonUp);
        bool grabInputHeld = Input.GetButton("Fire1") || CAVE2.GetButton(CAVE2.Button.ButtonUp);
        bool grabInputUp = Input.GetButtonUp("Fire1") || CAVE2.GetButtonUp(CAVE2.Button.ButtonUp);

        // 1. Raycast (Just like the PaintBrush)
        if (grabInputDown)
        {
            // Ray starts from THIS object's position and shoots forward
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

        // 2. Rotation & Filling
        if (isGrabbingValve && grabInputHeld && grabbedValve != null)
        {
            float deltaRoll = 0;

            // CAVE2: Calculate change in wrist rotation
            float currentWandRoll = transform.eulerAngles.z;
            deltaRoll = Mathf.DeltaAngle(previousWandRoll, currentWandRoll);
            previousWandRoll = currentWandRoll;

            // PC TESTING: Use Q and E to spin while holding Click
            if (Input.GetKey(KeyCode.T)) deltaRoll = 2f; 
            if (Input.GetKey(KeyCode.Y)) deltaRoll = -2f;

            if (Mathf.Abs(deltaRoll) > 0.01f)
            {
                // Spin the valve
                grabbedValve.Rotate(0, 0, deltaRoll, Space.Self);

                // If turning clockwise, add water
                if (deltaRoll > 0)
                {
                    float totalDegreesNeeded = 360f * maxRotationsToFill;
                    float fillAmount = (deltaRoll / totalDegreesNeeded) * tank.maxWater;
                    tank.AddWaterFromValve(fillAmount * fillMultiplier);
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