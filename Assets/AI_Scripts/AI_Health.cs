using UnityEngine;
using System.Collections;

public class AI_Health : MonoBehaviour {

	public float totalhitPoints = 100f;
	public float currentHitPoints;
	
	// Use this for initialization
	void Start () {
		currentHitPoints = totalhitPoints;
	}
	
	[PunRPC]
	public void TakeDamage(float amt) {
		currentHitPoints -= amt;
		
		if(currentHitPoints <= 0) {
			Die();
		}
	}

	void Die() {
		if( GetComponent<PhotonView>().instantiationId==0 ) {
			Destroy(gameObject);
		}
		else {
			if( GetComponent<PhotonView>().isMine ) {
				if( gameObject.tag == "Player" ) {		// This is my actual PLAYER object, then initiate the respawn process
					AI_NetworkManager nm = GameObject.FindObjectOfType<AI_NetworkManager>();
					
					nm.StandByCamera.SetActive(true);
					nm.respawnTimer = 3f;
				}
				else if( gameObject.tag == "Bot" ) {
					Debug.LogError("WARNING: No bot respawn code exists!");
				}
				
				PhotonNetwork.Destroy(gameObject);
			}
		}
	}


}
