using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class AI_SupportMovement : MonoBehaviour 
{
	// Private Members
	private NavMeshAgent navAgent = null;
	private float camRayLength = 100f;          // The length of the ray from the camera into the scene.
	private int floorMask;
	private Transform myPlayer;
	private bool destinationReached;
	private Vector3 destination;

	public float AttackRadius = 10f;

	
	// -----------------------------------------------------
	// Name :	Start
	// Desc	:	Cache MavMeshAgent and set initial 
	//			destination.
	// -----------------------------------------------------
	void Start () 
	{
		// Cache NavMeshAgent Reference
		navAgent = GetComponent<NavMeshAgent>();
		// Create a layer mask for the floor layer.
		floorMask = LayerMask.GetMask ("Floor");
		destinationReached = true;

		///EnemyPlayer = GameObject.FindGameObjectWithTag ("EnemyPlayer").transform;
		myPlayer = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	
	
	// ---------------------------------------------------------
	// Name	:	Update
	// Desc	:	Called each frame by Unity
	// ---------------------------------------------------------
	void Update () 
	{

		if(((myPlayer.position - transform.position).magnitude <= AttackRadius) && destinationReached){
			navAgent.SetDestination (myPlayer.position);	//Debug.Log ("navAgent.SetDestination");
		}
		if (Input.GetMouseButtonDown (1)) {
			NavMovement();
			destinationReached = false;	//Debug.Log ("destinationReached = false");
		}

		if((transform.position - destination).magnitude < 0.5f){
			destinationReached = true;	//Debug.Log ("destinationReached = true"+destinationReached);
		}//Debug.Log ((transform.position - destination).magnitude);


	}

	void NavMovement(){
		// Create a ray from the mouse cursor on screen in the direction of the camera.
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		// Create a RaycastHit variable to store information about what was hit by the ray.
		RaycastHit floorHit;
		
		// Perform the raycast and if it hits something on the floor layer...
		if(Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
		{
			// Create a vector from the player to the point on the floor the raycast from the mouse hit.
			destination = floorHit.point;
			navAgent.SetDestination (destination);
		}

	}
	
}
