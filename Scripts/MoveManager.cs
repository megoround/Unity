using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveManager : MonoBehaviour
{
    bool ready = true, fireReady = true;
    public bool jumpAble = false;
    public float isFaceRight = 1;

    public float speed = 0.25f, jump = 10f;
    Ray2D moveRay; RaycastHit2D moveHit;
    int blockMask;

    GameObject PlayerBall;

    Vector2 mOrigin;
    bool OnDrag_Move;
    GameObject Canvas;
    int circleWay;

    public GameObject Circle, Circle_Blue;

    // Use this for initialization
    void Start () {

        blockMask = LayerMask.GetMask("blockAble");
        PlayerBall = GameObject.Find("PlayerBall");
        Canvas = GameObject.Find("Canvas");

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) => { this.eMoveDown(); });
        Canvas.transform.Find("ZoneLeft").gameObject.GetComponent<EventTrigger>().triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((eventData) => { this.eMoveUp(); });
        Canvas.transform.Find("ZoneLeft").gameObject.GetComponent<EventTrigger>().triggers.Add(entry);

    }

    // Update is called once per frame
    void Update () {

        if (OnDrag_Move)
        {
            if (Vector2.Distance(mOrigin, (Vector2)Input.mousePosition) >= 50)
            {
                float degree =
                   Mathf.Atan2(mOrigin.y - Input.mousePosition.y,
                   mOrigin.x - Input.mousePosition.x);

                degree = degree * 180 / Mathf.PI;
                degree += 180;

                Circle.transform.rotation = Quaternion.Euler(0, 0, -90 + degree);

                Circle_Blue.SetActive(true);
                if (degree > 40 && degree <= 140)
                {
                    circleWay = 1;

                    if (jumpAble)
                    {
                        PlayerBall.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 10, 0);
                        PlayerBall.GetComponent<Rigidbody2D>().AddForce(transform.up * jump);
                        
                        jumpAble = false;
                    }
                }
                if (degree > 120 && degree <= 240)
                {
                    circleWay = 2;

                    if (ready)
                    {
                        StartCoroutine(Move(-1));
                        isFaceRight = -1;
                    }
                }
                else if (degree > 225 && degree <= 315)
                {
                    circleWay = 3;
                }
                if (degree > 300 || degree <= 60)
                {
                    circleWay = 4;

                    if (ready)
                    {
                        StartCoroutine(Move(1));
                        isFaceRight = 1;
                    }
                }
            }
            else
            {
                circleWay = 0;
                Circle_Blue.SetActive(false);
            }

        }
    }

    public void eMoveDown()
    {
        mOrigin = Input.mousePosition;
        OnDrag_Move = true;
        Debug.Log(mOrigin);

        Circle.SetActive(true);
        Circle.transform.position = Input.mousePosition;
    }
    public void eMoveUp()
    {
        OnDrag_Move = false;
        Debug.Log((Vector2)Input.mousePosition);

        switch (circleWay)
        {
            case 0: break;
            case 1: Debug.Log("1"); break;
            case 2: Debug.Log("22"); break;
            case 3: Debug.Log("333"); break;
            case 4: Debug.Log("4444"); break;
        }

        Circle.SetActive(false);
    }


    IEnumerator Move(float h)
    {
        ready = false;
        Vector3 movement = new Vector3(0, 0, 0);

        movement = PlayerBall.transform.right * h * speed;

        moveRay.origin = PlayerBall.transform.position;
        moveRay.direction = movement;

        if (!Physics2D.BoxCast(PlayerBall.transform.position, new Vector2(0.1f, 0.9f), 0, Vector2.right * h, speed, blockMask))
        {
            PlayerBall.transform.position = PlayerBall.transform.position + movement;
        }

        yield return new WaitForSeconds(0.03f);
        ready = true;

    }
}
