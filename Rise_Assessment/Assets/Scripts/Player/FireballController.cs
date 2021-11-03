using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    public float fireballSpeed;
    public float fireballRange;
    public GameObject player;

    private Vector3 originPoint;
    private Vector2 playerTarget;
    private bool leftOrRight; // true = facing right
    private bool atTarget;
    private Transform t;
    private Rigidbody2D rb2d;
    private float countdown;

    // Start is called before the first frame update
    void Start()
    {
        countdown = 5.0f;
        //t = transform;
        //rb2d = GetComponent<Rigidbody2D>();
        //originPoint = t.position;
        //playerTarget = player.GetComponent<PlayerController>().FireballTarget();
        //leftOrRight = player.GetComponent<PlayerController>().FireballDirection();
        //if (leftOrRight) GetComponent<SpriteRenderer>().flipX = true;
        //else GetComponent<SpriteRenderer>().flipX = false;
        //if (playerTarget == null)
        //{
        //    if (leftOrRight) //if facing right
        //    {
        //        playerTarget.x = originPoint.x + fireballRange;
        //        playerTarget.y = originPoint.y;
        //    }
        //    else //if facing left
        //    {
        //        playerTarget.x = originPoint.x - fireballRange;
        //        playerTarget.y = originPoint.y;
        //    }
        //}
        //atTarget = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!atTarget)
        //{
        //    rb2d.velocity = playerTarget * fireballSpeed;
        //}
        if (countdown <= 0.0f)
        {
            Destroy(this);
        }
        countdown -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Collisions")
        {
            GameObject.Destroy(this.gameObject);
            Debug.Log("Fireball hit wall");
        }

        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<AIController>().DoDamage(player.GetComponent<PlayerController>().GetFireballDamage());
            collision.gameObject.GetComponent<AIController>().Knockback(player.GetComponent<PlayerController>().GetPos(), player.GetComponent<PlayerController>().GetKnockbackForce() * 10);
            Debug.Log("Fireball hit AI");
            Destroy(this.gameObject);
        }
    }
}