using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    /*
        5 - Start
        4 - Paint done
        3 - Shade done
        2 - Mist done
        1 - Greenery done
    */
    public int gameProgressTracker = 5;

    [Header("Interaction 1: Paint")]
    public GameObject blockageToShade;

    [Header("Interaction 2: Shade")]
    public GameObject blockageToMist;

    [Header("Interaction 3: Mist")]
    public GameObject clearEastAndNorthTraffic;
    public GameObject invisibleWalls;
    public GameObject mistToGreeneryGunSign;
    public GameObject navigationSignage;

    [Header("Interaction 4: Greenery Gun")]
    public GameObject restOfTraffic;
    public GameObject backgroundFog;
    public GameObject C02Particles;

    [Header("Traffic Progression")]
    public GameObject[] trafficAfterPaint;
    public GameObject[] trafficAfterShade;
    public GameObject[] trafficAfterMist;
    public GameObject[] trafficFinal;

    [Header("Pedestrian Progression")]
    public GameObject[] peopleAfterPaint;
    public GameObject[] peopleAfterShade;
    public GameObject[] peopleAfterMist;
    public GameObject[] peopleFinal;

    public GameObject EditedSunObject;
    public ChangeSunColor ChangeSunColorScript;

    public AudioManager AudioManagerScript;


    void Update()
    {
        //DEBUGGING PURPOSES
        /*while (gameProgressTracker >= -1)
        {
            UpdateGameProgressScore();
        }*/

    }

    void Start()
    {
        ChangeSunColorScript = EditedSunObject.GetComponent<ChangeSunColor>();
        AudioManagerScript = GetComponent<AudioManager>();

        
    }

    // -------------------------
    // HELPERS
    // -------------------------
    void SetGroupActive(GameObject[] group, bool active)
    {
        if (group == null) return;

        foreach (GameObject obj in group)
        {
            if (obj != null)
                obj.SetActive(active);
        }
    }

    // -------------------------
    // PROGRESSION STEPS
    // -------------------------
    public void TriggerNextShadeInteraction()
    {
        blockageToShade.SetActive(false);

        // reduce some traffic, add some people
        SetGroupActive(trafficAfterPaint, false);
        SetGroupActive(peopleAfterPaint, true);
    }

    public void TriggerNextMistInteraction()
    {
        blockageToMist.SetActive(false);

        SetGroupActive(trafficAfterShade, false);
        SetGroupActive(peopleAfterShade, true);
    }

    public void TriggerNextGreeneryGunInteraction()
    {
        clearEastAndNorthTraffic.SetActive(false);
        invisibleWalls.SetActive(false);
        mistToGreeneryGunSign.SetActive(true);
        navigationSignage.SetActive(false);

        SetGroupActive(trafficAfterMist, false);
        SetGroupActive(peopleAfterMist, true);
    }

    public void TriggerNextEndingScene()
    {
        ChangeSunColorScript.UpdateSunColor();
        AudioManagerScript.WinConditionAudio();

        restOfTraffic.SetActive(false);
        backgroundFog.SetActive(false);
        C02Particles.SetActive(false);

        SetGroupActive(trafficFinal, false);
        SetGroupActive(peopleFinal, true);
    }

    public void UpdateGameProgressScore()
    {
        gameProgressTracker--;

        if (gameProgressTracker == 4)
        {
            TriggerNextShadeInteraction();
        }
        else if (gameProgressTracker == 3)
        {
            TriggerNextMistInteraction();
        }
        else if (gameProgressTracker == 2)
        {
            TriggerNextGreeneryGunInteraction();
        }
        else if (gameProgressTracker == 1)
        {
            TriggerNextEndingScene();
        }
    }
}
