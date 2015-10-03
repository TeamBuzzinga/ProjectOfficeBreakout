using UnityEngine;

public class gameover : MonoBehaviour
{
	public float restartDelay=5f;
	public NPCmovement NPC;
	Animator anim;
	float restartTimer;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void Update()
	{
		if (NPC.gameOver)
		{
			anim.SetTrigger("gameOver");
			print ("gameovergameovergameover");
			restartTimer+=Time.deltaTime;
			if(restartTimer>=restartDelay)
				Application.LoadLevel(Application.loadedLevel);
		}
	}
}
