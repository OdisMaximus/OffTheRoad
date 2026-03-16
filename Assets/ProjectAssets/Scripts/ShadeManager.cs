using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadeManager : MonoBehaviour
{
    // Start is called before the first frame update

    public int numShadePlaced = 0;

    public GameObject CongestionObjectsWest;

    void Awake()
    {
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(numShadePlaced == 6)
        {
            OpenInteraction3();
            numShadePlaced = 100;

        }
            
    }

    public void NumShadePlacedUpdate()
    {
        numShadePlaced += 1;
    }

    public void OpenInteraction3()
    {
        CongestionObjectsWest.SetActive(false);
    }



    

}
