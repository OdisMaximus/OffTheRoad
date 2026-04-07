using System.Collections;
using UnityEngine;

public class TrafficLooper : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Optional: If empty, resets to editor position. If set, resets to this Transform.")]
    public Transform startPoint; 
    public Transform endPoint;
    public float speed = 2f;
    public bool faceMovementDirection = true;
    public float endThreshold = 1f;

    [Header("Fade")]
    public float fadeDuration = 1f;

    private Vector3 initialEditorPosition; 
    private Renderer[] renderers;
    private bool isResetting = false;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        
        // 1. Capture birth-place
        initialEditorPosition = transform.position;

        if (endPoint == null)
        {
            Debug.LogWarning($"TrafficLooper on {gameObject.name} missing endPoint.");
            enabled = false;
            return;
        }

        // REMOVED: transform.position = GetResetPosition();
        // Now they stay exactly where you parked them in the scene.

        if (faceMovementDirection) FaceTarget(endPoint.position);
        SetAlpha(1f);
    }

    void Update()
    {
        if (isResetting) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            endPoint.position,
            speed * Time.deltaTime
        );

        if (faceMovementDirection) FaceTarget(endPoint.position);

        if (Vector3.Distance(transform.position, endPoint.position) <= endThreshold)
        {
            StartCoroutine(ResetLoop());
        }
    }

    private Vector3 GetResetPosition()
    {
        return (startPoint != null) ? startPoint.position : initialEditorPosition;
    }

    IEnumerator ResetLoop()
    {
        isResetting = true;
        yield return StartCoroutine(FadeTo(0f));

        // Only teleport AFTER reaching the end point
        transform.position = GetResetPosition();

        if (faceMovementDirection) FaceTarget(endPoint.position);

        yield return StartCoroutine(FadeTo(1f));
        isResetting = false;
    }

    // --- Helpers ---
    IEnumerator FadeTo(float targetAlpha)
    {
        float timer = 0f;
        float startAlpha = (renderers.Length > 0 && renderers[0].material.HasProperty("_Color")) 
            ? renderers[0].material.color.a : 1f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            SetAlpha(Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration));
            yield return null;
        }
        SetAlpha(targetAlpha);
    }

    void SetAlpha(float alpha)
    {
        foreach (Renderer rend in renderers)
        {
            if (rend != null && rend.material.HasProperty("_Color"))
            {
                Color c = rend.material.color;
                c.a = alpha;
                rend.material.color = c;
            }
        }
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0f;
        if (direction.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(direction);
    }
}