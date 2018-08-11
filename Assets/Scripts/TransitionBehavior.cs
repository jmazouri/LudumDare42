﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LD42.PlayerControllers;
using UnityEngine;

public abstract class TransitionBehavior : ScriptableObject
{
    public float Speed;

    [NonSerialized]
    public bool IsDone;
    [NonSerialized]
    public Room FromRoom;
    [NonSerialized]
    public Room ToRoom;
    [NonSerialized]
    public RoomTransitionPoint TransitionPoint;

    public virtual void StartTransition(Room fromRoom, Room toRoom, RoomTransitionPoint transitionPoint)
    {
        IsDone = false;
        FromRoom = fromRoom;
        ToRoom = toRoom;
        TransitionPoint = transitionPoint;

        Time.timeScale = 0;
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

        var targetPosition = ToRoom.TransitionPoints
            .FirstOrDefault(d => d.LinkedRoom == FromRoom).transform.position;

        if (targetPosition == null)
        {
            Debug.LogError($"Couldn't find a transition point in the linked room ({ToRoom.name}) that is linked to this room ({FromRoom.name}).", ToRoom.gameObject);
        }

        playerController.PlayerTransform.position = targetPosition;
        ToRoom.PlayerController = playerController;
    }

    public virtual void EndTransition()
    {
        Time.timeScale = 1;
    }

    public void Tick()
    {
        InternalTick(Time.unscaledDeltaTime * Speed);
    }

    public abstract void InternalTick(float deltaTime);
}
