using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public GameObject tpPoint;
    public float waterDamage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = tpPoint.transform.position;
            collision.gameObject.GetComponent<PlayerController>().DoDamage(waterDamage);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<AIController>().DoDamage(collision.gameObject.GetComponent<AIController>().GetHealth()); // do remainder of enemies health as damage if it falls in water
        }
    }
}
