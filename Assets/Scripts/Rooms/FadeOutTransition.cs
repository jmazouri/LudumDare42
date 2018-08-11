using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Transitions/Fade Out")]
public class FadeOutTransition : TransitionBehavior
{
    [NonSerialized]
    private float _fadeAmount = 0;
    [NonSerialized]
    private Color _startColor;
    [NonSerialized]
    private bool _flip = false;

    public override void StartTransition(Room fromRoom, Room toRoom, RoomTransitionPoint transitionPoint)
    {
        IsDone = false;
        _flip = false;
        base.StartTransition(fromRoom, toRoom, transitionPoint);

        _startColor = Camera.main.backgroundColor;
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

        //TODO: Actually make this fade
        Camera.main.backgroundColor = Color.Lerp(_startColor, Color.black, _fadeAmount);
    }
}