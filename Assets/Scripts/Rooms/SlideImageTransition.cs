using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Transitions/Slide Image")]
public class SlideImageTransition : TransitionBehavior
{
    [NonSerialized]
    private RectTransform _slideTarget;
    [NonSerialized]
    private float _slideAmount = 0;
    [NonSerialized]
    private bool _didMove = false;

    public AnimationCurve Curve;

    public override void StartTransition(Room fromRoom, Room toRoom, RoomTransitionPoint transitionPoint)
    {
        _slideTarget = GameObject.Find("SlideTarget").GetComponent<RectTransform>();
        _slideTarget.anchoredPosition = new Vector2(800, 0);

        base.StartTransition(fromRoom, toRoom, transitionPoint);
    }

    public override void InternalTick(float deltaTime)
    {
        if (_slideAmount >= 1)
        {
            IsDone = true;
            _didMove = false;
            _slideAmount = 0;
            return;
        }

        _slideAmount += deltaTime;
        float tweenAmount = Mathf.Lerp(800, -800, Curve.Evaluate(_slideAmount));// LeanTween.easeInOutQuad(800, -800, _slideAmount);

        if (tweenAmount <= 0f && !_didMove)
        {
            MoveCamera();
            _didMove = true;
        }

        _slideTarget.anchoredPosition = new Vector2(tweenAmount, 0);
    }
}
