using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform targetFollow;
    public WalkMechanics walkMechanics;
    public float verticalSensitivity = 5;
	public float horizontalSensitivity = 5;
	public float rotateSmoothing = 5;
	public float cameraSmoothing = 5;
    public float walkSmoothing = 5;
	Transform goalTransfom;
    

	Vector3 offset;
	float distanceFromTarget;


	void Start() {
		offset = this.transform.position - targetFollow.position;
		distanceFromTarget = Vector3.Magnitude (offset);
		goalTransfom = new GameObject ().transform;
		goalTransfom.rotation = this.transform.rotation;
        //walkMechanics = targetFollow.GetComponent<WalkMechanics>();

	}

	protected virtual void Update() {
        //print (distanceFromTarget);

        adjustRotation();
       // adjustMovementRotation();

        Vector3 goalPosition = -transform.forward * distanceFromTarget + targetFollow.position;
		//transform.position = Vector3.Lerp(transform.position, goalPosition, Time.deltaTime * cameraSmoothing);
		transform.position = goalPosition;
		resetZRotation ();
        
    }

    void adjustMovementRotation()
    {
        float horInput = walkMechanics.getHorizontalInput();
        float verInput = walkMechanics.getVerticalInput();
        Vector2 vec = new Vector2(horInput, verInput).normalized;
        float scale = Mathf.Max(horInput, verInput);

        float horRotation = vec.x * scale;
        //print(horRotation);
        goalTransfom.Rotate(new Vector3(0, horRotation * walkSmoothing * Time.deltaTime, 0));

    }

	protected virtual void adjustRotation () {
		transform.rotation = Quaternion.Slerp (transform.rotation, goalTransfom.rotation, Time.deltaTime * rotateSmoothing);
		//transform.rotation = goalTransfom.rotation;
	}

	void resetZRotation() {
		goalTransfom.rotation = Quaternion.Euler (goalTransfom.eulerAngles.x, goalTransfom.eulerAngles.y, 0);
		transform.rotation = Quaternion.Euler (transform.eulerAngles.x, transform.eulerAngles.y, 0);
	}

	public void cameraRotateHorizontal(float horizontalInput) {
		goalTransfom.Rotate (new Vector3 (0, horizontalInput * horizontalSensitivity * Time.deltaTime, 0)); 
	}

	public void cameraRotateVertical(float verticalInput) {
		goalTransfom.Rotate (new Vector3 (-verticalInput * verticalSensitivity * Time.deltaTime, 0, 0));
	}
}
