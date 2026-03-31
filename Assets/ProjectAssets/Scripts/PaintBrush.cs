using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float brushRange = 10f;
    public LayerMask paintableLayer;

    [Header("Visual Feedback")]
    public Transform brushModel; 
    private Vector3 handLocalPos; 
    public float maxReachDistance = 0.5f; 
    public float lerpSpeed = 10f;
    public float surfaceBuffer = 0.1f; 

    [Header("Audio Settings")]
    public AudioSource paintAudioSource; // Link your new 'PaintSFX' object here
    [Range(0f, 1f)] public float paintVolume = 0.7f;
    [Range(0.1f, 2f)] public float paintPitch = 0.8f; 
    public float pitchRandomness = 0.05f; 
    public float audioFadeSpeed = 10f; 

    private bool isCurrentlyPainting = false;
    private float currentTargetZ;
    private float targetVolume = 0f;

    void Awake()
    {
        if (brushModel != null)
        {
            handLocalPos = brushModel.localPosition;
            currentTargetZ = handLocalPos.z;
        }
    }

    void OnEnable()
    {
        if (brushModel != null) brushModel.gameObject.SetActive(true);
        currentTargetZ = handLocalPos.z;
    }

    // This ensures that even if the script is disabled externally, the sound DIES.
    void OnDisable()
    {
        isCurrentlyPainting = false;
        if (paintAudioSource != null)
        {
            paintAudioSource.Stop();
            paintAudioSource.volume = 0f;
        }
    }

    void Update()
    {
        bool isInputActive = Input.GetButton("Fire1") || CAVE2.GetButton(CAVE2.Button.ButtonUp);

        if (isInputActive)
        {
            CastPaintRay();
        }
        else
        {
            StopPainting();
        }

        // Smoothly fade volume in/out
        if (paintAudioSource != null)
        {
            targetVolume = isCurrentlyPainting ? paintVolume : 0f;
            paintAudioSource.volume = Mathf.MoveTowards(paintAudioSource.volume, targetVolume, Time.deltaTime * audioFadeSpeed);
            
            // If we aren't painting and it's quiet, kill the engine
            if (!isCurrentlyPainting && paintAudioSource.volume <= 0f && paintAudioSource.isPlaying)
            {
                paintAudioSource.Stop();
            }
        }

        if (brushModel != null)
        {
            Vector3 targetPosition = new Vector3(handLocalPos.x, handLocalPos.y, currentTargetZ);
            brushModel.localPosition = Vector3.Lerp(brushModel.localPosition, targetPosition, Time.deltaTime * lerpSpeed);
        }
    }

    void CastPaintRay()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, brushRange, paintableLayer))
        {
            PaintableChunk chunk = hit.collider.GetComponent<PaintableChunk>();
            if (chunk != null)
            {
                chunk.ReceivePaint(Time.deltaTime);
                StartPaintingSFX();
                isCurrentlyPainting = true;

                float hitZ = Mathf.Clamp(hit.distance - surfaceBuffer, 0, maxReachDistance);
                currentTargetZ = handLocalPos.z + hitZ;
                return;
            }
        }
        StopPainting(); 
    }

    void StartPaintingSFX()
    {
        if (paintAudioSource != null && !paintAudioSource.isPlaying)
        {
            paintAudioSource.pitch = paintPitch + Random.Range(-pitchRandomness, pitchRandomness);
            paintAudioSource.Play();
        }
    }

    public void StopPainting()
    {
        isCurrentlyPainting = false;
        currentTargetZ = handLocalPos.z; 
    }

    public void FinalizeWinState()
    {
        // Disabling the script triggers OnDisable() which stops the sound
        this.enabled = false; 
        if (brushModel != null) brushModel.gameObject.SetActive(false);
    }
}