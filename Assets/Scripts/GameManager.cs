using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;      //Static instance of GameManager which allows it to be accessed by any other script.
    /// <summary>
    /// Timer global for the time or something similar
    /// </summary>
    public float globalTimer = 0;
    public enum GameMode { Menu,Game};
    public GameMode gameMode;
                  

    //Awake is always called before any Start functions
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
        // Start is called before the first frame update
    }

    public void StartGame()
    {
        //CanvasManager.instance.
        SceneManager.LoadScene("WIP");
        print(":)");
        gameMode = GameMode.Game;
        CanvasManager.instance.OnChangeScene();
        PlayerManager.instance.transform.position = Vector3.up;
    }
}
