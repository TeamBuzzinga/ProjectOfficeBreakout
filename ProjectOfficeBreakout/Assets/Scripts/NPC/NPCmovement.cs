﻿using UnityEngine;
using System.Collections;

public class NPCmovement : MonoBehaviour {

	Transform player;
	public float speed=1f;
	Vector3 movement;
	Rigidbody playerRigidbody;
	Animator anim;
	NavMeshAgent nav;
	bool startNav=false;
	bool stop=false;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("player").transform;
		nav = GetComponent <NavMeshAgent> ();
		playerRigidbody = GetComponent<Rigidbody> ();
		anim=GetComponent<Animator>();
		anim.SetBool ("move", true);
	}
	
	// Update is called once per frame
	void Update () {
		if(!stop)
			move();//auto-move

		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hitInfo = new RaycastHit();
		Debug.DrawRay (transform.position, transform.forward,Color.red);
		if (Physics.Raycast(ray,out hitInfo))
		{
				print(hitInfo.collider.gameObject.tag);
				if(hitInfo.collider.CompareTag("player"))//once detect player, start following
				{
					
					startNav=true;
				}
		}
		
		if(startNav&&nav.enabled)
			nav.SetDestination (player.position);

	}
	void move()
	{
		movement=transform.forward;
		movement = movement.normalized * speed * Time.deltaTime;
		playerRigidbody.MovePosition (transform.position + movement);
	}

	void turn()
	{
		Vector3 point= new Vector3(Random.Range(-22.0F, 22.0F), 0, Random.Range(-20.0F, 22.0F));
		Vector3 playerToMouse=point-transform.position;
		playerToMouse.y=0f;
		Quaternion newRotation=Quaternion.LookRotation(playerToMouse);
		playerRigidbody.MoveRotation(newRotation);
	}
	
	void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts) {
			//print(contact.thisCollider.name + " hit " + contact.otherCollider.name+" at "+contact.normal);
			Debug.DrawRay(contact.point, contact.normal, Color.red,10,false);
			if(!contact.otherCollider.CompareTag("floor"))
			{
				if(contact.otherCollider.gameObject.CompareTag("player"))
				{
					print("this is player");
					stop=true;
					anim.SetBool ("move", false);
				}

				float angle=Vector3.Angle(contact.normal,transform.forward);
				nav.enabled = false;
				turn();
			}
		}
	}
	void OnCollisionExit(Collision collision)
	{
		if (!collision.gameObject.CompareTag ("floor")) {
			nav.enabled = true;
			stop=false;
			anim.SetBool ("move", true);
		}
	}
}
