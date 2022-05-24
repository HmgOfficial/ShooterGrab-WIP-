using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance = null;   //Static instance of GameManager which allows it to be accessed by any other script.
    public GameObject panel_Main;
    public GameObject panel_Exit;
    public GameObject panel_Settings;
    public GameObject[] panel_Shop;
    public GameObject panel_HUD;
    public GameObject panel_Menu;
    public Image hpStat_img;
    #region Enemy INFO
    /// <summary>
    /// Panel of the Enemy target
    /// </summary>
    public GameObject enemyInfoPanel;
    /// <summary>
    /// Enemy target HealthPoints bar
    /// </summary>
    public Image enemyBarHp;
    /// <summary>
    /// The identity image of the Enemy target
    /// </summary>
    public Image enemyHUDSprite;
    /// <summary>
    /// The current and max HP of the Enemy target
    /// </summary>
    public /*TMPro.TextMeshProUGUI*/Text enemyHP;
    /// <summary>
    /// Enemy target level
    /// </summary>
    public Text enemyLvlName;
    /// <summary>
    /// Enemy target
    /// </summary>
    private Enemy target;
    #endregion
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
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Set the enemy info in the enemy canvas
    /// </summary>
    public void EnemyInfo(/*Enemy target*/)
    {
        //target takes his value from player.enemyTarget
        target = PlayerManager.instance.enemyTarget;
        // if target has no value
        if (target == null)
        {
            //Deactivate the EnemyPanel
            enemyInfoPanel.SetActive(false);
        }
        else
        {
            //Check If the enemyPanel is not active
            if (enemyInfoPanel.activeInHierarchy == false)
            {
                //Active the panel
                enemyInfoPanel.SetActive(true);
            }
            //Update/Set the level and name of the enemy
            enemyLvlName.text = ("LVL. " + target.lvl + " " + target.name);
            //Update/Set the currentHP and maxHP
            enemyHP.text = (target.currentHP + " / " + target.maxHP);
            ////Update/Set the fillAmount
            enemyBarHp.fillAmount = ((float)target.currentHP / (float)target.maxHP);

            //enemyHUDSprite = player.enemyTarget.enemySprite

        }
    }
    #region Exit
    // Check if the exit panel it's active and if it is not active, active it and if is active, desactive him or exit the game
    public void Exit()
    {
        if (panel_Exit.activeInHierarchy)
        {
            Application.Quit();
        }
        else
        {
            panel_Exit.gameObject.SetActive(true);

        }

    }
    public void CloseExitPanel()
    {
        panel_Exit.gameObject.SetActive(false);
    }
    #endregion
    public void Settings()
    {
        if (panel_Settings.activeInHierarchy)
        {
            panel_Settings.gameObject.SetActive(false);
        }
        else
        {
            panel_Settings.gameObject.SetActive(true);
        }
    }

    public void ShowStatsOnHUD(/*player stats*/)
    {
        //hpStat_img.fillAmount = playerC.currentHp / playerC.maxHp;
        //etc etc etc
    }

    public void Shop(int index)
    {
        if (!panel_Shop[0].activeInHierarchy)
        {
            panel_Shop[0].SetActive(true);
        }
        if (index == 5)
        {
            for (int i = 0; i < panel_Shop.Length; i++)
            {
                panel_Shop[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 1; i < panel_Shop.Length; i++)
            {
                if (i == index)
                {
                    panel_Shop[i].SetActive(true);
                }
                else
                {
                    if (panel_Shop[i].activeInHierarchy)
                    {
                        panel_Shop[i].SetActive(false);
                    }
                }
            }
        } 
    }
    public void OnChangeScene()
    {
        if (GameManager.instance.gameMode == GameManager.GameMode.Menu)
        {
            panel_HUD.SetActive(false);
            panel_Main.SetActive(true);
        }
        else if (GameManager.instance.gameMode == GameManager.GameMode.Game)
        {
            panel_HUD.SetActive(true);
            panel_Main.SetActive(false);
        }
    }
       
}

