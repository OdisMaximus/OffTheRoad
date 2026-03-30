using System.Collections;
using UnityEngine;

// TrafficLooper
// Moves an object from startPoint to endPoint,
// fades it out, resets it to startPoint,
// then fades it back in and loops.
//
// Intended for cars or pedestrians.
// Assign:
// - startPoint
// - endPoint
// - speed
// - fadeDuration
public class TrafficLooper : MonoBehaviour
{
    [Header("Movement")]
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 2f;
    public bool faceMovementDirection = true;
    public float endThreshold = 1f;

    [Header("Fade")]
    public float fadeDuration = 1f;

    private Renderer[] renderers;
    private bool isResetting = false;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        if (startPoint == null || endPoint == null)
        {
            Debug.LogWarning("TrafficLooper on " + gameObject.name + " is missing startPoint or endPoint.");
            enabled = false;
            return;
        }

        transform.position = startPoint.position;

        if (faceMovementDirection)
        {
            FaceTarget(endPoint.position);
        }

        SetAlpha(1f);
    }

    void Update()
    {
        if (isResetting)
            return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            endPoint.position,
            speed * Time.deltaTime
        );

        if (faceMovementDirection)
        {
            FaceTarget(endPoint.position);
        }

        float distanceToEnd = Vector3.Distance(transform.position, endPoint.position);

        if (distanceToEnd <= endThreshold)
        {
            StartCoroutine(ResetLoop());
        }
    }

    IEnumerator ResetLoop()
    {
        isResetting = true;

        yield return StartCoroutine(FadeTo(0f));

        transform.position = startPoint.position;

        if (faceMovementDirection)
        {
            FaceTarget(endPoint.position);
        }

        yield return StartCoroutine(FadeTo(1f));

        isResetting = false;
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        float timer = 0f;

        float startAlpha = 1f;
        if (renderers.Length > 0 && renderers[0] != null && renderers[0].material.HasProperty("_Color"))
        {
            startAlpha = renderers[0].material.color.a;
        }

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            SetAlpha(alpha);
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
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}