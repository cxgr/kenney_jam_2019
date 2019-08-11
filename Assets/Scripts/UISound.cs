using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISound : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    private SoundManager sound => SingletonUtils<SoundManager>.Instance;

    public string clickKey = "button_click";
    public string overKey = "button_over";
    
    public void OnPointerDown(PointerEventData eventData)
    {
        sound.PlaySound(clickKey);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        sound.PlaySound(overKey);
    }
}
