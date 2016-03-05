using UnityEngine;
using System.Collections;

public class AI_PlayerShooting : Photon.MonoBehaviour {

	public float damagePerShot = 20f;                  // The damage inflicted by each bullet.
	public float timeBetweenBullets = 0.15f;        // The time between each shot.
	public float range = 100f;                      // The distance the gun can fire.
	
	
	float timer;                                    // A timer to determine when to fire.
	Ray shootRay;                                   // A ray from the gun end forwards.
	////RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
	int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
	////ParticleSystem gunParticles;                    // Reference to the particle system.
		////LineRenderer gunLine;                           // Reference to the line renderer.
		////AudioSource gunAudio;                           // Reference to the audio source.
		////Light gunLight;                                 // Reference to the light component.
		////Light faceLight;								// Duh
	float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.
	AI_WeaponData weaponData;
		////AI_FXManager fxManager;
	
	////void Awake ()
	void Awake()
	{
		// Create a layer mask for the Shootable layer.
		shootableMask = LayerMask.GetMask ("Shootable");
		
			// Set up the references.
			////gunParticles = GetComponent<ParticleSystem> ();
			////gunLine = GetComponentInChildren <LineRenderer> ();
			/////faceLight = GetComponentInChildren<Light>();
			////gunAudio = GetComponentInChildren<AudioSource> ();
			////AI_FXManager = GameObject.FindObjectOfType<AI_FXManager>();
	}
	
	
	void Update ()
	{
		// Add the time since Update was last called to the timer.
		timer += Time.deltaTime;
		
		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
		{
			if(photonView.isMine){
				// ... shoot the gun.
				//this.GetComponent<PhotonView>().RPC ("Shoot", PhotonTargets.All);
				Shoot ();
			}

		}
			/*
			// If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
			if(timer >= timeBetweenBullets * effectsDisplayTime)
			{
				// ... disable the effects.
				DisableEffects ();
			}*/
	}
	
		/*
		public void DisableEffects ()
		{
			// Disable the line renderer and the light.
			gunLine.enabled = false;
			faceLight.enabled = false;
			////gunLight.enabled = false;
		}*/
	
	void Shoot ()
	{
		// Reset the timer.
		timer = 0f;

		if(weaponData==null) {
			weaponData = gameObject.GetComponentInChildren<AI_WeaponData>();
			if(weaponData==null) {
				Debug.LogError("Did not find any WeaponData in our children!");
				return;
			}
		}

		// Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
		shootRay.origin = this.transform.FindChild("PlayerCamera").transform.position;
		shootRay.direction = this.transform.FindChild("PlayerCamera").transform.forward;

			///// Play the gun shot audioclip.
			////gunAudio.Play ();
			
			///// Enable the lights.
			////gunLight.enabled = true;
			////faceLight.enabled = true;
		
		// Stop the particles from playing if they were, then start the particles.
		////gunParticles.Stop ();
		////gunParticles.Play ();
		
			// Enable the line renderer and set it's first position to be the end of the gun.
			////gunLine.enabled = true;
			////gunLine.SetPosition (0, transform.position);
		
		RaycastHit[] hits = Physics.RaycastAll(shootRay,range,shootableMask);

		Transform hitTransform = null;
		float distance = 0;
		Vector3 hitPoint = Vector3.zero;
		foreach(RaycastHit hit in hits) {
			if(hit.transform != this.transform){
			   ////&& hit.distance < distance){
			   ////&& (hit.transform.GetComponent<AI_TeamMember> ().teamID != this.GetComponent<AI_TeamMember> ().teamID )) {
				// We have hit something that is:
				// a) not us
				// b) the first thing we hit (that is not us)
				// c) or, if not b, is at least closer than the previous closest thing
				hitTransform = hit.transform;
				distance = hit.distance;
				hitPoint = hit.point;
			}
		}

		if (hitTransform != null) {
			Debug.Log ("We hit: " + hitTransform.name);
			AI_Health health = hitTransform.GetComponent<AI_Health> ();
			
			while (health == null && hitTransform.parent) {
				hitTransform = hitTransform.parent;
				health = hitTransform.GetComponent<AI_Health> ();
			}

			// Once we reach here, hitTransform may not be the hitTransform we started with!
			
			if (health != null) {
				// This next line is the equivalent of calling:
				//    				h.TakeDamage( damage );
				// Except more "networky"
				PhotonView pv = health.GetComponent<PhotonView> ();
				if (pv == null) {
					Debug.LogError ("Freak out!");
				} else {
					
					////AI_TeamMember tm = hitTransform.GetComponent<AI_TeamMember> ();
					////AI_TeamMember myTm = this.GetComponent<AI_TeamMember> ();
					
					////if(tm == null || tm.teamID == 0 || myTm == null || myTm.teamID == 0 || tm.teamID != myTm.teamID ) {
					////if (tm.teamID != myTm.teamID) {
					health.GetComponent<PhotonView> ().RPC ("TakeDamage", PhotonTargets.AllBuffered, damagePerShot);
				}
				
			} else {
				Debug.LogError("Need AI_Health component!");
			}
	
				////fxManager.GetComponent<PhotonView>().RPC ("SniperBulletFX", PhotonTargets.All, weaponData.transform.position, hitPoint);
				////gunLine.SetPosition (1, hitPoint);
			weaponData.GetComponent<PhotonView>().RPC ("SniperBulletFX", PhotonTargets.All, hitPoint);
		} else {
			weaponData.GetComponent<PhotonView>().RPC ("SniperBulletFX", PhotonTargets.All, shootRay.origin + shootRay.direction * range);
				// ... set the second position of the line renderer to the fullest extent of the gun's range.
				////fxManager.GetComponent<PhotonView>().RPC ("SniperBulletFX", PhotonTargets.All, 
			                                          ////weaponData.transform.position, shootRay.origin + shootRay.direction * range);
				////gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
		}
	}
}
