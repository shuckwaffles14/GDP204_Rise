using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<AudioSource>().Play();
        //Destroy(gameObject); the noise plays nicely but if i add this it destroys it before the noise plays T ^ T
    }
}
