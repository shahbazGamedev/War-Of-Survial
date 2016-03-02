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
				
				if( GUILayout.Button("Red Team") ) {Debug.Log ("4");
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
		StandByCamera.SetActive (false);
		//SpawnMyPlayer();
	}
	
	void SpawnMyPlayer(int teamID) {Debug.Log ("SpawnMyPlayer");
		//StandByCamera.SetActive (false);Debug.Log ("SpawnMyPlayer1");
		this.teamID = teamID;
		hasPickedTeam = true;
		////AddChatMessage("Spawning player: " + PhotonNetwork.player.name);

		if(spawnSpots == null) {
			Debug.LogError ("There is no spawnSpot!");Debug.Log ("spawnSpots == null");
			return;
		}
		Debug.Log ("6");
		Transform mySpawnSpot = spawnSpots[ Random.Range (0, spawnSpots.Length) ].transform;Debug.Log ("7");
		GameObject myPlayerGO = (GameObject)PhotonNetwork.Instantiate("Player", mySpawnSpot.position, mySpawnSpot.rotation, 0);Debug.Log ("8");
		if (myPlayerGO == null)
			Debug.Log ("5");
		//((MonoBehaviour)myPlayerGO.GetComponent("FPSInputController")).enabled = true;
		((MonoBehaviour)myPlayerGO.GetComponent("AI_PlayerMouseLook")).enabled = true;
		((MonoBehaviour)myPlayerGO.GetComponent("AI_PlayerMovement")).enabled = true;


		myPlayerGO.GetComponent<PhotonView>().RPC ("SetTeamID", PhotonTargets.AllBuffered, teamID);
		
		myPlayerGO.transform.FindChild("PlayerCamera").gameObject.SetActive(true);

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
