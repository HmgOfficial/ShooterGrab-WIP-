using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickMovement : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public Canvas canvas_HUD;
    Vector3 inicial_Pos;
    public float radio;
    private Vector2 axis;
    public Vector2 Axis
    {
        get
        {
            return axis;
        }
    }
    public float Horizontal
    {
        get
        {
            return axis.x;
        }
    }
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
        inicial_Pos = transform.position;
        /*if (IsMoving)
        {

        }*/
    }

    public void OnDrag(PointerEventData point)
    {
        Vector2 joystick_pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas_HUD.transform as RectTransform, point.position, canvas_HUD.worldCamera, out joystick_pos);
        Vector3 newPosition = canvas_HUD.transform.TransformPoint(joystick_pos) - inicial_Pos;
        newPosition.x = Mathf.Clamp(newPosition.x, -radio, radio);
        newPosition.y = Mathf.Clamp(newPosition.y, -radio, radio);

        axis = newPosition / radio;

        transform.localPosition = newPosition;
    }

    public void OnEndDrag(PointerEventData point)
    {
        transform.position = inicial_Pos;
        axis = Vector2.zero;
        //GetComponentInParent<Image>().enabled = false;
        //GetComponent<Image>().enabled = false;
    }

}
