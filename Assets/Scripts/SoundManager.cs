using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    /// <summary>
    /// The sound is on or off
    /// </summary>
    public bool soundOn = true;
    /// <summary>
    /// Array with the audioSources of the game
    /// </summary>
    public AudioSource[] audioSources;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        CheckSound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Check if the sound is on or off and play or pauses the audioSources
    /// </summary>
    public void CheckSound()
    {
        //If the audio is on
        if (soundOn)
        {
            //Play the audioSources
            for (int i = 0; i < audioSources.Length; i++)
            {
                audioSources[i].Play();
            }

            soundOn = false;
        }
        //If the audio is off
        else if (!soundOn)
        {
            //Pause the audioSources
            for (int i = 0; i < audioSources.Length; i++)
            {
                audioSources[i].Pause();
            }
            soundOn = true;

        }
    }
}
