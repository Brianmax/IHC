using UnityEngine;
using System.Collections;

public class DivingPoolWater : MonoBehaviour {

    [Tooltip("Define the water sound when the Audio Listener is above the water.")]
    public AudioClip waterSound;

    [Tooltip("Define the water sound when the Audio Listener is under the water.")]
    public AudioClip underwaterSound;

    private AudioSource waterAudioSource;

    void Awake()
    {
        // Create and configure an audio source to play the water sound.
        waterAudioSource = gameObject.AddComponent<AudioSource>();
        waterAudioSource.loop = true;
        waterAudioSource.volume = 0.3f;
    }

	void Start()
    {
        waterAudioSource.clip = waterSound;
        waterAudioSource.Play();
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.GetComponentInChildren<AudioListener>())
        {
            waterAudioSource.clip = underwaterSound;
            waterAudioSource.Play();
        }        
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.GetComponentInChildren<AudioListener>())
        {
            waterAudioSource.clip = waterSound;
            waterAudioSource.Play();    
        }
    }
}
