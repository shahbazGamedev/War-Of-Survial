using UnityEngine;
using System.Collections;

public class AI_FXManager : MonoBehaviour {

	/////public GameObject sniperBulletPrefab;
	
	///[PunRPC]
	void SniperBulletFX( Vector3 startPos, Vector3 endPos ) {
		LineRenderer gunLine;

		// Enable the lights.
		///gunLight.enabled = true;
		////faceLight.enabled = true;

		// Enable the line renderer and set it's first position to be the end of the gun.
		///gunLine.enabled = true;
		///gunLine.SetPosition (0, startPos);

		///gunLine.SetPosition (1, endPos);
	}
}
