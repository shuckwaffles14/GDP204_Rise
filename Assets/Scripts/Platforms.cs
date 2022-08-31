using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    [SerializeField]
    private Transform pos1, pos2;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float waitTimer;


    private Vector3 nextPos;
    private Vector2 velocity;
    // Start is called before the first frame update
    void Start()
    {
        nextPos = pos1.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Platform update");
        if (transform.position == pos1.position)
        {
            //Debug.Log("Move to pos 2");
            nextPos = pos2.position;
        }
        if (transform.position == pos2.position)
        {
            //Debug.Log("Move to pos 1");
            nextPos = pos1.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pos1.position, pos2.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var groundCheckerTransform = collision.transform.GetChild(0);
            if (transform.position.y > groundCheckerTransform.transform.position.y) // if over the player
            {
                Debug.Log("Entering collision");
                BoxCollider2D thisbc2d = GetComponent<BoxCollider2D>();
                thisbc2d.isTrigger = true;
            }
            
            if (transform.position.y < collision.transform.position.y) //if under the player
            {
                collision.gameObject.GetComponent<PlayerController>().CheckForGround();
                //collision.gameObject.GetComponent<PlayerController>().SetOnGround(true);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var groundCheckerTransform = collision.transform.GetChild(0);
            if (transform.position.y > groundCheckerTransform.transform.position.y)
            {
                Debug.Log("Ongoing collision");
                //BoxCollider2D thisbc2d = GetComponent<BoxCollider2D>();
                //Physics2D.IgnoreCollision(thisbc2d, collision.gameObject.GetComponent<BoxCollider2D>());
            }
            
            if (transform.position.y < collision.transform.position.y)
            {
                collision.gameObject.GetComponent<PlayerController>().CheckForGround();
                collision.transform.position = Vector3.MoveTowards(collision.transform.position, nextPos, speed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Exit Collision");
            BoxCollider2D thisbc2d = GetComponent<BoxCollider2D>();
            thisbc2d.isTrigger = false;
        }
    }
}
