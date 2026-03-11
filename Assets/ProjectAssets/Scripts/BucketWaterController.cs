using UnityEngine;

public class BucketWaterController : MonoBehaviour
{
    [Header("Water Disc Reference")]
    public Transform waterDisc;
    public ParticleSystem pourParticles; // Drag your spill particles here

    [Header("Empty/Full States")]
    public Vector3 emptyLocalPosition;
    public Vector3 emptyLocalScale;
    public Vector3 fullLocalPosition;
    public Vector3 fullLocalScale;

    [Header("Settings")]
    public float fillPerParticle = 0.02f;
    public float spillRate = 0.5f;
    public float spillAngleThreshold = 75f;

    [Range(0, 1)]
    public float currentFill = 0f;

    void Update()
    {
        if (waterDisc == null) return;

        float tiltAngle = Vector3.Angle(Vector3.up, transform.up);
        bool isSpilling = tiltAngle > spillAngleThreshold && currentFill > 0.01f;

        // 1. Spill Logic
        if (isSpilling)
        {
            currentFill -= spillRate * Time.deltaTime;
            if (!pourParticles.isPlaying) pourParticles.Play();
        }
        else
        {
            if (pourParticles.isPlaying) pourParticles.Stop();
        }

        currentFill = Mathf.Clamp01(currentFill);

        // 2. Visual Update
        waterDisc.localPosition = Vector3.Lerp(emptyLocalPosition, fullLocalPosition, currentFill);
        waterDisc.localScale = Vector3.Lerp(emptyLocalScale, fullLocalScale, currentFill);
        waterDisc.gameObject.SetActive(currentFill > 0.001f);
    }

    void OnParticleCollision(GameObject other)
    {
        float tiltAngle = Vector3.Angle(Vector3.up, transform.up);
        if (tiltAngle <= spillAngleThreshold)
        {
            currentFill += fillPerParticle;
            currentFill = Mathf.Clamp01(currentFill);
        }
    }

    void OnTriggerStay(Collider other)
    {
        TankManager tank = other.GetComponent<TankManager>();
        if (tank != null)
        {
            float tiltAngle = Vector3.Angle(Vector3.up, transform.up);
            if (tiltAngle > spillAngleThreshold && currentFill > 0f)
            {
                tank.ReceiveWater(spillRate * Time.deltaTime);
            }
        }
    }

    void OnValidate() { Update(); } // Preview in editor
}