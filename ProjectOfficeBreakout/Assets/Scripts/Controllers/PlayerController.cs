using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    float horizontalInput;
	float verticalInput;
	WalkMechanics walkMechanics;
    ThrowMechanics throwMechanics;
    CrouchMechanics crouchMechanics;

	void Update() {
		horizontalInput = Input.GetAxisRaw ("Horizontal");
		verticalInput = Input.GetAxisRaw ("Vertical");

		walkMechanics.setVerticalInput (verticalInput);
		walkMechanics.setHorizontalInput (horizontalInput);

        throwMechanics.throwBall(Input.GetButtonUp("Fire1"));
        crouchMechanics.crouch(Input.GetKey(KeyCode.C));
		
	}

	void Start() {
		walkMechanics = GetComponent<WalkMechanics> ();
        throwMechanics = GetComponent<ThrowMechanics>();
        crouchMechanics = GetComponent<CrouchMechanics>();
	}

}
