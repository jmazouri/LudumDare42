using System.Collections;
using System.Collections.Generic;
using LD42.PlayerControllers;
using UnityEngine;
using UnityEngine.Serialization;

public enum TransitionState
{
    Entering,
    Within,
    Exiting,
    None
}

public class Room : MonoBehaviour
{
    [SerializeField] private TransitionState _transitionState = TransitionState.None;
    
    public TransitionState TransitionState => _transitionState;

    public bool IsInitialisingRoom;

    [SerializeField] private GameObject _initialPlayer;
    [SerializeField] private EnemySpawnerBehaviour[] _spawners;

    public IPlayerController PlayerController { get; set; }

    private RoomTransitionPoint[] _transitionPoints;
    public IReadOnlyList<RoomTransitionPoint> TransitionPoints => 
        _transitionPoints ?? (_transitionPoints = GetComponentsInChildren<RoomTransitionPoint>());

    void Start()
    {
        if (!IsInitialisingRoom) return;

        PlayerController = _initialPlayer.GetComponent<IPlayerController>();
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }

    private IEnumerator DoTransition(Room fromRoom, RoomTransitionPoint transitionPoint)
    {
        _transitionState = TransitionState.Exiting;
        transitionPoint.LinkedRoom._transitionState = TransitionState.Entering;

        transitionPoint.TransitionBehavior.StartTransition(fromRoom, transitionPoint.LinkedRoom, transitionPoint);

        while (!transitionPoint.TransitionBehavior.IsDone)
        {
            transitionPoint.TransitionBehavior.Tick();
            yield return new WaitForEndOfFrame();
        }

        transitionPoint.TransitionBehavior.EndTransition();

        _transitionState = TransitionState.None;
        transitionPoint.LinkedRoom._transitionState = TransitionState.Within;
    }

    public void TriggerTransition(RoomTransitionPoint point)
    {
        StartCoroutine(DoTransition(this, point));
    }

    public void EnableSpawners()
    {
        foreach (var enemySpawnerBehaviour in _spawners)
        {
            enemySpawnerBehaviour.ShouldSpawn = true;
        }
    }
}