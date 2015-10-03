using UnityEngine;
using System.Collections;

public class ThrowMechanics : MonoBehaviour {
    public GameObject projectile;
    public Vector3 throwDirection;
    public Transform throwPosition;
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
        GameObject obj = (GameObject)Instantiate(projectile, throwPosition.position, new Quaternion());
        Vector3 direction = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        direction += throwDirection;

        obj.GetComponent<Rigidbody>().AddForce(direction * throwForce);
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
