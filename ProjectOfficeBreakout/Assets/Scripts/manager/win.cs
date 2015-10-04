using UnityEngine;

public class win : MonoBehaviour
{
	public float restartDelay=5f;
	GameObject player;
	Animator anim;
	float restartTimer;
	
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("player");
		anim = GetComponent<Animator>();
	}
	
	void Update()
	{
		if (player.GetComponent<Animator>().GetBool("Win"))
		{
			anim.SetTrigger("Win");
			restartTimer+=Time.deltaTime;
			if(restartTimer>=restartDelay)
				Application.LoadLevel(Application.loadedLevel);
		}
	}
}
