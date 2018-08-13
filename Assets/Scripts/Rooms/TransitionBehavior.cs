using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LD42.PlayerControllers;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TransitionBehavior : ScriptableObject
{
    [Header("A multiplier - 0.5 is half, 2 is double")]
    public float Speed;

    [NonSerialized]
    public bool IsDone;
    [NonSerialized]
    public Room FromRoom;
    [NonSerialized]
    public Room ToRoom;
    [NonSerialized]
    public RoomTransitionPoint TransitionPoint;
    [NonSerialized]
    private bool _startTransition;
    
    public GameUIController UIController => FindObjectOfType<GameUIController>();

    public RoomTransitionPoint TargetTransitionPoint => 
        ToRoom.TransitionPoints.FirstOrDefault(d => d.LinkedRoom == FromRoom);

    public virtual void StartTransition(Room fromRoom, Room toRoom, RoomTransitionPoint transitionPoint)
    {
        IsDone = false;
        FromRoom = fromRoom;
        ToRoom = toRoom;
        TransitionPoint = transitionPoint;

        UIController.CancelAndClearDialog();

        LeanTween.value(FromRoom.gameObject, update =>
        {
            Time.timeScale = update;
            Time.fixedDeltaTime = update * 0.02f;
        }, 1, 0, 0.5f)
        .setEase(LeanTweenType.easeOutExpo)
        .setIgnoreTimeScale(true)
        .setOnComplete(() => _startTransition = true);

        
    }

    public virtual void MoveCamera()
    {
        Camera.main.transform.position = new Vector3(ToRoom.transform.position.x, ToRoom.transform.position.y, Camera.main.transform.position.z);
        TeleportPlayer();

        ToRoom.EnableSpawners();
        FromRoom.PlayerController = null;
    }

    public virtual void TeleportPlayer()
    {
        var playerController = FromRoom.PlayerController;

        if (playerController == null)
        {
            Debug.LogError($"No IPlayerController is present in the room \"{FromRoom.name}\", cannot teleport");
            return;
        }

        if (TargetTransitionPoint == null)
        {
            Debug.LogError($"Couldn't find a transition point in the linked room ({ToRoom.name}) that is linked to this room ({FromRoom.name}).", ToRoom.gameObject);
            return;
        }

        TargetTransitionPoint.CooldownTime = 2.5f;

        playerController.PlayerTransform.position = TargetTransitionPoint.transform.position;
        playerController.ClearVelocityAndInput();

        ToRoom.PlayerController = playerController;
    }

    public virtual void EndTransition()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
        _startTransition = false;

        if (ToRoom._spawners.Length > 0)
        {
            UIController.TriggerMusic();
        }

        if (TargetTransitionPoint.EntryDialog.Count > 0 && !TargetTransitionPoint.WasDialogActivated)
        {
            UIController.UIState = UIState.Dialog;

            TargetTransitionPoint.WasDialogActivated = true;

            foreach (var line in TargetTransitionPoint.EntryDialog)
            {
                UIController.QueueNewDialogueText(line.Key, line.Value);
            }
        }
        else if (UIController.UIState != UIState.InGame)
        {
            UIController.UIState = UIState.InGame;
        }
    }

    public void Tick()
    {
        if (_startTransition)
        {
            InternalTick(Time.unscaledDeltaTime * Speed);
        }
    }

    public abstract void InternalTick(float deltaTime);
}
