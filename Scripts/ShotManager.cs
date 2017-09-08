using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShotManager : MonoBehaviour {

    Vector2 mOrigin;
    bool OnDrag_Shot,shotReady = true;
    GameObject Canvas;

    public GameObject MissilePrefab;
    GameObject Player;

    public GameObject Circle, Circle_Red;
    int circleWay = 0;

    // Use this for initialization
    void Start ()
    {
        Player = GameObject.Find("PlayerBall");
        Canvas = GameObject.Find("Canvas");

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) => { this.eFireDown(); });
        Canvas.transform.Find("ZoneRight").gameObject.GetComponent<EventTrigger>().triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((eventData) => { this.eFireUp(); });
        Canvas.transform.Find("ZoneRight").gameObject.GetComponent<EventTrigger>().triggers.Add(entry);
    }

    // Update is called once per frame
    void Update()
    {
        if (OnDrag_Shot)
        {
            if (Vector2.Distance(mOrigin, (Vector2)Input.mousePosition) >= 50)
            {
                float degree =
                   Mathf.Atan2(mOrigin.y - Input.mousePosition.y,
                   mOrigin.x - Input.mousePosition.x);

                degree = degree * 180 / Mathf.PI;
                degree += 180;

                Circle_Red.SetActive(true);
                if (degree > 45 && degree <= 135)
                {
                    Circle_Red.transform.localPosition = new Vector3(0, 0, 0) + Vector3.up * 75;
                    circleWay = 1;
                }
                else if (degree > 135 && degree <= 225)
                {
                    Circle_Red.transform.localPosition = new Vector3(0, 0, 0) + Vector3.left * 75;
                    circleWay = 2;
                }
                else if (degree > 225 && degree <= 315)
                {
                    Circle_Red.transform.localPosition = new Vector3(0, 0, 0) + Vector3.down * 75;
                    circleWay = 3;
                }
                else
                {
                    Circle_Red.transform.localPosition = new Vector3(0, 0, 0) + Vector3.right * 75;
                    circleWay = 4;
                }

                Circle_Red.transform.rotation = Quaternion.Euler(0, 0, -45 + circleWay * 90);
            }
            else
            {
                circleWay = 0;
                Circle_Red.SetActive(false);
            }

        }
    }

    public void eFireDown()
    {
        mOrigin = Input.mousePosition;
        OnDrag_Shot = true;
        Debug.Log(mOrigin);

        if(shotReady)
        {
            StartCoroutine(ShotMissile());
        }

        Circle.SetActive(true);
        Circle.transform.position = Input.mousePosition;
    }
    public void eFireUp()
    {
        OnDrag_Shot = false;
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

    IEnumerator ShotMissile()
    {
        for (;OnDrag_Shot;)
        {
            shotReady = false;
            GameObject newMissile = Instantiate(MissilePrefab) as GameObject;
            newMissile.transform.position = Player.transform.position;
            newMissile.GetComponent<Missile>().isRight = Player.GetComponent<PlayerManager>().isFaceRight; 
            yield return new WaitForSeconds(0.3f);
            shotReady = true;
        }
    }
}
