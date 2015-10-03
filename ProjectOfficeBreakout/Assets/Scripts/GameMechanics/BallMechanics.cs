using UnityEngine;
using System.Collections;

public class BallMechanics : MonoBehaviour {
    void OnTriggerEnter(Collider collider)
    {
        if (!(collider.tag == "player"))
        {
            Destroy(this.gameObject);
        }
    }
}
