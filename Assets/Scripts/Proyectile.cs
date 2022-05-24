using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectile : MonoBehaviour

    
{
    private Rigidbody rb;
    public bool crit;
    public int critRandom;
    public float proyectileDMG;

    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void FixedUpdate()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if (other.tag == "Enemy")
        {
            PlayerManager.instance.enemyTarget = other.GetComponentInParent<Enemy>();
            rb.velocity = Vector3.zero;
            critRandom = Random.Range(0, 101);

            if (PlayerManager.instance.critProbability >= critRandom)
            {
                crit = true;
                proyectileDMG *= 2;
            }
            else
            {
                crit = false;
            }
            other.GetComponentInParent<Enemy>().GetDMG(proyectileDMG, crit);
            CanvasManager.instance.EnemyInfo();
            Destroy(gameObject);
        }

        else if (other.name != "Player Mesh")
        {
            rb.velocity = Vector3.zero;
        }
        
    }
    //public void
}
