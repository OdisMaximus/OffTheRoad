using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSunColor : MonoBehaviour
{
    public Light sunColor;
    public Color colorLevel1 = new Color(241f, 221f, 162f, 255f);
    public Color colorLevel2 = new Color(141f, 221f, 162f, 255f);
    public Color colorLevel3 = new Color(41f, 221f, 162f, 255f);

    public Color defaultColorLevel = new Color(241f, 221f, 162f, 255f);

    //FOR SMOOTH TRANSITION
    public float speed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        sunColor = gameObject.GetComponent<Light>();
        //colorLevel1 = new Color(241f, 221f, 162f, 255f);
        //colorLevel2 = new Color(141f, 221f, 162f, 255f);

    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKey(KeyCode.Space) || CAVE2.GetButton(CAVE2.Button.ButtonLeft))
        {
            
            sunColor.color = Color.Lerp(colorLevel1, colorLevel2, Mathf.PingPong(Time.time * speed, 1));
        }
        else
        {
            sunColor.color = colorLevel1;
        }*/

    }

    public void UpdateSunColor()
    {
        sunColor.color = defaultColorLevel;
    }
}
