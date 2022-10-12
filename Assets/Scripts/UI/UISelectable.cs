using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISelectable : Selectable
{
    private bool onFocus = false;

    override public void OnPointerEnter(PointerEventData data)
    {
        onFocus = true;
        base.OnPointerEnter(data);
    }

    override public void OnPointerExit(PointerEventData data)
    {
        onFocus = false;
        base.OnPointerExit(data);
    }

    void Update()
    {

    }
}
