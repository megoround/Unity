using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchManager : MonoBehaviour {

    //All
    GameObject PlayerBall;
    GameObject Canvas;
    
    int MoveTouchNumber = -1;
    int FireTouchNumber = -1;

    //Move
    bool moveReady = true; // 이동 텀
    public bool jumpAble = false;
    public float isFaceRight = 1;

    public float speed = 0.25f, jump = 10f;
    Ray2D moveRay; RaycastHit2D moveHit;
    int blockMask;

    public GameObject MoveCircle;
     GameObject MoveCircle_Blue;
    int MoveCircleWay;
    bool OnDrag_Move = false;

    Vector2 mOrigin_Move; //터치 시작점

    //Fire, 
    bool fireReady = true;
    public GameObject MissilePrefab;

    public GameObject FireCircle;
     GameObject FireCircle_Red;
    int FireCircleWay;
    bool OnDrag_Fire = false;

    Vector2 mOrigin_Fire; //터치 시작점

    // Use this for initialization
    void Start ()
    {
         PlayerBall = GameObject.Find("PlayerBall");
         Canvas = GameObject.Find("Canvas");

        blockMask = LayerMask.GetMask("blockAble");

        MoveCircle_Blue = MoveCircle.GetComponent<Transform>().Find("Blue").gameObject;
        FireCircle_Red = FireCircle.GetComponent<Transform>().Find("Red").gameObject;

        //이벤트 부여
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) => { this.eMoveDown(); });
        Canvas.transform.Find("ZoneLeft").gameObject.GetComponent<EventTrigger>().triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((eventData) => { this.eMoveUp(); });
        Canvas.transform.Find("ZoneLeft").gameObject.GetComponent<EventTrigger>().triggers.Add(entry);

        //entry = new EventTrigger.Entry();
        //entry.eventID = EventTriggerType.PointerDown;
        //entry.callback.AddListener((eventData) => { this.eFireDown(); });
        //Canvas.transform.Find("ZoneRight").gameObject.GetComponent<EventTrigger>().triggers.Add(entry);

        //entry = new EventTrigger.Entry();
        //entry.eventID = EventTriggerType.PointerUp;
        //entry.callback.AddListener((eventData) => { this.eFireUp(); });
        //Canvas.transform.Find("ZoneRight").gameObject.GetComponent<EventTrigger>().triggers.Add(entry);

    }

    // Update is called once per frame
    void Update () {
        //Move();
	}


    //events

    void eMoveDown()
    {
        if (!OnDrag_Move) // 다른 movetouch중엔 무시
        {
            MoveTouchNumber = Input.touchCount - 1;
            //가장 최신의 터치가 현재 터치


            mOrigin_Move = Input.touches[MoveTouchNumber].position; // touch의 원점 기억

            GameObject.Find("DebugText").GetComponent<Text>().text = ""+Input.touches[MoveTouchNumber].position;

            OnDrag_Move = true;

            MoveCircle.SetActive(true); // 원판 표시
            MoveCircle.transform.position = Input.touches[MoveTouchNumber].position;

        }
    }
    void eMoveUp()
    {
        OnDrag_Move = false;
        MoveCircle.SetActive(false);
    }
    void eFireDown()
    {

    }
    void eFireUp()
    {

    }

    //

    void Move()
    {
        if (OnDrag_Move)
        {
            Vector2 Start = mOrigin_Move; // 원점
            Vector2 End = Input.touches[MoveTouchNumber].position; // 현재점
            
            //Input.mousePosition을 MoveTouchNumber의 mouseposition으로 교체해야함 -> 함
            if (Vector2.Distance(Start, End) >= 50)
            {
                float degree =
                   Mathf.Atan2(Start.y - End.y,
                   Start.x - End.x);

                degree = degree * 180 / Mathf.PI;
                degree += 180;

                MoveCircle.transform.rotation = Quaternion.Euler(0, 0, -90 + degree);

                MoveCircle_Blue.SetActive(true);
                if (degree > 35 && degree <= 145) // 상단(점프)
                {
                    MoveCircleWay = 1;
                    DoJump();
                }
                if (degree > 120 && degree <= 240) // 좌측
                {
                    MoveCircleWay = 2;

                    if (moveReady)
                    {
                        StartCoroutine(Move(-1));
                        isFaceRight = -1;
                    }
                }
                if (degree > 300 || degree <= 60) // 우측
                {
                    MoveCircleWay = 4;

                    if (moveReady)
                    {
                        StartCoroutine(Move(1));
                        isFaceRight = 1;
                    }
                }
                if (degree > 225 && degree <= 315) // 하단
                {
                    MoveCircleWay = 3;
                }
            }
            else
            {
                MoveCircleWay = 0;
                MoveCircle_Blue.SetActive(false);
            }

        }
    }
    void DoJump()
    {
        if (jumpAble)
        {
            PlayerBall.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 10, 0);
            PlayerBall.GetComponent<Rigidbody2D>().AddForce(transform.up * jump);

            jumpAble = false;
        }
    }

    IEnumerator Move(float h)
    {
        moveReady = false;
        Vector3 movement = new Vector3(0, 0, 0);

        movement = PlayerBall.transform.right * h * speed;

        //Raycast
        moveRay.origin = PlayerBall.transform.position;
        moveRay.direction = movement;

        if (!Physics2D.BoxCast(PlayerBall.transform.position, new Vector2(0.1f, 0.9f), 0, Vector2.right * h, speed, blockMask))
        {
            PlayerBall.transform.position = PlayerBall.transform.position + movement;
        }

        yield return new WaitForSeconds(0.03f); //이동 텀, 1/30f
        moveReady = true;

    }
}
