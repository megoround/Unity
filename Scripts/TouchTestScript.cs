using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchTestScript : MonoBehaviour
{
    GameObject Canvas;
    public GameObject DebugText_GO;
    Text DebugText;

    // Use this for initialization
    void Start () {

        Canvas = GameObject.Find("Canvas");
        DebugText = DebugText_GO.GetComponent<Text>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) => { this.Down(); });
        Canvas.transform.Find("TouchTestZone").gameObject.GetComponent<EventTrigger>().triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((eventData) => { this.Up(); });
        Canvas.transform.Find("TouchTestZone").gameObject.GetComponent<EventTrigger>().triggers.Add(entry);

    }

    // Update is called once per frame
    void Update () {
    }

    void Down()
    {
        DebugText.text = "";
        for (int i=0;i<Input.touchCount;i++)
        {
            DebugText.text += "\n Touch DOWN : " + i + " : " + Input.touches[i].position;
        }
    }
    void Up()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            DebugText.text += "\n Touch UP : " + i + " : " + Input.touches[i].position;
        }
    }
}
