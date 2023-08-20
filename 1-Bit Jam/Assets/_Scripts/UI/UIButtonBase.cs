using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UIButtonBase : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField] AudioClip buttonClick;

    AudioClip buttonClickPlay;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnEnter();
    }

    protected void Start()
    {
        buttonClickPlay = buttonClick;
    }

    public virtual void OnClick()
    {
        AudioManager.Instance.PlayAudioClip(buttonClickPlay);
    }

    public virtual void OnEnter()
    {

    }
}
