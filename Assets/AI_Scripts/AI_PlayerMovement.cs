using UnityEngine;

public class AI_PlayerMovement : MonoBehaviour
{
	public float speed = 6f;            // The speed that the player will move at.
	
	
	Vector3 movement;                   // The vector to store the direction of the player's movement.
	Animator anim;                      // Reference to the animator component.
	//////int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
	////float camRayLength = 100f;          // The length of the ray from the camera into the scene.
	public Camera follow_camera;
	public Camera player_camera;

	CharacterController cc;

	////void Awake ()
	void Start()
	{
		// Create a layer mask for the floor layer.
		//////floorMask = LayerMask.GetMask ("Floor");

		////Camera
		follow_camera.enabled = true;
		player_camera.enabled = false;

		// Set up references.
		anim = GetComponent <Animator> ();
		if(anim == null) {
			Debug.LogError ("ZOMG, you forgot to put an Animator component on this character prefab!");
		}
		


		cc = GetComponent<CharacterController>();
		if(cc == null) {
			Debug.LogError("No character controller!");
		}

	}
	
	
	void Update ()
	{

		// Store the input axes.
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
		// Move the player around the scene.
		Move (h, v);
			
		// Turn the player to face the mouse cursor.
		/////Turning ();
			
		// Animate the player.
		Animating (h, v);


		////switch between main camera and player camera
		SwitchCamera ();
	}
	
	
	void Move (float h, float v)
	{
		// Set the movement vector based on the axis input.
		movement = transform.forward * v + transform.right * h;
		//movement.Set (h, 0f, v);Debug.Log ("transform.forward "+transform.forward+"transform.right "+transform.right);
		//movement.Set (h+transform.right.x, 0f, v+transform.forward.z);
		
		// Normalise the movement vector and make it proportional to the speed per second.
		movement = movement.normalized * speed * Time.deltaTime;
		cc.Move( movement );
		
	}
	
	void Animating (float h, float v)
	{
		// Create a boolean that is true if either of the input axes is non-zero.
		bool walking = h != 0f || v != 0f;
		
		// Tell the animator whether or not the player is walking.
		anim.SetBool ("IsWalking", walking);
	}

	void SwitchCamera(){
		if (Input.GetKeyDown(KeyCode.Z)){
			follow_camera.enabled = !follow_camera.enabled;
			player_camera.enabled = !player_camera.enabled;
		}
	}
}