using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransitionState
{
    Entering,
    Within,
    Exiting,
    None
}

public class Room : MonoBehaviour
{
    [SerializeField]
    private TransitionState _transitionState;
    public TransitionState TransitionState => _transitionState;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private IEnumerator DoTransition(Room fromRoom, RoomTransitionPoint transitionPoint)
    {
        _transitionState = TransitionState.Exiting;
        transitionPoint.NextRoom._transitionState = TransitionState.Entering;

        transitionPoint.TransitionBehavior.StartTransition(fromRoom, transitionPoint.NextRoom, transitionPoint);
        
        while (!transitionPoint.TransitionBehavior.IsDone)
        {
            transitionPoint.TransitionBehavior.Tick();
            yield return new WaitForEndOfFrame();
        }

        transitionPoint.TransitionBehavior.EndTransition();

        _transitionState = TransitionState.None;
        transitionPoint.NextRoom._transitionState = TransitionState.Within;
    }

    public void TriggerTransition(RoomTransitionPoint point)
    {
        StartCoroutine(DoTransition(this, point));
    }
}
