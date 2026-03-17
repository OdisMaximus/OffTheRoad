using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BucketWaterController : MonoBehaviour
{
    [Header("Water Disc Reference")]
    public Transform waterDisc;
    public ParticleSystem pourParticles;

    [Header("Empty/Full States")]
    public Vector3 emptyLocalPosition;
    public Vector3 emptyLocalScale;
    public Vector3 fullLocalPosition;
    public Vector3 fullLocalScale;

    [Header("Physics & Weight")]
    public float emptyMass = 1f;
    public float fullMass = 5f;
    public Transform customCenterOfMass;
    public float uprightSpeed = 10f; 

    [Header("Settings")]
    public float fillPerParticle = 0.005f;
    public float spillRate = 0.4f;
    public float spillAngleThreshold = 45f;

    [Range(0, 1)]
    public float currentFill = 0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        if (customCenterOfMass != null)
            rb.centerOfMass = customCenterOfMass.localPosition;

        // Stops the bucket from sliding down the road
        rb.drag = 2f;
        rb.angularDrag = 5f;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
    }

    void Update()
    {
        if (waterDisc == null) return;

        // 1. Update Mass
        rb.mass = Mathf.Lerp(emptyMass, fullMass, currentFill);

        // 2. Tipping & Pouring
        float tiltAngle = Vector3.Angle(Vector3.up, transform.up);
        bool isSpilling = tiltAngle > spillAngleThreshold && currentFill > 0f;

        if (isSpilling)
        {
            float tiltMulti = Mathf.InverseLerp(spillAngleThreshold, 180f, tiltAngle);
            currentFill -= spillRate * (tiltMulti + 0.2f) * Time.deltaTime;
            
            if (pourParticles != null && !pourParticles.isPlaying) pourParticles.Play();
        }
        else
        {
            if (pourParticles != null && pourParticles.isPlaying) pourParticles.Stop();
        }

        currentFill = Mathf.Clamp01(currentFill);

        // 3. Visual Update (The part we missed!)
        waterDisc.localPosition = Vector3.Lerp(emptyLocalPosition, fullLocalPosition, currentFill);
        waterDisc.localScale = Vector3.Lerp(emptyLocalScale, fullLocalScale, currentFill);
        waterDisc.gameObject.SetActive(currentFill > 0.001f);
    }

    void FixedUpdate()
    {
        // 4. Stabilization Logic (Gentle, no spasms)
        if (rb.velocity.magnitude < 0.2f && rb.angularVelocity.magnitude < 0.2f)
        {
            float tiltAngle = Vector3.Angle(Vector3.up, transform.up);
            if (tiltAngle < spillAngleThreshold)
            {
                Quaternion uprightRotation = Quaternion.FromToRotation(transform.up, Vector3.up) * rb.rotation;
                rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, uprightRotation, uprightSpeed * Time.fixedDeltaTime));
            }
        }
    }

    // 5. This handles the Particle System "Triggers" Module
    void OnParticleTrigger()
    {
        float tiltAngle = Vector3.Angle(Vector3.up, transform.up);
        if (tiltAngle < spillAngleThreshold)
        {
            currentFill += fillPerParticle;
            Debug.Log("Filling! " + currentFill);
        }
    }
}