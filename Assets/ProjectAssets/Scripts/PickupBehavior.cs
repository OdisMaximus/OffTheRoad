using UnityEngine;

public class PickupBehavior : MonoBehaviour
{
    public float rotateSpeed = 90f;
    public float bobHeight = 0.2f;
    public float bobSpeed = 1f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);

        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}