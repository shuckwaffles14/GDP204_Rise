using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ding : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<AudioSource>().Play();

    }
}
