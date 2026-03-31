using UnityEngine;

public class ShadeManager : MonoBehaviour
{
    public int numShadePlaced = 0;
    public int shadesRequired = 6; // Set this to 6 in Inspector
    public GameObject CongestionObjectsWest;
    
    private bool interaction3Opened = false;

    void Update()
    {
        // Added interaction3Opened check so it doesn't keep running every frame
        if(!interaction3Opened && numShadePlaced >= shadesRequired)
        {
            OpenInteraction3();
        }
    }

    public void NumShadePlacedUpdate()
    {
        numShadePlaced += 1;
    }

    public void OpenInteraction3()
    {
        interaction3Opened = true;
        numShadePlaced = 100; // Keep your flag logic

        if(CongestionObjectsWest != null) 
        {
            CongestionObjectsWest.SetActive(false);
            Debug.Log("Roadblocks cleared by Shade Manager!");
        }
    }
}