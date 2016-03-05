using UnityEngine;
using System.Collections;

public class AI_WeaponData : MonoBehaviour {
	LineRenderer gunLine;                           // Reference to the line renderer.
	////Light gunLight;                                 // Reference to the light component.
	Light faceLight;								// Duh
	AudioSource gunAudio;                           // Reference to the audio source.
	public float selfDestructTime = 0.2f;
	bool light_open = false;

	void Start(){
		gunLine = GetComponentInChildren <LineRenderer> ();
		faceLight = GetComponentInChildren<Light>();
		gunAudio = GetComponentInChildren<AudioSource> ();
	}


	
	void Update () {
		if (light_open) {
			selfDestructTime -= Time.deltaTime;
			if(selfDestructTime <= 0) {
				// Disable the line renderer and the light.
				gunLine.enabled = false;
				faceLight.enabled = false;
				////gunLight.enabled = false;
				light_open = false;
				selfDestructTime = 0.2f;
			}
		}

	}
	[PunRPC]
	void SniperBulletFX(Vector3 endPos ) {

		// Play the gun shot audioclip.
		gunAudio.Play ();
		// Enable the lights.
		////gunLight.enabled = true;
		faceLight.enabled = true;
		
		// Enable the line renderer and set it's first position to be the end of the gun.
		gunLine.enabled = true;
		gunLine.SetPosition (0, transform.position);
		gunLine.SetPosition (1, endPos);
		light_open = true;
	}
}
