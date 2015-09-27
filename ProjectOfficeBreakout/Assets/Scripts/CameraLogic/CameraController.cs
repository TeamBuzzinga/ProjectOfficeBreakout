using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	float horizontalInput;
	float verticalInput;
	CameraFollow cameraFollow;

	void Start() {
		cameraFollow = GetComponent<CameraFollow> ();
	}
	
	// Update is called once per frame
	void Update () {
		horizontalInput = Input.GetAxisRaw("LookHorizontal");
		verticalInput = Input.GetAxisRaw ("LookVertical");

		cameraFollow.cameraRotateHorizontal (horizontalInput);
		cameraFollow.cameraRotateVertical (verticalInput);
	}
}
