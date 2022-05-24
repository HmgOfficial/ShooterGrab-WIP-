using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    public int swordRange = 35;
    private Rigidbody rb;
    //public float initialYRotation;
    public bool crit;
    public int critRandom;
    public float swordDMG;
    private Animation anim;
    private AnimationClip attackAnim;

    private void Awake()
    {
        //Get the rigidbody component
        rb = GetComponent<Rigidbody>();
        //Find the player controller script
        anim = GetComponent<Animation>();
    }
    void Start()
    {
        transform.rotation = PlayerManager.instance.transform.rotation;
        AnimationCurve curve = AnimationCurve.Linear(0, -swordRange, 1, swordRange);
        attackAnim = new AnimationClip
        {
            legacy = true
        };
        attackAnim.SetCurve("", typeof(Transform), "localRotation.y", curve);
        anim.AddClip(attackAnim, "test");
        anim.Play("test");
        //https://docs.unity3d.com/es/530/ScriptReference/AnimationClip.SetCurve.html
    }






    //transform.rotation = player.transform.rotation;
    //initialYRotation = transform.localRotation.y;
    //transform.Rotate(Vector3.up * -swordRange);


    // Update is called once per frame
    void Update()
    {
        transform.position = PlayerManager.instance.transform.position;
        /*print(initialYRotation + swordRange * 2 + "AAAAAAAAAA");
        print(transform.localRotation.y + "YYYYYYYYYYYYYY");
        if (transform.localRotation.y <= initialYRotation + swordRange*2)
        {
            
            
            transform.Rotate(Vector3.up);
        }
        else
        {
            Destroy(gameObject);
        }*/
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            PlayerManager.instance.enemyTarget = other.GetComponentInParent<Enemy>();
            rb.velocity = Vector3.zero;
            critRandom = Random.Range(0, 101);

            if (PlayerManager.instance.critProbability >= critRandom)
            {
                crit = true;
                swordDMG *= 2;
            }
            else
            {
                crit = false;
            }
            other.GetComponentInParent<Enemy>().GetDMG(swordDMG, crit);
            CanvasManager.instance.EnemyInfo();
        }

        else
        {
            rb.velocity = Vector3.zero;
        }

    }
    private void OnDestroy()
    {
        PlayerManager.instance.canRotate = false;
        PlayerManager.instance.RefreshContrains();
    }
}
