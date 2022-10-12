using UnityEngine;
using System.Collections;

// AudioTrigger  requires the GameObject to have a BoxCollider component
[RequireComponent(typeof(BoxCollider))]

public class AudioTrigger : MonoBehaviour
{
    [Tooltip("Define the audio clip played when the AudioListener enters the trigger.")]
    public AudioClip audioOnEnter;

    [Tooltip("Define the audio clip played when the AudioListener is touching the trigger.")]
    public AudioClip audioOnStay;

    [Tooltip("Define the audio clip played when the AudioListener has stopped touching the trigger.")]
    public AudioClip audioOnExit;

    public bool loop;
    public float volume;
    
    private BoxCollider boxCollider;
    private AudioSource audioSource;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = loop;
        audioSource.volume = volume;
        audioSource.priority = 256;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (audioOnEnter == null)
            return;

        if (collider.gameObject.GetComponentInChildren<AudioListener>())
        {
            audioSource.clip = audioOnEnter;
            audioSource.Play();
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (audioOnStay == null)
            return;

        if (collider.gameObject.GetComponentInChildren<AudioListener>())
        {
            audioSource.clip = audioOnStay;
            audioSource.Play();
        }
            
    }

    void OnTriggerExit(Collider collider)
    {
        if (audioOnExit == null)
            return;

        if (collider.gameObject.GetComponentInChildren<AudioListener>())
        {
            audioSource.clip = audioOnExit;
            audioSource.Stop();
        }
            
    }
}
