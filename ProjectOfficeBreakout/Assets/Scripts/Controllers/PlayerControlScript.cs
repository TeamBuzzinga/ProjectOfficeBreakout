using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Animator))]
[RequireComponent(typeof (CapsuleCollider))]
[RequireComponent(typeof (Rigidbody))]
public class PlayerControlScript : MonoBehaviour
{
	[System.NonSerialized]					
	public float lookWeight;					// the amount to transition when using head look
	
	[System.NonSerialized]
	public Transform enemy;						// a transform to Lerp the camera to during head look
	
	public float animSpeed = 1.5f;				// a public setting for overall animator animation speed
	public float lookSmoother = 3f;				// a smoothing setting for camera motion
	public bool useCurves;						// a setting for teaching purposes to show use of curves
	//	public float ePos = 0f;
	float angle;
//	public float a2;

	public float crouchSpeed = 0.8f;
	
	private Animator anim;							// a reference to the animator on the character
	private AnimatorStateInfo currentBaseState;			// a reference to the current state of the animator, used for base layer
	private CapsuleCollider col;					// a reference to the capsule collider of the character
	
	private Rigidbody rb;
	
	static int idleState = Animator.StringToHash("Base Layer.Idle");	
	static int locoState = Animator.StringToHash("Base Layer.Locomotion");			// these integers are references to our animator's states
	static int jumpState = Animator.StringToHash("Base Layer.Jump");				// and are used to check state for various actions to occur
	static int climbUpMedState = Animator.StringToHash("Base Layer.ClimbUpMed");				// and are used to check state for various actions to occur
	static int climbUpHighState = Animator.StringToHash("Base Layer.ClimbUpHigh");				// and are used to check state for various actions to occur
	static int jumpDownState = Animator.StringToHash("Base Layer.JumpDown");				// and are used to check state for various actions to occur
	static int cannotClimbState = Animator.StringToHash("Base Layer.CannotClimb");				// and are used to check state for various actions to occur

	static int crouchState = Animator.StringToHash("Base Layer.Crouch");
	static int crouchBackwardState = Animator.StringToHash("Base Layer.CrouchBackward");
	static int crouchForwardState = Animator.StringToHash("Base Layer.CrouchForward");

	static int useState = Animator.StringToHash("Base Layer.Use");		// within our FixedUpdate() function below
	
