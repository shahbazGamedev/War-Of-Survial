using UnityEngine;
using System.Collections;

public class AI_NetworkCharater : MonoBehaviour {

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {

		if(stream.isWriting) {
			// This is OUR player. We need to send our actual position to the network.
			
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
;
		}
		else {

			
		}
		
	}
}
