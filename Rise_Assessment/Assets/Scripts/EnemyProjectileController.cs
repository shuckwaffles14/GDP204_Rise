using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    private GameObject player;
    
    public GameObject AI;
    public float projectileSpeed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<Rigidbody2D>().velocity = player.transform.position * projectileSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Collisions")
        {
            GameObject.Destroy(this.gameObject);
            Debug.Log("Projectile hit wall");
        }

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().DoDamage(AI.GetComponent<AIController>().GetAttackDamage());
            collision.gameObject.GetComponent<PlayerController>().Knockback(AI.GetComponent<AIController>().GetPos(), AI.GetComponent<AIController>().GetKnockbackForce() * 125);
            Destroy(this.gameObject);
        }
    }
}
