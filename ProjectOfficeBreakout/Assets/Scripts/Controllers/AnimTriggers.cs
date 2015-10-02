using UnityEngine;
using System.Collections;

public class AnimTriggers : MonoBehaviour
{	
	// Create a reference to the animator component
	private Animator animator;

	void Start ()
	{
		// initialise the reference to the animator component
		animator = GetComponent<Animator>();

	}
	
	// Check for which trigger the player is currently in
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.name == "ClimbUpMedTrigger")
		{
			animator.SetInteger("TriggerIndex",1);				

		}
		else if(col.gameObject.name == "ClimbUpHighTrigger")
		{
			animator.SetInteger("TriggerIndex",2);	
		}
		else if (col.gameObject.name == "JumpDownTrigger")
			animator.SetInteger("TriggerIndex",3);
		else if (col.gameObject.name == "UseTrigger")
			animator.SetInteger("TriggerIndex",4);
		else if (col.gameObject.name == "CannotClimbTrigger")
			animator.SetInteger("TriggerIndex",5);
	}
	
	//Reset it to 0 when leaving the trigger
	void OnTriggerExit(Collider col)
	{

		animator.SetInteger("TriggerIndex",0);
	}
}
