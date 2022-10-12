using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

public class Player : MonoBehaviour
{
    [Tooltip("Define the player breathing sound.")]
    public AudioClip breathingSound;

    [Tooltip("Define the player normal hearbeat sound.")]
    public AudioClip normalHeartbeatSound;

    [Tooltip("Define the sound when the player is moving under the water.")]
    public AudioClip bubblingSound;

    [Tooltip("Define the listener which detects the player gesture")]
    public PlayerGestureListener playerGestureListener;

    [Tooltip("Define the controller which moves the player")]
    public AvatarControllerClassic avatarControllerClassic;

    // Reference to the Kinect Manager
    public KinectManager kinectManager;

    private PathFollower pathFollower;

    private AudioSource breathingAudioSource;
    private AudioSource heartbeatAudioSource;
    private AudioSource effectsAudioSource;

    private Vector3 swimmingDirection;
    public float maxSwimmingSpeed = 30.0f;
    private float currentSwimmingSpeed;
    private int swimmingFrameIndex = 0;
    private int numSwimmingFrames = 20;

    private int rigidbodyDragForce = 0;

    private long userID = 0;

	private SkinnedMeshRenderer[] skinnedMeshRenderers;

	private Vector3 restorePosition;

    enum States
    {
        Walking,
        ReadyToDive,
        Diving,
        ReadyToSwim,
        Swimming,
        Resting
    };

    private States state;

    public enum DivingGestures
    {
        NoGesture,
        HandsUp
    }

    public DivingGestures divingGesture;

    void Awake()
	{
		// Force external root motion
		avatarControllerClassic.externalRootMotion = true;

		pathFollower = GetComponentInChildren<PathFollower> ();

		breathingAudioSource = gameObject.AddComponent<AudioSource> ();
		breathingAudioSource.clip = breathingSound;
		breathingAudioSource.loop = true;
		breathingAudioSource.volume = 1.0f;
		breathingAudioSource.priority = 0;
		breathingAudioSource.Play ();

		heartbeatAudioSource = gameObject.AddComponent<AudioSource> ();
		heartbeatAudioSource.clip = normalHeartbeatSound;
		heartbeatAudioSource.loop = true;
		heartbeatAudioSource.volume = 0.015f;
		heartbeatAudioSource.priority = 1;
		heartbeatAudioSource.Play ();

		effectsAudioSource = gameObject.AddComponent<AudioSource> ();
		effectsAudioSource.loop = true;
		effectsAudioSource.volume = 0.08f;
		effectsAudioSource.priority = 1;

		restorePosition = transform.position;
	}

	void Start()
	{
        GetComponent<Rigidbody>().drag = 0;
        Camera.main.GetComponent<ScreenFade>().FadeIn (5.0f);
    }

