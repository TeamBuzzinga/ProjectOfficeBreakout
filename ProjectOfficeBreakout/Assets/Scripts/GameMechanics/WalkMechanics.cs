using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class WalkMechanics : MonoBehaviour {
	public float speed = 5;
	public float velSmoothing = 5;
	public float rotationSmoothing = 5;
	public float runThreshold = .5f;
	
	float horizontalInput;
	float verticalInput;
    float rotationDirecction;
	Vector3 inputDirection;
	Rigidbody rigid;

	public void setHorizontalInput(float horizontalInput) {
		this.horizontalInput = horizontalInput;
	}

	public void setVerticalInput(float verticalInput) {
		this.verticalInput = verticalInput;
	}

	void Start() {
		rigid = GetComponent<Rigidbody> ();
	}

	protected virtual void Update() {
		adjustInputDirection ();
		updateRotation ();
	}

	protected virtual void adjustInputDirection() {
		inputDirection = new Vector3 (horizontalInput, 0, verticalInput).normalized;
	}

	void FixedUpdate() {
		updateVelocity ();
	}

	void updateVelocity() {
		float scale = Mathf.Max (Mathf.Abs (horizontalInput), Mathf.Abs (verticalInput));
		Vector3 goalVelocity = inputDirection * speed * scale + new Vector3(0, rigid.velocity.y, 0);

		rigid.velocity = Vector3.Lerp (rigid.velocity, goalVelocity, Time.deltaTime * velSmoothing);

	}

	protected virtual void updateRotation() {
		if (Mathf.Abs(inputDirection.x) > .0001 || Mathf.Abs(inputDirection.z) > .001) {
			float degrees = Mathf.Atan2 (inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
			float x = transform.eulerAngles.x;
			float y = degrees;
			float z = transform.eulerAngles.z;

			Quaternion goal = Quaternion.Euler (x, y, z);
			transform.rotation = Quaternion.Lerp (transform.rotation, goal, rotationSmoothing * Time.deltaTime);
		}

	}

	public bool getIsWalking() {
		return Mathf.Abs (rigid.velocity.x) > .001f || Mathf.Abs (rigid.velocity.z) > .001f;
	}

	public bool getIsRunning() {
		return Mathf.Abs(horizontalInput) > runThreshold || Mathf.Abs(verticalInput) > runThreshold;
	}

	protected void setInputDirection(Vector3 inputDirection) {
		this.inputDirection = inputDirection;
	}

	public float getHorizontalInput() {
		return horizontalInput;
	}

	public float getVerticalInput() {
		return verticalInput;
	}

    public float getCurrentSpeedRatio()
    {
        Vector2 vec = new Vector2(rigid.velocity.x, rigid.velocity.z);
        return vec.magnitude / speed;
    }
}
