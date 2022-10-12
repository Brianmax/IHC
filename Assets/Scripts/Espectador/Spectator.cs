using UnityEngine;
using System.Collections;

public class Spectator : MonoBehaviour
{
    void ChangeAnimation(string animationName)
    {
        GetComponent<Animation>().Play(animationName);
    }
}
