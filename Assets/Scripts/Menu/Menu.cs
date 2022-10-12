using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
    public bool isActive = false;

    void Awake()
    {
        this.gameObject.SetActive(isActive);
    }
}
