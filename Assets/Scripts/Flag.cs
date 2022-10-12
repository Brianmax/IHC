using UnityEngine;
using System.Collections;

public class Flag : MonoBehaviour
{
    void Start()
    {
        GetComponent<Cloth>().externalAcceleration = new Vector3(
            Random.Range(0.5f, 2.0f),
            Random.Range(0.5f, 2.0f),
            Random.Range(0.5f, 2.0f));
    }
}
