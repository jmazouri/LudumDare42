using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RoomTransitionPoint : MonoBehaviour
{
    public Room NextRoom;
    public TransitionBehavior TransitionBehavior;

    public bool Demo;

    private Room _thisRoom;

    private void Start()
    {
        _thisRoom = transform.GetComponentInParent<Room>();

        if (Demo)
        {
            //TODO: Remove this, only for demo purposes
            _thisRoom.TriggerTransition(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        
        _thisRoom.TriggerTransition(this);
    }
}
