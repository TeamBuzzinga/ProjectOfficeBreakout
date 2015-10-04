using UnityEngine;
using System.Collections;

public class NPCmovement : MonoBehaviour {

	Transform player;
	public Transform rayorigin;
	public float speed=1f;
	Vector3 movement;
	Rigidbody playerRigidbody;
	Animator anim;
	NavMeshAgent nav;
	bool startNav=false;
	bool stop=false;
	bool attack=false;
	float timer=5f;
	public bool gameOver=false;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("player").transform;
		nav = GetComponent <NavMeshAgent> ();
		playerRigidbody = GetComponent<Rigidbody> ();
		anim=GetComponent<Animator>();
		anim.SetBool("move", true);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//stop the NPC when player wins
		if(GameObject.FindGameObjectWithTag("player").GetComponent<Animator>().GetBool("Win"))
		{
			stop= true;
			nav.Stop();
		}


		//when attacked, start the timer. 
		if (attack) {
			nav.Stop();//pause nav mesh agent
			stop=true;//stop moving
			timer=timer-Time.deltaTime;//decreasing timer
			if(timer<0)
			{
				nav.Resume();//resume nav mesh agent
				timer=5f;//reset timer
				stop=false;//reset
				attack=false;//reset
				anim.SetBool("move",true);
			}
		}


		if(!stop)
			move();//auto-move
		//array of rays to detect players in a certain range
		Ray[] rays=new Ray[10];
		for (int i=0; i<10; i++) {
			rays[i].origin=rayorigin.position;
			rays[i].direction=Quaternion.AngleAxis(12*(i-5),transform.up)*transform.forward;
			//Debug.DrawRay (rays[i].origin, rays[i].direction,Color.red);

			RaycastHit hitInfo = new RaycastHit();
			
			if (Physics.Raycast(rays[i],out hitInfo,50))
			{
				//print(hitInfo.collider.gameObject.tag);
				if(hitInfo.collider.CompareTag("player"))//once detect player, start following
				{
					startNav=true;
				}
			}
		}


		//start nav
		if (startNav && nav.enabled) {
			print ("start naving");
			nav.SetDestination(player.position);
		}

	}
	void move()
	{
		movement=transform.forward;
		movement = movement.normalized * speed * Time.deltaTime;
		playerRigidbody.MovePosition (transform.position + movement);
	}

	void turn()
	{
		//pick a random spot in the map for turning
		print ("turning!");
		Vector3 point= new Vector3(Random.Range(-22.0F, 22.0F), 0, Random.Range(-20.0F, 22.0F));
		Vector3 playerToMouse=point-transform.position;
		playerToMouse.y=0f;
		Quaternion newRotation=Quaternion.LookRotation(playerToMouse);
		playerRigidbody.MoveRotation(newRotation);
	}

	void OnCollisionEnter(Collision collision)
	{
		//check each collision contact point
		foreach (ContactPoint contact in collision.contacts) 
		{
			//Debug.DrawRay(contact.point, contact.normal, Color.red,10,false);
			if(!contact.otherCollider.CompareTag("floor"))
			{
				if(contact.otherCollider.gameObject.CompareTag("player"))//lose condition, when colliding with player, game over
				{
					print("this is player, stop");
					stop=true;
					anim.SetBool ("move", false);
					gameOver=true;
					GameObject.FindGameObjectWithTag("player").GetComponent<Animator>().SetBool("GameOver",true);
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

	//when attacked, set attack to be true;
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("ball")) {
			//Debug.Log("attackattackattack");
			attack=true;
			anim.SetBool("move",false);
		}
	}
}