	GameObject obj;
	void Start ()
	{
		anim = GetComponent<Animator>();					  
		col = GetComponent<CapsuleCollider>();
		rb = GetComponent<Rigidbody>();
	}
	
	
	void FixedUpdate ()
	{
		float h = Input.GetAxis("Horizontal");				// setup h variable as our horizontal input axis
		float v = Input.GetAxis("Vertical");				// setup v variables as our vertical input axis
		anim.SetFloat("Speed", v);							// set our animator's float parameter 'Speed' equal to the vertical input axis				
		anim.SetFloat("Direction", h); 						// set our animator's float parameter 'Direction' equal to the horizontal input axis		
		anim.speed = animSpeed;								// set the speed of our animator to the public variable 'animSpeed'
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	// set our currentState variable to the current state of the Base Layer (0) of animation
		
		Debug.DrawRay(transform.position + Vector3.up,-Vector3.up,Color.green);
		// Use Raycast to prevent character floating bug
		Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
		RaycastHit hitInfo = new RaycastHit();
		
		if (Physics.Raycast(ray, out hitInfo))
		{
			if (hitInfo.distance > 1.75f)
			{
				anim.MatchTarget(hitInfo.point, Quaternion.identity, AvatarTarget.Root, new MatchTargetWeightMask(new Vector3(0, 1, 0), 0), 0.35f, 0.5f);
			}
		}
		
		
		
		if (currentBaseState.nameHash == locoState)
		{
			//Press Shift to Run
			if(!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
			{
				float s = anim.GetFloat("Speed");
				anim.SetFloat("Speed",s/4);
			}
			
			
			
			if (anim.GetInteger("TriggerIndex") == 3)
			{
				// Use a Raycast to check that the ground diagonally down infront of the character is of a lower height
				
				RaycastHit hit;
				Ray landingRay = new Ray(transform.position+Vector3.up,transform.forward + Vector3.down);
				RaycastHit hitInfo2 = new RaycastHit();
				//				ePos = hitInfo.distance;
				
				if (!Physics.Raycast(landingRay, out hitInfo2,4))
				{
					rb.velocity = Vector3.zero;
					anim.SetBool("JumpDown",true);
				}
			}
			
			if(Input.GetButtonDown("Jump"))
			{
				if (anim.GetInteger("TriggerIndex") == 0)
					anim.SetBool("Jump", true);
				else if (anim.GetInteger("TriggerIndex") == 1)
				{
					//if jump is called when player is on the climb medium trigger,
					//Check that player is facing object before climbing
					obj = GameObject.FindGameObjectWithTag("ClimbUpMedObject");
					angle = Vector3.Angle(transform.forward, obj.transform.position - transform.position) ;
					
					if (angle < 40)
					{
						anim.SetBool("ClimbUpMed",true);
					}
					else
						anim.SetBool("Jump",true);
				}
				else if (anim.GetInteger("TriggerIndex") == 2)
				{
					//Climb Up High
					//Use a RayCast to Check if player is facing shelf
					RaycastHit hit;
					Ray landingRay = new Ray(transform.position,transform.forward);
					RaycastHit hitInfo3 = new RaycastHit();
					
					if (Physics.Raycast(landingRay, out hitInfo3,1))
					{
						if(hitInfo3.collider.tag == "ClimbUpHighObject")
							anim.SetBool("ClimbUpHigh",true);				
						else
							anim.SetBool("Jump",true);
						
					}
				}
				else if (anim.GetInteger("TriggerIndex") == 5)
				{
					//Cannot Climb
					//Use a RayCast to check if player is infront of a tall object
					RaycastHit hit;
					Ray landingRay = new Ray(transform.position,transform.forward);
					RaycastHit hitInfo3 = new RaycastHit();
					
					if (Physics.Raycast(landingRay, out hitInfo3,1))
					{
						anim.SetBool("CannotClimb",true);				
					}
					else
						anim.SetBool("Jump",true);
				}
				
				
				
				
			}
			
			//If 'E' is pressed, check that the player is near to and facing the control panel before performing use action
			if(Input.GetKey(KeyCode.E))
			{
				if (anim.GetInteger("TriggerIndex") == 4)
				{
					
					//if jump is called when player is on the climb medium trigger,
					//Check that player is facing object before climbing
					obj = GameObject.FindGameObjectWithTag("UseTriggerObject");
					angle = Vector3.Angle(transform.forward, obj.transform.position - transform.position) ;
//					a2 = angle;
					if (angle < 80)
					{
						anim.SetBool("Use",true);
					}
					
				}
			}

			if(Input.GetKey(KeyCode.C))
			{
				anim.SetBool("Crouch",true);
			}
			
		}
		else if(currentBaseState.nameHash == idleState)
		{
			//If 'E' is pressed, check that the player is near to and facing the control panel before performing use action
			if(Input.GetKey(KeyCode.E))
			{
				if (anim.GetInteger("TriggerIndex") == 4)
				{
					
					//if jump is called when player is on the climb medium trigger,
					//Check that player is facing object before climbing
					obj = GameObject.FindGameObjectWithTag("UseTriggerObject");
					angle = Vector3.Angle(transform.forward, obj.transform.position - transform.position) ;
//					a2 = angle;
					if (angle < 80)
					{
						anim.SetBool("Use",true);
					}
					
				}
			}
			if(Input.GetKey(KeyCode.C))
			{
				anim.SetBool("Crouch",true);


			}

		}

		else if(currentBaseState.nameHash == crouchState)
		{
			if(Input.GetKey(KeyCode.C))
			{
				anim.SetBool("Crouch",false);
			}
		}
		else if (currentBaseState.nameHash == crouchBackwardState)
		{
			transform.Translate(Vector3.back * crouchSpeed * Time.deltaTime);
			float LRValue = Input.GetAxis("Horizontal");

			transform.Rotate(0, LRValue*60*Time.deltaTime,0);

		}
		else if (currentBaseState.nameHash == crouchForwardState)
		{
			transform.Translate(Vector3.forward * crouchSpeed * Time.deltaTime);
			float LRValue = Input.GetAxis("Horizontal");
//			if (LRValue<-0.1)
			transform.Rotate(0, LRValue*60*Time.deltaTime,0);
		}

		else if(currentBaseState.nameHash == useState)
		{
			if(!anim.IsInTransition(0))
			{	
				anim.SetBool("Use", false);
			}
		}
		// if we are in the jumping state... 
		else if(currentBaseState.nameHash == jumpState)
		{
			//  ..and not still in transition..
			if(!anim.IsInTransition(0))
			{
				if(useCurves)
					// ..set the collider height to a float curve in the clip called ColliderHeight
					col.height = anim.GetFloat("ColliderHeight");
				
				// reset the Jump bool so we can jump again, and so that the state does not loop 
				anim.SetBool("Jump", false);
			}
			
		}
		else if(currentBaseState.nameHash == climbUpMedState)
		{
			//  ..and not still in transition..
			if(!anim.IsInTransition(0))
			{
				if(useCurves)
				{
					// ..set the collider height to a float curve in the clip called ColliderHeight
					col.height = anim.GetFloat("ColliderHeight");
					col.center = new Vector3(0, anim.GetFloat("ColliderY"), 0);
				}
				
				//				
				// reset the Jump bool so we can jump again, and so that the state does not loop 
				anim.SetBool("ClimbUpMed", false);
			}
			
		}
		else if(currentBaseState.nameHash == climbUpHighState)
		{
			//  ..and not still in transition..
			if(!anim.IsInTransition(0))
			{
				if(useCurves)
				{
					// ..set the collider height to a float curve in the clip called ColliderHeight
					col.height = anim.GetFloat("ColliderHeight");
					col.center = new Vector3(0, anim.GetFloat("ColliderY"), 0);
				}
				
				//				
				// reset the Jump bool so we can jump again, and so that the state does not loop 
				anim.SetBool("ClimbUpHigh", false);
			}
			
		}
		
		else if(currentBaseState.nameHash == jumpDownState)
		{
			//  ..and not still in transition..
			if(!anim.IsInTransition(0))
			{
				anim.SetBool("JumpDown", false);
			}
			
		}
		else if(currentBaseState.nameHash == cannotClimbState)
		{
			//  ..and not still in transition..
			if(!anim.IsInTransition(0))
			{
				anim.SetBool("CannotClimb", false);
			}
			
		}
	}
	
	
}
