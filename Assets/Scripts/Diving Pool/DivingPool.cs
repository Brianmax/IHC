using UnityEngine;
using System.Collections;

public class DivingPool : MonoBehaviour {

    void OnCollisionEnter (Collision collision)
    {
        // Send a message to the player to apply the state "OnStopMovement"
        collision.collider.SendMessage("ApplyState", "OnStopMovement");
    }
}
