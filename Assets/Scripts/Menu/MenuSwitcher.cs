using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSwitcher : MonoBehaviour
{
    [Tooltip("Reference to the previous or next menu.")]
    public Menu menu;

    void SwitchMenu()
    {
        GetComponentInParent<Menu>().gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
    }    
}
