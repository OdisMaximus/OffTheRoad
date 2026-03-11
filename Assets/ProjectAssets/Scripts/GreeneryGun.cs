using UnityEngine;

public class GreeneryGun : MonoBehaviour
{
    public float range = 50f;
    public Texture2D greeneryTexture;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ShootGreenery();
        }
    }

    void ShootGreenery()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Building"))
            {
                Renderer r = hitObject.GetComponent<Renderer>();
                r.material.color = Color.white;
                r.material.mainTexture = greeneryTexture;

                Transform buildingFull = hitObject.transform.parent;
                foreach (Transform child in buildingFull.GetComponentsInChildren<Transform>())
                {
                    if (child.CompareTag("Type"))
                    {
                        Renderer tr = child.GetComponent<Renderer>();
                        if (tr != null)
                        {
                            tr.material.color = new Color(0.0f, 0.6f, 0.0f);
                        }
                    }
                }
            }
        }
    }
}