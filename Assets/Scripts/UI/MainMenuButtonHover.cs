using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MenuUIController _menuUI;
    private float _initialSpeed;

    void Start()
    {
        _initialSpeed = _menuUI.HeartTween.time;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _menuUI.HeartTween.setTime(_initialSpeed / 3f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _menuUI.HeartTween.setTime(_initialSpeed);
    }
}
