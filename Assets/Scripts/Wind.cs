using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour {

    [Tooltip("Define the wind sound.")]
    public AudioClip windSound;

    private AudioSource windAudioSource;
    private bool isActivatedByTrigger = false;

    void Awake()
    {
        if(GetComponent<Collider>())
            isActivatedByTrigger = true;
    }

    void Start()
    {
        windAudioSource = gameObject.AddComponent<AudioSource>();
        windAudioSource.loop = true;
        windAudioSource.volume = 0.2f;
        windAudioSource.priority = 256;
        windAudioSource.clip = windSound;

        if(!isActivatedByTrigger)
            windAudioSource.Play();
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.GetComponentInChildren<AudioListener>())
            windAudioSource.Play();
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.GetComponentInChildren<AudioListener>())
            windAudioSource.Stop();
    }
}