using UnityEngine;
using System.Collections;

public class Crowd : MonoBehaviour
{
    [Tooltip("Define the applause sound.")]
    public AudioClip applauseSound;

    [Tooltip("Define the celebration sounds.")]
    public AudioClip[] celebrationSounds;

    [Tooltip("Define the yelling sounds.")]
    public AudioClip[] yellingSounds;

    private AudioSource crowdAudioSource;
    private AudioSource effectsAudioSource;

    private int celebrationIndex = 0;
    private int yellingIndex = 0;

    void Awake()
    {
        crowdAudioSource = gameObject.AddComponent<AudioSource>();
        crowdAudioSource.loop = false;
        crowdAudioSource.volume = 0.3f;
        crowdAudioSource.priority = 128;

        effectsAudioSource = gameObject.AddComponent<AudioSource>();
        effectsAudioSource.loop = false;
        effectsAudioSource.volume = 0.6f;
        effectsAudioSource.priority = 128;
    }

    void Start()
    {
        crowdAudioSource.PlayOneShot( applauseSound );
    }

    void ApplyState( string state )
    {
    }

    void OnTriggerExit( Collider collider )
    {
        if( collider.gameObject.name == "OVRCameraRig" )
        {
            effectsAudioSource.PlayOneShot(yellingSounds[yellingIndex++]);
            if( yellingIndex == yellingSounds.Length ) yellingIndex = 0;
        }
    }
}
