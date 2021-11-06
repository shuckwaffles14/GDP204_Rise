using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockAudioPlayer : MonoBehaviour
{
    public AudioClip impact;
    public float volume;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "projectile") audioSource.PlayOneShot(impact, volume); // if hit by projectile, damage rock
    }
}
