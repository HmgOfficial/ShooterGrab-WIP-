using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickAttack : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    /// <summary>
    /// 
    /// </summary>
    public Canvas canvas_HUD;
    /// <summary>
    /// The position when the game starts
    /// </summary>
    Vector3 inicial_Pos;
    /// <summary>
    /// The radio in that the JoyStick can be moved
    /// </summary>
    public float radio;
    /// <summary>
    /// X and Y of the Joystick
    /// </summary>
    private Vector2 axis;
    public Vector2 Axis
    {
        get
        {
            return axis;
        }
    }
    /// <summary>
    /// X axis
    /// </summary>
    public float Horizontal
    {
        get
        {
            return axis.x;
        }
    }
    /// <summary>
    /// Y axis
    /// </summary>
    public float Vertical
    {
        get
        {
            return axis.y;
        }
    }
    public bool IsMoving
    {
        get
        {
            return axis != Vector2.zero;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Set the inicial position
        inicial_Pos = transform.position;
        
        
        /*if (IsMoving)
        {

        }*/
    }

    public void OnDrag(PointerEventData point)
    {
        //Joystick position
        Vector2 joystick_pos;
        //Set the joystick position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas_HUD.transform as RectTransform, point.position, canvas_HUD.worldCamera, out joystick_pos);
        //
        Vector3 newPosition = canvas_HUD.transform.TransformPoint(joystick_pos) - inicial_Pos;
        //
        newPosition.x = Mathf.Clamp(newPosition.x, -radio, radio);
        //
        newPosition.y = Mathf.Clamp(newPosition.y, -radio, radio);

        //Set the axis
        axis = newPosition / radio;

        //Moves the joystick to new position
        transform.localPosition = newPosition;

        //If the player is an archer
        if (PlayerManager.instance.playerType == PlayerManager.PlayerType.Archer)
        {
            //Call the method ArcherAttack
            PlayerManager.instance.ArcherAttack();
        }

    }

    public void OnEndDrag(PointerEventData point)
    {
        if (PlayerManager.instance.playerType == PlayerManager.PlayerType.Knight)
        {
            PlayerManager.instance.KnightAttack();
        }
        transform.position = inicial_Pos;
        axis = Vector2.zero;
        PlayerManager.instance.onAttack = false;
        //GetComponentInParent<Image>().enabled = false;
        //GetComponent<Image>().enabled = false;
    }

    public void OnBeginDrag(PointerEventData point)
    {
        PlayerManager.instance.onAttack = true;
        //player.StartCoroutine("Shoot");
    }

}
