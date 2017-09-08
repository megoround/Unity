using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBottomManager : MonoBehaviour {

    GameObject Player;

    // Use this for initialization
    void Start() {
        Player = GameObject.Find("PlayerBall");
    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("Enter1");
        if (coll.tag.Equals("Walls"))
        {
            Debug.Log("Enter");
            //Player.GetComponent<PlayerManager>().jumpAble = true;
            GameObject.Find("TouchManager").GetComponent<TouchManager>().jumpAble = true;
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag.Equals("Walls"))
        {
            Debug.Log("Exit");
            //Player.GetComponent<PlayerManager>().jumpAble = false;
            GameObject.Find("TouchManager").GetComponent<TouchManager>().jumpAble = false;
        }
    }
}
