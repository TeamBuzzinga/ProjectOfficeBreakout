using UnityEngine;
using System.Collections;

public class PlayerWalkMechanics : WalkMechanics {
	public Transform cameraOffset;

	protected override void adjustInputDirection ()
	{
		float x = cameraOffset.transform.forward.x;
		float z = cameraOffset.transform.forward.z;
		Vector3 offsetDirection = new Vector3 (x, 0, z).normalized;

		Vector3 newInputDirection = offsetDirection * getVerticalInput () + new Vector3(z, 0, -x) * getHorizontalInput();
		setInputDirection (newInputDirection.normalized);

	}

}
