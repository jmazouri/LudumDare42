using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Transitions/Fade Out")]
public class FadeOutTransition : TransitionBehavior
{
    [NonSerialized]
    private float _fadeAmount = 0;
    [NonSerialized]
    private bool _flip = false;
    [NonSerialized]
    public Image _fadeTarget;

    public override void StartTransition(Room fromRoom, Room toRoom, RoomTransitionPoint transitionPoint)
    {
        IsDone = false;
        _flip = false;
        _fadeTarget = GameObject.Find("FadeTarget").GetComponent<Image>();

        base.StartTransition(fromRoom, toRoom, transitionPoint);
    }

    public override void InternalTick(float deltaTime)
    {
        if (_flip)
        {
            _fadeAmount -= deltaTime;
        }
        else
        {
            _fadeAmount += deltaTime;
            
            if (_fadeAmount > 1)
            {
                _flip = true;
                MoveCamera();
            }
        }

        if (_fadeAmount < 0)
        {
            _fadeAmount = 0;
            IsDone = true;
            return;
        }

        float tweenedFade = LeanTween.easeInOutQuart(0, 1, _fadeAmount);
        _fadeTarget.color = Color.Lerp(new Color(), Color.black, tweenedFade);
    }
}