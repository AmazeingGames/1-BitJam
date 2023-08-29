using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static AudioManager.EventSounds;
public abstract class UIButtonBase : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnEnter();
    }

    public virtual void OnClick()
    {
        AudioManager.Instance.PlayAudioClip(UIClick, transform.position);
    }

    public virtual void OnEnter()
    {

    }
}
