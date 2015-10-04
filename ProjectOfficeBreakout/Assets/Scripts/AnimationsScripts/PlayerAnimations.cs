using UnityEngine;
using System.Collections;

public class PlayerAnimations : MonoBehaviour {
    Animator anim;
    WalkMechanics walkMechanics;
    CrouchMechanics crouchMechanics;

    void Start()
    {
        anim = GetComponent<Animator> ();
        walkMechanics = GetComponent <WalkMechanics> ();
        crouchMechanics = GetComponent<CrouchMechanics> ();
    }

    void Update()
    {
        anim.SetFloat("Speed", walkMechanics.getCurrentSpeedRatio());
        anim.SetBool("isCrouching", crouchMechanics.getIsCrouching());
    }
}
