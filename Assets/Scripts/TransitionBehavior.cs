using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    public virtual void MoveCamera()
    {
        Camera.main.transform.position = new Vector3(ToRoom.transform.position.x, ToRoom.transform.position.y, Camera.main.transform.position.z);
        ToRoom.TeleportPlayer(FromRoom.PlayerController);
        FromRoom.PlayerController = null;
    }

    public virtual void EndTransition() { }

    public void Tick()
    {
        InternalTick(Time.deltaTime * Speed);
    }

    public abstract void InternalTick(float deltaTime);
}
