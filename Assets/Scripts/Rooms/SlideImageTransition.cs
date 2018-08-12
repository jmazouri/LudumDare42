using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Transitions/Slide Image")]
public class SlideImageTransition : TransitionBehavior
{
    [NonSerialized]
    private RectTransform _slideTarget;
    [NonSerialized]
    private Image _slideImage;

    [NonSerialized]
    private float _slideAmount = 0;
    [NonSerialized]
    private bool _didMove = false;

    public AnimationCurve Curve;
    public Sprite ImageOverride;

    public override void StartTransition(Room fromRoom, Room toRoom, RoomTransitionPoint transitionPoint)
    {
        _slideTarget = GameObject.Find("SlideTarget").GetComponent<RectTransform>();
        _slideTarget.anchoredPosition = new Vector2(800, 0);

        _slideImage = _slideTarget.GetComponent<Image>();

        if (ImageOverride != null)
        {
            _slideImage.sprite = ImageOverride;
        }

        _slideImage.color = new Color(0, 0, 0, 0);

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

        float tweenAmount = Mathf.Lerp(800, -800, Curve.Evaluate(_slideAmount));

        //The hackiest shit in the world, but it looks good
        float fadeAmount = (_slideAmount <= 0.5f ? Mathf.Lerp(0, 2, Curve.Evaluate(_slideAmount))
                                                 : Mathf.Lerp(2, 0, Curve.Evaluate(_slideAmount)));
        fadeAmount *= 1.5f;

        if (tweenAmount <= 0f && !_didMove)
        {
            MoveCamera();
            _didMove = true;
        }

        _slideImage.color = new Color(1, 1, 1, fadeAmount);
        _slideTarget.anchoredPosition = new Vector2(tweenAmount, 0);
    }
}
