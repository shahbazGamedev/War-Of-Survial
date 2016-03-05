using UnityEngine;
using System.Collections;

public class AI_NetworkCharacter : Photon.MonoBehaviour {

	Vector3 realPosition = Vector3.zero;
	Quaternion realRotation = Quaternion.identity;
	bool gotFirstUpdate = false;
	Animator anim;


	void Awake()
	{
		anim = GetComponent <Animator> ();
		if(anim == null) {
			Debug.LogError ("ZOMG, you forgot to put an Animator component on this character prefab!!!!!");
		}


	}
	
	
	void FixedUpdate ()
	{
		if (!photonView.isMine) {

			transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);
			transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
		}
	}
	



	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {

		if(stream.isWriting) {
			// This is OUR player. We need to send our actual position to the network.
			
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			if(anim == null) {
				Debug.LogError ("ZOMG, you forgot to put an Animator component on this character prefab!");
			}

			stream.SendNext(anim.GetBool("IsWalking"));
		}
		else {
			realPosition = (Vector3)stream.ReceiveNext();
			realRotation = (Quaternion)stream.ReceiveNext();
			if(anim == null) {
				Debug.LogError ("ZOMG, you forgot to put an Animator component on this character prefab!");
			}

			anim.SetBool("IsWalking", (bool)stream.ReceiveNext());

			if(gotFirstUpdate == false) {
				transform.position = realPosition;
				transform.rotation = realRotation;
				gotFirstUpdate = true;
			}
		}
		
	}
}
