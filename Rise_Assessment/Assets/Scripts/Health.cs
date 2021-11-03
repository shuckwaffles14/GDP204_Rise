using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Make sure to set bool to true if using values in editor")]
    [SerializeField]
    bool useEditorValue;
    [SerializeField]
    [Tooltip("Set value to something close to time it takes audio clip to play")]
    float destructionTimer; // set value to something close to time it takes sound to play
    [SerializeField]
    float replenishmentValue;

    private void Awake()
    {
        if (!useEditorValue)
        {
            destructionTimer = 1.0f;
            replenishmentValue = 25.0f;
        }
    }

    // no longer necessary because collider changed to trigger
    //public void OnCollisionEnter2D(Collision2D collision)
    //{
    //    GetComponent<AudioSource>().Play();
    //    Destroy(gameObject, destructionTimer);// the noise plays nicely but if i add this it destroys it before the noise plays T ^ T
    //    if (collision.gameObject.tag == "Player") // player object has tag set to player in editor
    //    {
    //        collision.gameObject.GetComponent<PlayerController>().AddHealth(replenishmentValue);
    //    }
    //}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<AudioSource>().Play();
        Destroy(gameObject, destructionTimer);// the noise plays nicely but if i add this it destroys it before the noise plays T ^ T
        if (collision.gameObject.tag == "Player") // player object has tag set to player in editor
        {
            collision.gameObject.GetComponent<PlayerController>().AddHealth(replenishmentValue);
        }
    }
}
