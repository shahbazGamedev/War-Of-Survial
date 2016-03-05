using UnityEngine;
using System.Collections;

public class AI_StandByCamera : MonoBehaviour {
	public Camera StandByCamera;
	////Transform CamPos;
	float speed = 1f;
	// Use this for initialization
	void Start () {
		/////CamPos = StandByCamera.transform; 

	}
	
	void Update () {

		StandByCamera.transform.Translate (speed * Time.deltaTime, 0, speed * Time.deltaTime);
	}
}
