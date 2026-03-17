using System.Collections;
using UnityEngine;



// TrafficLooper
// Moves an object from startPoint to endPoint,
// fades it out, resets it to startPoint,
// then fades it back in and loops.
//
//for cars or pedestrians.
// Assign Field:
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

    [Header("Fade")]
    public float fadeDuration = 1f;

    private Renderer[] renderers;
    private bool isResetting = false;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        if (startPoint != null)
        {
            transform.position = startPoint.position;
        }

        SetAlpha(1f);
    }

    void Update()
    {
        if (startPoint == null || endPoint == null || isResetting)
            return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            endPoint.position,
            speed * Time.deltaTime
        );

        float distanceToEnd = Vector3.Distance(transform.position, endPoint.position);

        if (distanceToEnd <= 1f)
        {
            StartCoroutine(ResetLoop());
        }
    }

    IEnumerator ResetLoop()
    {
        isResetting = true;

        yield return StartCoroutine(FadeTo(0f));

        transform.position = startPoint.position;

        yield return StartCoroutine(FadeTo(1f));

        isResetting = false;
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        float timer = 0f;

        float startAlpha = 1f;
        if (renderers.Length > 0 && renderers[0].material.HasProperty("_Color"))
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
}