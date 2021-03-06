﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TransitionPointMode
{
    Entrance,
    Exit
}

public class RoomTransitionPoint : MonoBehaviour
{
    public TransitionPointMode TransitionPointMode;

    public TransitionBehavior TransitionBehavior;
    public bool Demo;
    public bool PlayerCanUse = true;

    public Room LinkedRoom;
    public Room ParentRoom { get; private set; }

    public float CooldownTime = 1.0f;
    public bool OnCooldown => CooldownTime > 0;

    private Collider2D _collider;

    public bool IsViableEntrance => TransitionPointMode == TransitionPointMode.Entrance;
    public bool IsViableExit => TransitionPointMode == TransitionPointMode.Exit;

    public bool WasDialogActivated { get; set; } = false;
    public Dictionary<string, DartisMood> EntryDialog { get; set; } = new Dictionary<string, DartisMood>();

    private static GameObject _tpPrefab;
    private SpriteRenderer _tpSprite;

    private void Start()
    {
        if (_tpPrefab == null)
        {
            _tpPrefab = (GameObject)Resources.Load("Teleporter");
        }

        _collider = GetComponent<Collider2D>();
        ParentRoom = transform.GetComponentInParent<Room>();

        if (Demo)
        {
            //TODO: Remove this, only for demo purposes
            ParentRoom.TriggerTransition(this);
        }
        
        if (_collider != null && !_collider.isTrigger)
        {
            Debug.LogWarning($"Collider on transition point \"{name}\" is not a trigger, but it needs to be.", gameObject);
        }

        if (PlayerCanUse && _collider == null)
        {
            Debug.LogWarning($"Transition point \"{name}\" was marked as player usable but has no Collider2D, so nothing will happen.", gameObject);
        }

        if (GetComponent<SpriteRenderer>() == null)
        {
            var obj = Instantiate(_tpPrefab, transform, true);
            obj.transform.position = transform.position;
            obj.transform.localScale = _collider.bounds.size * 0.66f;

            _tpSprite = obj.GetComponent<SpriteRenderer>();
        }
    }

    private bool EnemiesDead =>
        ParentRoom._spawners.Length == 0 ||
        !ParentRoom._spawners.SelectMany(d => d.Enemies).Any(d => d != null);

    private void Update()
    {
        if (CooldownTime >= 0)
        {
            CooldownTime  -= Time.deltaTime;
        }

        if (!EnemiesDead || OnCooldown)
        {
            _tpSprite.color = new Color(1, 0.25f, 0.25f, 1);
            _tpSprite.GetComponent<Animator>().speed = 0;
        }
        else
        {
            _tpSprite.color = new Color(1, 1, 1, 1);
            _tpSprite.GetComponent<Animator>().speed = 1;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (OnCooldown) return;

        if (!EnemiesDead)
        {
            Debug.Log("Player was denied entry to transition because not all enemies are dead");
            return;
        }

        if (!PlayerCanUse)
        {
            Debug.LogWarning("Player was denied entry to transition point", gameObject);
            return;
        }

        if (LinkedRoom == null)
        {
            if (PlayerCanUse)
            {
                Debug.LogError($"Linked room wasn't set for point \"{name}\" in room \"{ParentRoom.name}\" - please link to another room", gameObject);
                return;
            }
            else
            {
                Debug.LogWarning("Linked room wasn't set for non-usable point - probably fine, just FYI", gameObject);
            }
        }

        ParentRoom.TriggerTransition(this);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        _collider = GetComponent<Collider2D>();

        Gizmos.color = Color.green;
        var size = new Vector3(1, 1, 1);

        if (_collider != null)
        {
            size = _collider.bounds.size * 0.95f;
        }

        if (!PlayerCanUse)
        {
            Gizmos.color = Color.gray;
        }
        else
        {
            if (OnCooldown)
            {
                Gizmos.DrawIcon(transform.position, "CollabProgress");
            }
            else
            {
                switch (TransitionPointMode)
                {
                    case TransitionPointMode.Entrance:
                        Gizmos.DrawIcon(transform.position, "CollabPull");
                        break;
                    case TransitionPointMode.Exit:
                        Gizmos.DrawIcon(transform.position, "CollabPush");
                        break;
                    default:
                        break;
                }
            }
        }
        
        Gizmos.DrawCube(transform.position, size);
    }
#endif
}
