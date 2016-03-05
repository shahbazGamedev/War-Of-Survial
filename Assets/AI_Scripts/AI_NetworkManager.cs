using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AI_NetworkManager : MonoBehaviour {

	GameObject[] spawnSpots;
	
	public bool offlineMode = false;
	
	bool connecting = false;
	
	////List<string> chatMessages;
	////int maxChatMessages = 5;
	
	public float respawnTimer = 0;
	
	bool hasPickedTeam = false;
	int teamID=0;

	public GameObject StandByCamera;
	private GameObject follow_camera;
	private GameObject player_camera;
	
	// Use this for initialization
	void Start () {
		spawnSpots = GameObject.FindGameObjectsWithTag("spawnSpot");
		PhotonNetwork.player.name = PlayerPrefs.GetString("Username", "Awesome Dude");
		////chatMessages = new List<string>();
	}
	
	void OnDestroy() {
		PlayerPrefs.SetString("Username", PhotonNetwork.player.name);
	}

	/*
	public void AddChatMessage(string m) {
		GetComponent<PhotonView>().RPC ("AddChatMessage_RPC", PhotonTargets.AllBuffered, m);
	}
	
	[PunRPC]
	void AddChatMessage_RPC(string m) {
		while(chatMessages.Count >= maxChatMessages) {
			chatMessages.RemoveAt(0);
		}
		chatMessages.Add(m);
	}
	*/
	
	void Connect() {
		PhotonNetwork.ConnectUsingSettings(null);
	}
	
	void OnGUI() {
		GUILayout.Label( PhotonNetwork.connectionStateDetailed.ToString() );
		
		if(PhotonNetwork.connected == false && connecting == false ) {
			// We have not yet connected, so ask the player for online vs offline mode.
			GUILayout.BeginArea( new Rect(0, 0, Screen.width, Screen.height) );
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("Username: ");
			PhotonNetwork.player.name = GUILayout.TextField(PhotonNetwork.player.name);
			GUILayout.EndHorizontal();
			
			if( GUILayout.Button("Single Player") ) {
				connecting = true;
				PhotonNetwork.offlineMode = true;
				OnJoinedLobby();
			}
			
			if( GUILayout.Button("Multi Player") ) {
				connecting = true;
				Connect();
			}
			
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		
		if(PhotonNetwork.connected == true && connecting == false) {
			////Debug.Log ("2");
			if(hasPickedTeam) {
				// We are fully connected, make sure to display the chat box.
				GUILayout.BeginArea( new Rect(0, 0, Screen.width, Screen.height) );
				GUILayout.BeginVertical();
				GUILayout.FlexibleSpace();

				/*
				foreach(string msg in chatMessages) {
					GUILayout.Label(msg);
				}*/
				
				GUILayout.EndVertical();
				GUILayout.EndArea();
			}
			else {
				// Player has not yet selected a team.

				GUILayout.BeginArea( new Rect(0, 0, Screen.width, Screen.height) );
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				GUILayout.FlexibleSpace();
				
				if( GUILayout.Button("Red Team") ) {
					SpawnMyPlayer(1);
				}
				
				if( GUILayout.Button("Green Team") ) {
					SpawnMyPlayer(2);
				}
				
				if( GUILayout.Button("Random") ) {
					SpawnMyPlayer(Random.Range(1,3));	// 1 or 2
				}
				
				if( GUILayout.Button("Renegade!") ) {
					SpawnMyPlayer(0);
				}
				
				GUILayout.FlexibleSpace();
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.EndArea();
				
				
			}
			
		}
		
	}
	
	void OnJoinedLobby() {
		Debug.Log ("OnJoinedLobby");
		PhotonNetwork.JoinRandomRoom();
	}
	
	void OnPhotonRandomJoinFailed() {
		Debug.Log ("OnPhotonRandomJoinFailed");
		PhotonNetwork.CreateRoom( null );
	}
	
	void OnJoinedRoom() {
		Debug.Log ("OnJoinedRoom");
		
		connecting = false;

		//SpawnMyPlayer();
	}
	
	void SpawnMyPlayer(int teamID) {
		this.teamID = teamID;
		hasPickedTeam = true;
		////AddChatMessage("Spawning player: " + PhotonNetwork.player.name);

		if(spawnSpots == null) {
			Debug.LogError ("There is no spawnSpot!");
			return;
		}
		Transform mySpawnSpot = spawnSpots[ Random.Range (0, spawnSpots.Length) ].transform;
		StandByCamera.SetActive (false);
		GameObject myPlayerGO = (GameObject)PhotonNetwork.Instantiate("Player", mySpawnSpot.position, mySpawnSpot.rotation, 0);
		if (myPlayerGO == null)
			Debug.LogError ("Can't instantiate player!");

		//set other components
		myPlayerGO.GetComponent<AI_PlayerMouseLook>().enabled = true;
		myPlayerGO.GetComponent<AI_PlayerMovement>().enabled = true;
		myPlayerGO.GetComponent<AI_PlayerShooting>().enabled = true;
		myPlayerGO.GetComponent<PhotonView>().RPC ("SetTeamID", PhotonTargets.AllBuffered, teamID);

		//set follow camera
		////follow_camera = GameObject.FindGameObjectWithTag ("FollowCamera");
		follow_camera = GameObject.FindGameObjectWithTag ("FollowCamera");
		myPlayerGO.GetComponent<AI_PlayerMovement>().follow_camera = follow_camera.GetComponent<Camera>();
		follow_camera.GetComponent<AI_FollowCamera>().target = myPlayerGO.transform;
		////follow_camera.SetActive (true);
		follow_camera.GetComponent<Camera>().enabled = true;


		//set player camera
		player_camera = myPlayerGO.transform.FindChild ("PlayerCamera").gameObject;
		player_camera.SetActive (true);


	}
	
	void Update() {
		if(respawnTimer > 0) {
			respawnTimer -= Time.deltaTime;
			
			if(respawnTimer <= 0) {
				// Time to respawn the player!
				SpawnMyPlayer(teamID);
			}
		}
	}
}
