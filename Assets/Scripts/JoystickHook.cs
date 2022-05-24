using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickHook : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Canvas canvas_HUD;
    Vector3 inicial_Pos;
    public float radio;
    public Hook hook;
    //public LineRenderer hooklr;
    public Shader lineShader;
    public GameObject prefabHook;
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
        if (hook != null)
        {
            Vector2 joystick_pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas_HUD.transform as RectTransform, point.position, canvas_HUD.worldCamera, out joystick_pos);
            Vector3 newPosition = canvas_HUD.transform.TransformPoint(joystick_pos) - inicial_Pos;
            newPosition.x = Mathf.Clamp(newPosition.x, -radio, radio);
            newPosition.y = Mathf.Clamp(newPosition.y, -radio, radio);

            axis = newPosition / radio;

            transform.localPosition = newPosition;
            Vector3 hookDirection = transform.position - inicial_Pos;
            hook.transform.forward = new Vector3(hookDirection.x - PlayerManager.instance.transform.position.x, hook.transform.position.y, hookDirection.y - PlayerManager.instance.transform.position.y);
            hookDirection.z = hookDirection.y;
            hookDirection.y = 0.5f;
            DrawLine(PlayerManager.instance.transform.position, hookDirection.normalized * 10, Color.red, Time.deltaTime * 6);
        }

    }

    public void OnEndDrag(PointerEventData point)
    {
        if (hook != null)
        {
            hook.GetComponentInChildren<SphereCollider>().isTrigger = false;
            PlayerManager.instance.rb.velocity = Vector3.zero;
            Vector3 hookDirection = transform.position - inicial_Pos;
            //hook.MoveToInitialPos();
            //hook.transform.position = player.transform.position;
            hook.GrapplingShot(hookDirection.x, hookDirection.y);
            transform.position = inicial_Pos;
            axis = Vector2.zero;

        }
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
    {
        start.y = 0.5f;
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(lineShader);
        //lr.SetColors(color, color);
        lr.startColor = color;
        lr.endColor = color;
        //lr.SetWidth(0.1f, 0.1f);
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }

    public void OnBeginDrag(PointerEventData point)
    {
        if (hook == null)
        {
            GameObject clon = Instantiate(prefabHook, PlayerManager.instance.transform,false);
            clon.transform.rotation = PlayerManager.instance.transform.rotation;
            clon.name = "Hook";
            hook = clon.GetComponent<Hook>();
            hook.GetComponentInChildren<SphereCollider>().isTrigger = true;
        }
    }
}
