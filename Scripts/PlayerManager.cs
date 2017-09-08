using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    bool ready = true, fireReady = true;
    public bool jumpAble = false, isJump = false;

    float h = 0, v = 0;
    public float isFaceRight = 1;

    public float speed = 0.25f, jump = 10f;
    Ray2D moveRay; RaycastHit2D moveHit;
    int blockMask;

    GameObject PlayerBall;

    /// //////////////////////////////
    int temp = 0;


    // Use this for initialization
    void Start ()
    {
        blockMask = LayerMask.GetMask("blockAble");
        PlayerBall = GameObject.Find("PlayerBall");
    }

    // Update is called once per frame

    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        if (ready && h != 0)
        {
            StartCoroutine(Move(h));
            isFaceRight = h;
        }

        if (v == 1 && jumpAble)
        {
            PlayerBall.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 10, 0);
            PlayerBall.GetComponent<Rigidbody2D>().AddForce(transform.up * jump);
            Debug.Log("JUMP : " + temp++);
            jumpAble = false;
        }
    }
    
    IEnumerator Move(float h)
    {
        ready = false;
        Vector3 movement = new Vector3(0, 0, 0);

            movement = PlayerBall.transform.right * h * speed;

            moveRay.origin = PlayerBall.transform.position;
            moveRay.direction = movement;

            if(!Physics2D.BoxCast(PlayerBall.transform.position, new Vector2(0.1f,0.9f), 0, Vector2.right * h, speed, blockMask))
            {
                PlayerBall.transform.position = PlayerBall.transform.position + movement;
            }

        yield return new WaitForSeconds(0.03f);
        ready = true;

    }
}

