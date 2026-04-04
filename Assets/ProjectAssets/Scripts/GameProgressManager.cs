using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressManager : MonoBehaviour
{

    /* GAME PROGRESS TRACKER EXPLANATION
        5 - Nothing Done, everything on default values
        4 - 'Paint' interaction done, opens pathway to do 'Shade' interaction
        3 - 'Shade' interaction done, opens pathway to 'Mist' interaction
        2 - 'Mist' interaction done, opens pathway to 'Greenery Gun' interaction
        1 - 'Greenery Gun' interaction done, opens pathway to 
     
     */
    public int gameProgressTracker = 5;

    [Header("Interaction 1: Paint")]
    public GameObject blockageToShade;

    [Header("Interaction 2: Shade")]
    public GameObject blockageToMist;

    [Header("Interaction 3: Mist")]
    public GameObject clearEastAndNorthTraffic; //EAST + NORTH TRAFFIC CONGESTION REMOVED FOR GREENERY GUN
    public GameObject invisibleWalls; //CAN WALK STRAIGHT TO MIDDLE NOW
    public GameObject mistToGreeneryGunSign;
    public GameObject navigationSignage;

    [Header("Interaction 4: Greenery Gun")]
    public GameObject restOfTraffic;
    

    public GameObject EditedSunObject;
    public ChangeSunColor ChangeSunColorScript;
    

    public AudioManager AudioManagerScript; 

    [Header("Debugging")]
    /* Sets all traffic to dissapear, test to make sure all conections wired correctly + make it obvious something happened :]c */
    public GameObject TestDissapear;


    // Start is called before the first frame update
    void Start()
    {
        ChangeSunColorScript = EditedSunObject.GetComponent<ChangeSunColor>();
        AudioManagerScript = GetComponent<AudioManager>();

    }

    // Update is called once per frame
    void Update()
    {
        //DEBUGGING - Get through all scenes, mainly to see ending scene :]
        /*while(gameProgressTracker > -1)
        {
            UpdateGameProgressScore();
        }*/
            
    }

    public void TriggerNextShadeInteraction()
    {
        blockageToShade.SetActive(false);

    }

    public void TriggerNextMistInteraction()
    {
        
        blockageToMist.SetActive(false);
    }

    public void TriggerNextGreeneryGunInteraction()
    {
        clearEastAndNorthTraffic.SetActive(false);
        invisibleWalls.SetActive(false);
        mistToGreeneryGunSign.SetActive(true);
        navigationSignage.SetActive(false);


    }

    public void TriggerNextEndingScene()
    {
        ChangeSunColorScript.UpdateSunColor();
        AudioManagerScript.WinConditionAudio();
        restOfTraffic.SetActive(false);
        
    }

    public void UpdateGameProgressScore()
    {
        gameProgressTracker--;

        if (gameProgressTracker == 4)
        {
            TriggerNextShadeInteraction();
        
        } else if (gameProgressTracker == 3)
        {
            TriggerNextMistInteraction();
        
        } else if(gameProgressTracker == 2)
        {
            TriggerNextGreeneryGunInteraction();
        
        } else if (gameProgressTracker == 1)
        {
            TriggerNextEndingScene();
        }
            
    }
}
