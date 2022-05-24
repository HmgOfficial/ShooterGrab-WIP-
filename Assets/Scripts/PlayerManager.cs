using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance = null;
    /// <summary>
    /// Posición hacia la que hay que desplazarse
    /// </summary>
    public Vector3 target;

    /// <summary>
    /// Distancia a la que se considera que el player ha llegado al target
    /// </summary>
    public float clearance = 0.1f;
    public float velocity = 2f;
    public JoystickMovement joystickMovement;
    public JoystickAttack joystickAttack;
    private bool canMove = true;
    public Rigidbody rb;
    public enum LifeStates { Alive, Death };
    public LifeStates lifeStates;
    //public enum ActionStates
    public GameObject proyectilePrefab;
    public GameObject swordPrefab;
    public Transform kp;
    public float currentAttackCD;
    public float baseAttackCD = 1f;
    public float proyectileVelocity;
    public bool onAttack = false;
    public float critProbability = 20;
    public Enemy enemyTarget = null;
    public bool isMovingToHook;
    public Vector3 targetDirecction;
    public enum PlayerType {Archer, Knight};
    public PlayerType playerType;
    public bool canRotate = true;
    public List<Transform> targetHooks = new List<Transform>();

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


    // Start is called before the first frame update
    void Start()
    {
        lifeStates = LifeStates.Alive;

        /*if (targetHooks.Count() == 0)
        {
            print(targetHooks);
        }*/

        //Stats stat = new Stats();
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.instance.gameMode == GameManager.GameMode.Game)
        {
            currentAttackCD -= Time.deltaTime;
        }
            
        /*if (Input.GetKeyDown(KeyCode.E))
        {
        }*/
    }

    void FixedUpdate()
    {
        if (GameManager.instance.gameMode == GameManager.GameMode.Game)
        {
            if (joystickMovement == null || joystickAttack == null)
            {
                joystickMovement = FindObjectOfType<JoystickMovement>();
                joystickAttack = FindObjectOfType<JoystickAttack>();
            }
            else
            {
                transform.localPosition += Time.deltaTime * new Vector3(joystickMovement.Horizontal, 0, joystickMovement.Vertical) * velocity;
                if ((joystickMovement.Horizontal != 0) || (joystickMovement.Vertical != 0) && canRotate == true)
                {
                    transform.forward = new Vector3(joystickMovement.Horizontal - transform.position.x, 0, joystickMovement.Vertical - transform.position.y);
                }
                if (isMovingToHook)
                {
                    MoveToHook();
                }
            }
        }
    }



    public void ArcherAttack()
    {
        if (currentAttackCD <= 0)
        {
            canRotate = false;
            RefreshContrains();
            GameObject proyectile = Instantiate(proyectilePrefab, kp.position, Quaternion.identity);
            proyectile.GetComponent<Proyectile>().proyectileDMG = 10;
            transform.forward = new Vector3(joystickAttack.Horizontal, 0, joystickAttack.Vertical);
            proyectile.transform.forward = new Vector3(joystickAttack.Horizontal, 0, joystickAttack.Vertical);
            proyectile.GetComponent<Rigidbody>().useGravity = false;
            Vector3 proyectileDirecction = (kp.position - transform.position);
            proyectileDirecction.y = 0;
            proyectile.GetComponent<Rigidbody>().AddForce(proyectileDirecction * proyectileVelocity, ForceMode.Impulse);
            currentAttackCD = baseAttackCD;
            canRotate = true; //esto debe ir como evento cuando termine la anim de attacar
            RefreshContrains();
            
        }


    }

    public void KnightAttack()
    {
        canRotate = false;
        RefreshContrains();
        //Quaternion swordRotation = Quaternion.LookRotation(targetDirecction);
        GameObject sword = Instantiate(swordPrefab, transform.position, Quaternion.identity/*, null*/);
        //sword.transform.forward = new Vector3(joystickAttack.Horizontal, 0, joystickAttack.Vertical);
        //rb.constraints = RigidbodyConstraints.FreezeRotationY;
        //GameObject pro = Instantiate<0>
        //sword.GetComponent<Proyectile>().proyectileDMG = 10;
        
    }

    #region pc version player look at
    /*private void FixedUpdate()
    {
        //PlayerDirecction();
    }*/
    /*public void PlayerDirecction()
    {
        //Se crea un rayo cuyo origen es el cursor del ratón
        //Y la dirección es donde mire la cámara
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Aquí se guarda información acerca de la colisión del rayo
        //RaycastHit hit;

        //Se lanza el rayo, y se compueba que colisiona con algo
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            //Sólo si colisinó con el suelo, se instancia el prefab
            if (hit.collider.tag == "Ground")
            {
                target = hit.point;
            }
        }
        if (target != null)
        {
            //Se obtiene la disrección de desplazamiento con el target
            Vector3 direction = target - transform.position;

            //Si la magnitud del vector execde a clearance, se desplaza al player
            if (direction.magnitude > clearance)
            {
                //El player se desplaza en dirección direction,
                //a velocity metros por segundo

                transform.forward = direction.normalized;
                //transform.Translate(Vector3.forward * velocity * Time.deltaTime);
            }
        }
    }*/

    #endregion

    public void ReciveDMG(int dmg)
    {

        //Stats.hp -= dmg;
        //activar trigger recibir damage del animator
        //MenuController.ShowStatsOnHUD(/*pasar clase o stats*/);
        CheckHP();
    }
    private void CheckHP()
    {
        /*if (hp <= 0)
        {
            //activar trigger muerte de animator y meter un evento al final de la anim que sea 
            //(Time.deltaTime = 0 y CanvasController.GO_Panel.SetActive(true) o CanvasController.ActiveGO_Panel)
	    }*/
    }

    public void RefreshStats()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotationY|RigidbodyConstraints.FreezeRotationX;
        //pos calcular los dmg o yokse
    }

    public void MoveToHook()
    {
        if (targetHooks != null)
        {
            if (canMove)
            {
                canMove = false;
            }
            rb.velocity = Vector3.zero;
            print(targetHooks[0].position);
            targetDirecction = targetHooks[0].transform.position - transform.position;
            targetDirecction.y = 0;
            rb.velocity = targetDirecction.normalized * 20f;
            if (clearance > targetDirecction.magnitude)
            {
                rb.velocity = Vector3.zero;
                isMovingToHook = false;
                canMove = true;
                targetHooks[0].GetComponent<Destroyable>().DeleteObject();
            }
        }
    }
    /// <summary>
    /// Update the costrains of the rigidbody
    /// </summary>
    public void RefreshContrains()
    {
        if (!canMove && !canRotate)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else if (!canMove)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        else if (!canRotate)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }
}



 public class Stats : PlayerManager
 {
    public int baseDmg, currentDmg;
    public int hp;
    public int playerLevel;
    public int skill1Level, skill2Level, skill3Level;
    public int skill1Dmg, skill2Dmg, skill3Dmg;


 }
 
