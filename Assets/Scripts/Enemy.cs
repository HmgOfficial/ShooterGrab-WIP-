using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : EnemyManager
{
    /// <summary>
    /// The health points that the enemy has at the moment
    /// </summary>
    public int currentHP;
    /// <summary>
    /// The maximum health points that the enemy can have
    /// </summary>
    public int maxHP = 100;
    /// <summary>
    /// Enemy health bar
    /// </summary>
    public Image healthBar;
    /// <summary>
    /// Sprite that have a draw of the enemy
    /// </summary>
    public Sprite enemySprite;
    /// <summary>
    /// Canvas with the information of the enemy
    /// </summary>
    public Canvas enemyCanvas;
    /// <summary>
    /// Timer for the enemy canvas
    /// </summary>
    private float timerCanvas;
    /// <summary>
    /// 
    /// </summary>
    public GameObject txtDMGPrefab;
    /// <summary>
    /// Level of the enemy
    /// </summary>
    public int lvl;
    /// <summary>
    /// Define the type of the enemy
    /// </summary>
    public enum EnemyType { Guard};
    /// <summary>
    /// Define the current state of the enemy
    /// </summary>
    public enum EnemyState { Idle,Following, Death, Guardian, Attack};
    /// <summary>
    /// Saves the state of the enemy
    /// </summary>
    public EnemyState enemyState;
    private bool isRotating = false;
    public Vector3 initPos;
    private Rigidbody rb;
    public float enemyVelocity;

    // Start is called before the first frame update
    void Awake()
    {
        //Set the value of currentHP
        currentHP = maxHP;

        PlayerManager.instance = FindObjectOfType<PlayerManager>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        InvokeRepeating("CheckPlayerDistance", 1, 0.1f);
    }
    void CheckPlayerDistance()
    {
        Vector3 distance = transform.position - PlayerManager.instance.transform.position;
        if (Mathf.Abs(distance.magnitude) <= 5)
        {
            enemyState = EnemyState.Attack;
        }
        else if (Mathf.Abs(distance.magnitude) <= 10)
        {
            enemyState = EnemyState.Following;
        }
        else
        {
            enemyState = EnemyState.Guardian;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Update the transform of the enemy canvas so that it looks at the camera
        enemyCanvas.transform.LookAt(Camera.main.transform);
        //Check if the enemyCanvas is active
        if (enemyCanvas.gameObject.activeInHierarchy)
        {
            //Update the timer
            timerCanvas -= Time.deltaTime;
            //Check if the timer is minor than 0
            if (timerCanvas <= 0)
            {
                //Deactivate the enemyCanvas         
                enemyCanvas.gameObject.SetActive(false);
            }
        }

        
        if (Input.GetKeyDown(KeyCode.W))
        {

            GameObject clon = Instantiate(txtDMGPrefab, enemyCanvas.transform);
            
            
        }
        

        //Check the state of the enemy
        if (enemyState == EnemyState.Idle)
        {
            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 9;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;

            RaycastHit hit;
            
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position + (Vector3.up / 2), transform.TransformDirection(PlayerManager.instance.transform.position - transform.position), out hit, 10f, layerMask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(PlayerManager.instance.transform.position - transform.position) * hit.distance, Color.yellow);
                //Debug.Log("Did Hit");
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);
                //Debug.Log("Did not Hit");
            }
        }
        if (enemyState == EnemyState.Guardian)
        {
            RaycastHit hit;
            if (!isRotating)
            {
                //print(rb.velocity);
                if (rb.velocity.magnitude == 0)
                {
                    print("in");
                    //rb.AddRelativeForce(Vector3.forward * Time.deltaTime * enemyVelocity, ForceMode.Impulse);
                    rb.velocity = transform.forward * enemyVelocity;
                }
                Vector3 distance = transform.position - initPos;
                if (Mathf.Abs(distance.magnitude) >= 5)
                {
                    StartCoroutine("Rotate180");
                }
                // Does the ray intersect any objects excluding the player layer
                else if (Physics.Raycast(transform.position + (Vector3.up / 2), transform.forward, out hit, 3f/*, layerMask*/))
                {
                    StartCoroutine("Rotate180");
                }
            }
            
            
        }
        if (enemyState == EnemyState.Following)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            Vector3 direcction = PlayerManager.instance.transform.position - transform.position;
            transform.forward = direcction;
            rb.velocity = direcction.normalized * enemyVelocity;
        }
        if (enemyState == EnemyState.Attack)
        {
            if (rb.velocity.magnitude != 0)
            {
                rb.velocity = Vector3.zero;
                rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            }
        }
    }
    IEnumerator Rotate180()
    {
        if (!isRotating)
        {
            isRotating = true;
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }
        for (int i = 0; i < 180; i++)
        {
            transform.Rotate(Vector3.up);
            yield return 0;
        }
        isRotating = false;
        initPos = transform.position;
        rb.constraints = RigidbodyConstraints.FreezeRotation;


    }
    /// <summary>
    /// Enemy Get DMG
    /// </summary>
    /// <param name="dmg">The damage that the enemy recive</param>
    /// <param name="crit">If it's critical or not</param>
    public void GetDMG(float dmg, bool crit)
    {
        //Check if enemy canvas is not active
        if (!enemyCanvas.gameObject.activeInHierarchy)
        {
            //Active the enemyCanvas
            enemyCanvas.gameObject.SetActive(true);
        }
        //Update the currentHp
        currentHP -= (int)dmg;
        //Update the fill amount of the health bar
        healthBar.fillAmount = ((float)currentHP / (float)maxHP);
        //Creates a GameObject called textDMG
        GameObject textDMG = Instantiate(txtDMGPrefab, enemyCanvas.transform);
        //
        textDMG.transform.LookAt(Camera.main.transform);
        //Set the text of the GameObject textDMG
        textDMG.GetComponent<TMPro.TextMeshProUGUI>().text = dmg.ToString();
        //If the attack is critical
        if (crit)
        {
            //Change the color of textDMG to yellow
            textDMG.GetComponent<TMPro.TextMeshProUGUI>().color = Color.yellow;
        }
        //Set the timerCanvas
        timerCanvas = 3f;

    }
}