    void Update()
    {
		// Reload the scene
		if (Input.GetKeyDown("r"))
		{
			int scene = SceneManager.GetActiveScene().buildIndex;
			SceneManager.LoadScene(scene, LoadSceneMode.Single);

			state = States.Walking;
			divingGesture = DivingGestures.NoGesture;
		}

		if (Input.GetKeyDown ("c"))
		{
			UnityEngine.XR.InputTracking.Recenter();
		}

		if (kinectManager.GetUsersCount () == 0) {
			//transform.position = restorePosition;
			return;
		}
			
		userID = kinectManager.GetPrimaryUserID ();
		restorePosition = transform.position;
        Debug.Log("update avatar"+userID);
        switch (state)
        {
            case States.Walking:
                {
                    Vector3 userPosition = kinectManager.GetUserPosition(userID);
                    transform.position = new Vector3(userPosition.x, transform.position.y, -userPosition.z);

                    if (playerGestureListener.IsWave() || playerGestureListener.isRaiseLeftHand() || playerGestureListener.isRaiseRightHand())
                    {
                        // TODO: Fazer Raycast do foward da camera com a plateia e animar somente a plateia que o avatar olhou e acenou

                        // Send a message to all audiences
                        foreach (GameObject audience in GameObject.FindGameObjectsWithTag("Audience"))
                        {
                            audience.SendMessage("ResponseTo", "PlayerIsWaving");
                        }
                    }

                    break;
                }

            case States.ReadyToDive:
                {
                    Vector3 userPosition = kinectManager.GetUserPosition(userID);
                    transform.position = new Vector3(userPosition.x, transform.position.y, -userPosition.z);

                    if (playerGestureListener.IsHandsUp())
                    {
                        pathFollower.enabled = true;

                        state = States.Diving;
                        divingGesture = DivingGestures.HandsUp;
                    }

                    // TODO: Implementar outros gestos

                    break;
                }

            case States.Diving:
                {
                    playerGestureListener.enabled = false;

                    if (pathFollower != null)
                    {
                        if (pathFollower.isComplete())
                        {
                            pathFollower.enabled = false;
                            // TODO: Colocar instrução de nadar e contar tempo embaixo dagua
                        }
                    }
                    break;
                }

            case States.ReadyToSwim:
                {
                    if (playerGestureListener.IsSwimming() || playerGestureListener.IsSwipeDown())
                    {
                        currentSwimmingSpeed = maxSwimmingSpeed;
                        swimmingFrameIndex = 0;

                        //swimmingDirection = riftCamera.transform.TransformDirection(Vector3.forward);
                        swimmingDirection = GameObject.Find("Diving Pool Exit Target").transform.position - transform.position;
                        swimmingDirection.Normalize();

                        state = States.Swimming;

                        GameObject.Find("Left Hand Bubbles").GetComponent<ParticleSystem>().Play();
                        GameObject.Find("Right Hand Bubbles").GetComponent<ParticleSystem>().Play();
                        GetComponent<Rigidbody>().drag = 5;
                        Physics.gravity = new Vector3( 0, -1.2f, 0 );
                    }

                    break;
                }

            case States.Swimming:
                {
                    if (swimmingFrameIndex < numSwimmingFrames)
                    {
                        //if (swimmingFrameIndex >= numSwimmingFrames / 2.0f)
                        //{
                        //    currentSwimmingSpeed = -Mathf.Pow(swimmingFrameIndex - numSwimmingFrames / 2.0f, 2.0f) + maxSwimmingSpeed;
                        //    if (currentSwimmingSpeed < 0) currentSwimmingSpeed = 0;
                        //}

                        GetComponent<Rigidbody>().AddForce(36*swimmingDirection / Time.fixedDeltaTime);
                        //transform.position = Vector3.Lerp(transform.position, transform.position + swimmingDirection, currentSwimmingSpeed * Time.deltaTime);
                        swimmingFrameIndex++;
                    }
                    else
                    {
                        state = States.ReadyToSwim;

                        GameObject.Find("Left Hand Bubbles").GetComponent<ParticleSystem>().Stop();
                        GameObject.Find("Right Hand Bubbles").GetComponent<ParticleSystem>().Stop();
                    }

                    break;
                }

            case States.Resting:
			{					
				//transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0.0f,1.0f,0.0f,-15.0f), Time.deltaTime * 0.05f);
					
				//GameObject.Find("Left Hand Bubbles").GetComponent<ParticleSystem>().Play();
				//GameObject.Find("Right Hand Bubbles").GetComponent<ParticleSystem>().Play();

				// esperar 10 segundos e dar fade out e aguardar o reset

                    break;
               }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.collider.gameObject.name)
        {
            case ("PiscinaPiso01"):
                {
                    playerGestureListener.enabled = true;
                    playerGestureListener.StopAllGestures();

                    GameObject.Find("Left Hand Bubbles").GetComponent<ParticleSystem>().Stop();
                    GameObject.Find("Right Hand Bubbles").GetComponent<ParticleSystem>().Stop();

                    state = States.ReadyToSwim;
                    break;
                }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        switch (collider.gameObject.name)
        {
            case ("Ready To Dive Trigger"):
                {
                    state = States.ReadyToDive;
                    break;
                }

            case ("Out Of Bounds Trigger"):
                {
                    state = States.Walking;
                    break;
                }

            case ("No Gesture Trigger"):
				{                    
                    divingGesture = DivingGestures.NoGesture;
                    state = States.Diving;
                    break;
                }

            case ("Splashing Surface Trigger"):
                {
                    break;
                }

            case ("Diving Pool Water"):
                {
                    GetComponent<Rigidbody>().drag = 10;

                    GameObject.Find("Bubbles").transform.parent = transform;
                    GameObject.Find("Bubbles").transform.position = transform.position;

                    //GameObject.Find("Bubbles").GetComponent<EllipsoidParticleEmitter>().emit = true;

                    break;
                }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        switch (collider.gameObject.name)
        {
            case ("Out Of Bounds Trigger"):
                {
                    // Send a message to all audiences
                    foreach (GameObject audience in GameObject.FindGameObjectsWithTag("Audience"))
                    {
                        audience.SendMessage("ResponseTo", "PlayerIsOutOfDivingBoardBounds");
                    }

                    break;
                }

			case ("Splashing Surface Trigger"):
                {
                    breathingAudioSource.Stop();
                    heartbeatAudioSource.Stop();

                    effectsAudioSource.clip = bubblingSound;
                    effectsAudioSource.Play();

                    playerGestureListener.StopAllGestures();
                    break;
                }

            case ("Diving Pool Water"):
                {
                    GetComponent<Rigidbody>().useGravity = false;
                    //GameObject.Find("Bubbles").GetComponent<EllipsoidParticleEmitter>().emit = false;
                    GameObject.Find("Left Hand Bubbles").GetComponent<ParticleSystem>().Stop();
                    GameObject.Find("Right Hand Bubbles").GetComponent<ParticleSystem>().Stop();

                    playerGestureListener.StopAllGestures();
                    Destroy(playerGestureListener);
                    Destroy(GetComponentInChildren<KinectGestures>());

                    //GameObject.Find("2nd section").SendMessage("ResponseTo", "PlayerEmerged");
                    breathingAudioSource.Play();
                    heartbeatAudioSource.Play();

                    effectsAudioSource.Stop();
                    state = States.Resting;

                    GameObject[] allFireworks = GameObject.FindGameObjectsWithTag("Fireworks");
                    foreach (GameObject fireworks in allFireworks)
                    {
                        fireworks.GetComponent<ParticleSystem>().Play();
                    }

                    GameObject.Find("Som do tracado").GetComponentInChildren<AudioSource>().Play();
                    GameObject.Find("Som da explosao").GetComponentInChildren<AudioSource>().Play();

					Camera.main.GetComponent<ScreenFade>().FadeOut(15.0f);
					Invoke("Freeze", 15);

                    break;
                }
        }
    }

	void Freeze()
	{
		kinectManager.enabled = false;
		GameObject.Find("Som do tracado").GetComponentInChildren<AudioSource>().Stop();
		GameObject.Find("Som da explosao").GetComponentInChildren<AudioSource>().Stop();
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.up);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right);
    }
}
