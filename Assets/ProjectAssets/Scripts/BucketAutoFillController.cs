using UnityEngine;

public class BucketAutoFillController : MonoBehaviour
{
    [Header("Visuals")]
    public Transform waterDisc;
    public ParticleSystem pourParticles;

    [Header("Y-Axis: Position & Scale")]
    public float emptyY;
    public float fullY;
    public Vector3 emptyScale;
    public Vector3 fullScale;

    [Header("Settings")]
    public float fillSpeed = 0.1f;
    public float spillRate = 0.5f;
    public float spillAngleThreshold = 45f;

    [Range(0, 1)] public float currentFill = 0f;

    void Update()
    {
        if (waterDisc == null) return;

        // 1. Calculate Tilt Angle
        float angle = Vector3.Angle(Vector3.up, transform.up);
        bool isTilted = angle > spillAngleThreshold;

        // 2. Fill/Spill Logic
        if (isTilted && currentFill > 0)
        {
            currentFill -= spillRate * Time.deltaTime;
            if (pourParticles != null && !pourParticles.isPlaying) pourParticles.Play();
        }
        else 
        {
            if (pourParticles != null && pourParticles.isPlaying) pourParticles.Stop();

            if (!isTilted && currentFill < 1.0f)
            {
                currentFill += fillSpeed * Time.deltaTime;
            }
        }

        currentFill = Mathf.Clamp01(currentFill);

        // 3. Visual Updates (Position + Scale)
        float lerpY = Mathf.Lerp(emptyY, fullY, currentFill);
        waterDisc.localPosition = new Vector3(waterDisc.localPosition.x, lerpY, waterDisc.localPosition.z);
        waterDisc.localScale = Vector3.Lerp(emptyScale, fullScale, currentFill);
        
        waterDisc.gameObject.SetActive(currentFill > 0.005f);
    }
}