using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hook : MonoBehaviour
{
    /// <summary>
    /// if the hook was thrown
    /// </summary>
    private bool inAir = false;
    /// <summary>
    /// Rigidbody component
    /// </summary>
    public Rigidbody rb;
    /// <summary>
    /// The velocity with that the hook is moving
    /// </summary>
    public float hookVelocity = 20;

    private void Awake()
    {
    }

    private void FixedUpdate()
    {

    }
    public void ComprobePlayerDistance()
    {
        //If inAir is true
        if (inAir)
        {
            //Calculate the vector from the hook to the player
            Vector3 playerDistance = transform.position - PlayerManager.instance.transform.position;
            //If the magnitude of the distance it's over than +-10
            if ((Mathf.Abs(playerDistance.magnitude)) > 10f)
            {
                //Stop the hook
                rb.velocity = Vector3.zero;
                //Change the state of the object
                inAir = false;
                //rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            }
        }
    }

    //Shoot the object in the direction that you give
    public void GrapplingShot(float x, float z)
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        //Calculate the direction of the hook
        Vector3 direcction = new Vector3(x, 0, z);
        //Set the velocity of the object
        rb.velocity = direcction.normalized * hookVelocity;
        //The object now it's inAir
        inAir = true;
    }

    private void OnCollisionEnter(Collision col)
    {
        //Stop the object
        rb.velocity = Vector3.zero;
        //if the object is inAir and the collision is not with the player 
        if (inAir == true && col.gameObject.name != "PlayerMesh" /*&&*/)
        {
            //Stop the object
            rb.velocity = Vector3.zero;
            //The object now it is not inAir
            inAir = false;

            /*grabHinge = gameObject.AddComponent<HingeJoint>();
            grabHinge.connectedBody = col.GetComponent<Rigidbody>();*/
            //This stops the hook once it collides with something, and creates a HingeJoint to the object it collided with.



            //rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            if (PlayerManager.instance.targetHooks.Count() == 0)
            {
                PlayerManager.instance.targetHooks.Add(transform);
                //Now the player is moving to the hook
                PlayerManager.instance.isMovingToHook = true;
                print("aahhh");

                print(transform.position);
            }
            else if (PlayerManager.instance.targetHooks[0].transform.position != transform.position)
            {
                if (PlayerManager.instance.isMovingToHook)
                {
                    PlayerManager.instance.targetHooks.Add(transform);
                    PlayerManager.instance.targetHooks.RemoveAt(0);
                    if (!PlayerManager.instance.isMovingToHook)
                    {
                        PlayerManager.instance.isMovingToHook = true;
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        //the hook attached to the player is null
        if (PlayerManager.instance.targetHooks[0] == transform)
        {
            PlayerManager.instance.targetHooks.RemoveAt(0);
            
        }
    }

    /*public void MoveToInitialPos()
    {
        print("HI");
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.constraints = RigidbodyConstraints.None;
        rb.velocity = Vector3.zero;
        transform.position = player.transform.position;
        transform.rotation = player.transform.rotation;
    }*/


}
