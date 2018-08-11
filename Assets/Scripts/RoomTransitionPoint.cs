using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransitionPoint : MonoBehaviour
{
    public Room LinkedRoom;

    public TransitionBehavior TransitionBehavior;
    public bool Demo;
    public bool PlayerCanUse = true;

    public Room ParentRoom { get; private set; }

    private void Start()
    {
        ParentRoom = transform.GetComponentInParent<Room>();

        if (Demo)
        {
            //TODO: Remove this, only for demo purposes
            ParentRoom.TriggerTransition(this);
        }

        if (LinkedRoom == null)
        {
            if (PlayerCanUse)
            {
                Debug.LogError($"Linked room wasn't set for point \"{name}\" in room \"{ParentRoom.name}\" - please link to another room", gameObject);
            }
            else
            {
                Debug.LogWarning("Linked room wasn't set for non-enterable point - probably fine, just FYI", gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (!PlayerCanUse)
        {
            Debug.LogWarning("Player was denied entry to transtion point", gameObject);
            return;
        }

        ParentRoom.TriggerTransition(this);
    }
}
