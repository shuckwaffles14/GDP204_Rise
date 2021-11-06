using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    public float fireballSpeed;
    public float fireballRange;
    public GameObject player;

    private float countdown;

    // Start is called before the first frame update
    void Start()
    {
        countdown = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
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
            Debug.Log("Fireball damage: " + player.GetComponent<PlayerController>().GetFireballDamage());
            collision.gameObject.GetComponent<AIController>().DoDamage(player.GetComponent<PlayerController>().GetFireballDamage());
            Debug.Log("Fireball knockback force: " + player.GetComponent<PlayerController>().GetKnockbackForce() * 125);
            collision.gameObject.GetComponent<AIController>().Knockback(player.GetComponent<PlayerController>().GetPos(), player.GetComponent<PlayerController>().GetKnockbackForce() * 125);
            Debug.Log("Fireball hit AI");
            Destroy(this.gameObject);
        }
    }
}