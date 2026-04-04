using UnityEngine;

public class ShadeManager : MonoBehaviour
{
    [Header("Shade Interaction Tracker")]
    public int numShadePlaced = 0;
    public AudioSource SoundEffectPlacedShade;
    public AudioSource VictoryAllShadePlaced;

    [Header("Shade Interaction Goal")]
    public int shadesRequired = 6; // Set this to 6 in Inspector

    [Header("Connection: Game Progress Manager")]
    public GameObject GameManagerObject;
    public GameProgressManager GameProgressManagerScript;

    void Start()
    {
        GameProgressManagerScript = GameManagerObject.GetComponent<GameProgressManager>();
    }
    
    void Update()
    {
       if(numShadePlaced == shadesRequired)
        {
            VictoryAllShadePlaced.Play();
            GameProgressManagerScript.UpdateGameProgressScore();


            numShadePlaced++;
        }
    }

    public void NumShadePlacedUpdate()
    {
        numShadePlaced += 1;
        
        if(numShadePlaced != shadesRequired)
        {
            SoundEffectPlacedShade.Play();
        }
            
    }

   
}