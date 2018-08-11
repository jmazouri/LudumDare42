using System.Collections;
using System.Collections.Generic;
using LD42.PlayerControllers;
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
    [SerializeField] private TransitionState _transitionState = TransitionState.None;
    
    public TransitionState TransitionState => _transitionState;

    [SerializeField] private GameObject _initialPlayer;
    [SerializeField] private bool _isInitialisingRoom;

    [SerializeField] private GameObject _entryPoint;

    [SerializeField] private EnemySpawnerBehaviour[] _spawners;

    public IPlayerController PlayerController { get; set; }

    void Start()
    {
        if (!_isInitialisingRoom) return;

        PlayerController = _initialPlayer.GetComponent<IPlayerController>();
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
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

    public void TeleportPlayer(IPlayerController playerController)
    {
        if (playerController == null)
        {
            Debug.LogError("No IPlayerController is present in the scene, cannot transition");
            return;
        }

        PlayerController = playerController;
        PlayerController.PlayerTransform.position = _entryPoint.transform.position;
    }

    public void EnableSpawners()
    {
        foreach (var enemySpawnerBehaviour in _spawners)
        {
            enemySpawnerBehaviour.ShouldSpawn = true;
        }
    }
}