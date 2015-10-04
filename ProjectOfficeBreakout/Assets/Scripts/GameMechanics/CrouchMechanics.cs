using UnityEngine;
using System.Collections;

public class CrouchMechanics : MonoBehaviour {
    bool crouchInputDown;

    public void crouch(bool crouchInputDown)
    {
        this.crouchInputDown = crouchInputDown;
    }

    public bool getIsCrouching()
    {
        return crouchInputDown;
    }
}
