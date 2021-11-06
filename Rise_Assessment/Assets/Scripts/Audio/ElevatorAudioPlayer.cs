using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorAudioPlayer : MonoBehaviour
{
    public AudioClip elevatorNoise;
    public float volume;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") audioSource.PlayOneShot(elevatorNoise, volume);
    }
}
