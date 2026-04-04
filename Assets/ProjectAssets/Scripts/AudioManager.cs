using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update



    public AudioSource loudTrafficSounds;
    public AudioSource quietTrafficSounds;

    public TrafficManager trafficManagerScript;

    void Start()
    {
        trafficManagerScript = GetComponent<TrafficManager>();
        loudTrafficSounds.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WinConditionAudio()
    {
        loudTrafficSounds.Stop();
        quietTrafficSounds.Play();
        quietTrafficSounds.loop = true;

        //trafficManagerScript.FinalCongestionValue();
    }
}
