using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject theModel;
    public MeshRenderer modelRenderer;
    public BoxCollider theModelBoxCollider;

    public Color startColor = new Color(14, 219, 230, 255); //PLACEHOLDER DEFAULT
    public Color endColor = new Color(14, 219, 230, 255); //PLACEHOLDER DEFAULT

    public Color currentColor = Color.red;

    public float time = 1.0f;
    public float speed = 1.0f;

    public bool allowHighlight = true;

    public GameObject shadeGrabbableObject;

    
    [Header("Shade Manager Object")]
    public GameObject ShadeManagerObject;
    public ShadeManager ShadeManagerScript;

    [Header("Shade Shadow Object")]
    public GameObject shadeShadowObject;

   

    //public MeshRenderer shadeShadowRenderer;

    void Start()
    {
        modelRenderer = theModel.GetComponent<MeshRenderer>();
        theModelBoxCollider = theModel.GetComponent<BoxCollider>();
        ShadeManagerScript = ShadeManagerObject.GetComponent<ShadeManager>();
        //InvokeRepeating("CycleColor", 2.0f, 2.0f);
    }

    
    // Update is called once per frame
    void Update()
    {
        if (allowHighlight) 
        {
            HighlightShadeToPlace();
        }
    }



    //IF SHADE IS PLACED, TURN ON SHADE
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("ShadeTag"))
        {
            allowHighlight = false;
            modelRenderer.enabled = true;
            shadeShadowObject.SetActive(true);

            modelRenderer.material.color = currentColor;
            Destroy(shadeGrabbableObject);



            //ADD TO 'shadeScore'
            ShadeManagerScript.NumShadePlacedUpdate();

        }
    }

    //HIGHLIGHTING BEHAVIOR
    void HighlightShadeToPlace()
    {
        if (Input.GetKey(KeyCode.Space) || CAVE2.GetButton(CAVE2.Button.ButtonLeft))
        {
            modelRenderer.enabled = true;
            modelRenderer.material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1));
        }
        else
        {
            modelRenderer.enabled = false;
        }
    }


    /*void CycleColor()
    {
        towardsColor = !towardsColor;
        if (towardsColor)
        {
            
        } 
        else
        {
            currentColor = Color.Lerp(endColor, startColor, time);
        }

        modelRenderer.material.color = currentColor;
    }*/
}
