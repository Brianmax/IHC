using UnityEngine;
using System.Collections;

public class Audience : MonoBehaviour
{
    [Tooltip("Define the applause sound.")]
    public AudioClip applauseSound;

    [Tooltip("Define the celebration sounds.")]
    public AudioClip[] celebrationSounds;

    [Tooltip("Define the yelling sounds.")]
    public AudioClip[] yellingSounds;

    private AudioSource audienceAudioSource;
    private AudioSource effectsAudioSource;

    private bool reactToWaveDone = false;

    void Start()
    {
        audienceAudioSource = gameObject.AddComponent<AudioSource>();
        audienceAudioSource.loop = false;
        audienceAudioSource.volume = 0.2f;
        audienceAudioSource.priority = 256;
        audienceAudioSource.minDistance = 25;
    }

    void ResponseTo(string action)
    {
        switch(action)
        {
            case "PlayerIsWaving":
                {
                    if(!reactToWaveDone)
                    {
                        audienceAudioSource.PlayOneShot(applauseSound);

                        // Send a message to all spectators
                        foreach (Spectator spectator in GetComponentsInChildren<Spectator>())
                        {
                            //Spectator spectator = spectatorTransform.GetComponent<Spectator>();
                            spectator.SendMessage("ChangeAnimation", "Applause Loop");
                        }

                        reactToWaveDone = true;
                    }

                    break;
                }

            case "PlayerIsOutOfDivingBoardBounds":
                {
                    if(yellingSounds.Length > 0)
                    {
                        int yellingSoundIndex = (int)Random.Range(0, yellingSounds.Length - 1);
                        audienceAudioSource.PlayOneShot(yellingSounds[yellingSoundIndex]);
                    }

                    break;
                }

            case "PlayerEmerged":
                {
                    audienceAudioSource.clip = applauseSound;
                    audienceAudioSource.Play();

                    if(celebrationSounds.Length > 0)
                    {
                        int celebrationSoundIndex = (int)Random.Range(0, celebrationSounds.Length - 1);
                        audienceAudioSource.PlayOneShot(yellingSounds[celebrationSoundIndex]);
                    }

                    break;
                }

            default:
                break;
        }
    }
}
