using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    float horizontalInput;
	float verticalInput;
	WalkMechanics walkMechanics;

	void Update() {
		horizontalInput = Input.GetAxisRaw ("Horizontal");
		verticalInput = Input.GetAxisRaw ("Vertical");

		walkMechanics.setVerticalInput (verticalInput);
		walkMechanics.setHorizontalInput (horizontalInput);
		
	}

	void Start() {
		walkMechanics = GetComponent<WalkMechanics> ();
	}

}
