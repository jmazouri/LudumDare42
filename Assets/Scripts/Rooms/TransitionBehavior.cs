﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LD42.PlayerControllers;
using UnityEngine;

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

    public virtual void StartTransition(Room fromRoom, Room toRoom, RoomTransitionPoint transitionPoint)
    {
        IsDone = false;
        FromRoom = fromRoom;
        ToRoom = toRoom;
        TransitionPoint = transitionPoint;

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

        var targetPoint = ToRoom.TransitionPoints.FirstOrDefault(d => d.LinkedRoom == FromRoom);

        if (targetPoint == null)
        {
            Debug.LogError($"Couldn't find a transition point in the linked room ({ToRoom.name}) that is linked to this room ({FromRoom.name}).", ToRoom.gameObject);
            return;
        }

        targetPoint.CooldownTime = 1f;

        playerController.PlayerTransform.position = targetPoint.transform.position;
        //playerController.Controller.velocity = new Vector2();
        playerController.Controller.move(new Vector3());
        playerController.PlayerVelocity = new Vector3();
        Input.ResetInputAxes();

        ToRoom.PlayerController = playerController;
    }

    public virtual void EndTransition()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
        _startTransition = false;
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
