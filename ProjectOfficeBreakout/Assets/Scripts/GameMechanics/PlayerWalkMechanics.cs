using UnityEngine;
using System.Collections;

public class PlayerWalkMechanics : WalkMechanics {
	public Transform cameraOffset;
    ThrowMechanics throwMechanics;
    Vector3 rotationDirection;

	protected override void adjustInputDirection ()
	{
        
		float x = cameraOffset.transform.forward.x;
		float z = cameraOffset.transform.forward.z;
		Vector3 offsetDirection = new Vector3 (x, 0, z).normalized;

		Vector3 newInputDirection = offsetDirection * getVerticalInput () + new Vector3(z, 0, -x) * getHorizontalInput();
        rotationDirection = newInputDirection.normalized;
		setInputDirection (rotationDirection);
        if (Input.GetButton("Fire1"))
        {
            rotationDirection = new Vector3(cameraOffset.forward.x, 0, cameraOffset.forward.z).normalized;
            return;

        }

    }

    protected override void Update()
    {
        adjustInputDirection();
        updateRotation(rotationDirection);
    }

    protected override void Start()
    {
        base.Start();
        throwMechanics = GetComponent<ThrowMechanics>();
    }
}
