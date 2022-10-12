using UnityEngine;
using System.Collections;

public class SplashTrigger : MonoBehaviour {

    public Transform avatar;
    public ParticleSystem surfaceSplashParticles;
    public ParticleSystem underSurfaceSplashParticles;

    private float feetParticleOffset = -1.1f;
    private float headParticleOffset = 0.1f;
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void OnTriggerEnter( Collider collider )
    {        
        if( collider == avatar.GetComponent<Collider>() )
        {
            Player p = avatar.GetComponent<Player>();
            Transform ankle = GameObject.Find("Ankle Right").transform;
            Transform waterSurface = GameObject.Find("Splashing Surface Trigger").transform; // use surface spash as water surface y position
            surfaceSplashParticles.transform.position = new Vector3(ankle.position.x, waterSurface.position.y + headParticleOffset, ankle.position.z);
            underSurfaceSplashParticles.transform.position = new Vector3(ankle.position.x, waterSurface.position.y + feetParticleOffset, ankle.position.z);                
            surfaceSplashParticles.Play();
            underSurfaceSplashParticles.Play();
        }
    }
}
