using UnityEngine;
using System.Collections;

public class AI_SupportSpawn : MonoBehaviour {
	public float produce_time;
	public GameObject support_prefab;
	private Vector3 spawn_point;
	// Use this for initialization
	void Start () {
		produce_time = 3;
		spawn_point = new Vector3 (-80, 10, -80);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ProduceSupport(){
		GameObject.Instantiate (support_prefab, spawn_point, Quaternion.identity);
	}
}
