using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

    public float isRight = 1;
    float speed = 2f;

	// Use this for initialization
	void Start () {
        StartCoroutine(timeOut());
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 movement = new Vector3(0, 0, 0);
        movement = transform.right * isRight * speed * Time.deltaTime;
        transform.position = transform.position + movement;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.transform.tag.Equals("Walls"))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator timeOut()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
