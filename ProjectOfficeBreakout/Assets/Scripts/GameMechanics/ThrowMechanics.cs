using UnityEngine;
using System.Collections;

public class ThrowMechanics : MonoBehaviour {
    public GameObject projectile;
    public Vector3 throwDirection;
    public Vector3 throwPosition;
    public float throwForce;
    public float throwTime;//The time that it takes to complete one throw
    public float coolDownTime;//The time before you can throw another ball

    float throwTimer;
    float coolDownTimer;

    void Update()
    {
        updateTimers();
    }



    void updateTimers()
    {
        throwTimer -= Time.deltaTime;
        coolDownTimer -= Time.deltaTime;

        if (throwTimer < 0)
        {
            throwTimer = 0;
        }

        if (coolDownTimer < 0)
        {
            coolDownTimer = 0;
        }
    }

    void createBall()
    {
        GameObject obj = (GameObject)Instantiate(projectile, throwPosition, new Quaternion());
        obj.GetComponent<Rigidbody>().AddForce(throwDirection.normalized * throwForce);
    }

    public void throwBall(bool throwButtonDown) 
    {
        if (throwButtonDown && canThrow())
        {
            throwTimer = throwTime;
            coolDownTimer = coolDownTime;
            createBall();
        }
    }

    public bool getIsThrowing()
    {
        return throwTimer > 0;
    }

    public bool canThrow()
    {
        return coolDownTimer <= 0;
    }
}
