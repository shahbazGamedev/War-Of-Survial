using UnityEngine;
using System.Collections;

public class AI_PlayerMouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -15F;
	public float maximumX = 15F;
	public float minimumY = -15F;
	public float maximumY = 15F;

	float rotationX = 0F;
	float rotationY = 0F;
	private Vector3 eulerAngles;
	
	void LateUpdate ()
	{
		if (axes == RotationAxes.MouseX)
		{
			rotationX = Input.GetAxis("Mouse X") * sensitivityX;
			rotationX = Mathf.Clamp (rotationX, minimumX, maximumX);
			transform.Rotate(0, rotationX, 0);
		}
		else
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(eulerAngles.x-rotationY, eulerAngles.y, eulerAngles.z);
		}
	}
	
	void Start ()
	{
		eulerAngles = transform.localEulerAngles;
	}
}