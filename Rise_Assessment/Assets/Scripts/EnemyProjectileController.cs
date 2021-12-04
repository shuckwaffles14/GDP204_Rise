using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{    
    public GameObject AI;
    public float projectileSpeed;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(null);
        Vector2 target = AI.GetComponent<AIController>().GetCurrentTarget();
        Vector2 AILocation = AI.transform.position;
        if (target.x > AILocation.x) GetComponent<Rigidbody2D>().velocity = (target - AILocation).normalized * projectileSpeed;
        if (target.x < AILocation.x) GetComponent<Rigidbody2D>().velocity = (target - AILocation).normalized * projectileSpeed; // *** NEED TO TEST THIS PART OF CODE, AI WOULD NOT LOOK LEFT IN TESTING
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Collisions" || collision.gameObject.tag == "Projectile")
        {
            Destroy(this.gameObject);
            //Debug.Log("Projectile hit wall");
        }

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().DoDamage(AI.GetComponent<AIController>().GetAttackDamage());
            collision.gameObject.GetComponent<PlayerController>().Knockback(AI.GetComponent<AIController>().GetPos(), AI.GetComponent<AIController>().GetKnockbackForce() * 125);
            Destroy(this.gameObject);
        }
    }
}
