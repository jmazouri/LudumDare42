using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LD42.Shrinking.Prototypes;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public int MaxRooms = 10;

    public float AmountToShrinkBy;

    public Room StartingRoom;
    public Room[] PreloadedPrefabs;

    private List<Room> _roomPrefabs = new List<Room>();
    private List<Room> _generatedRooms = new List<Room>();
    private Room _activeRoom;
    private float _initialShrinkAmount = 1f;

    private WeightedRandomizer<Room> _weights = new WeightedRandomizer<Room>();

	// Use this for initialization
	void Start ()
    {
        if (PreloadedPrefabs != null)
        {
            _roomPrefabs.AddRange(PreloadedPrefabs);
        }

        _roomPrefabs.AddRange(Resources.LoadAll<Room>("RoomPrefabs"));

        if (_roomPrefabs.Count == 0)
        {
            Debug.LogError("No room prefabs found - make sure they are placed in \"Resources/RoomPrefabs\" have a root Room component.");
        }

        if (StartingRoom == null)
        {
            StartingRoom = FindObjectsOfType<Room>()?.FirstOrDefault(d => d.IsInitialisingRoom);
        }

        if (StartingRoom == null)
        {
            Debug.LogError("Generator could not find starting room, and one was not set - did you forget to set IsInitialisingRoom? Disabling.", gameObject);

            gameObject.SetActive(false);
            return;
        }

        _activeRoom = StartingRoom;
        InitialGeneration();
	}

    int depth = 0;

    private void InitialGeneration()
    {
        foreach (var prefab in _roomPrefabs)
        {
            _weights.Weights.Add(prefab, Mathf.RoundToInt((1f / _roomPrefabs.Count) * 100));
        }

        while (_generatedRooms.Count < MaxRooms)
        {
            var exits = _activeRoom.TransitionPoints.Where(d => d.IsViableExit).ToArray();
            
            if (exits.Length == 0)
            {
                Debug.LogError($"No exits found on room \"{_activeRoom.name}\" - stopping generation", _activeRoom.gameObject);
                break;
            }

            var thisRoom = _activeRoom;

            foreach (var exit in exits)
            {
                exit.LinkedRoom = GenerateNext(thisRoom);
                depth++;
            }

            depth = 0;
        }

        SealRooms();
    }

    private void SealRooms()
    {
        foreach (var unlinked in _generatedRooms
            .SelectMany(d=>d.TransitionPoints).Where(d => d.LinkedRoom == null))
        {
            unlinked.PlayerCanUse = false;
        }
    }
	
    private GameObject GetNextPrefab()
    {
        var picked = _weights.TakeOne();
        _weights.Weights[picked] = Mathf.RoundToInt(_weights.Weights[picked] * 0.8f);

        return picked.gameObject;
    }

    private void ShrinkRooms()
    {
        foreach (var room in _generatedRooms)
        {
            _initialShrinkAmount += AmountToShrinkBy;
            var shrinkComponent = room.GetComponent<Shrink>();
            shrinkComponent.ShrinkAmount += _initialShrinkAmount;
            shrinkComponent.ShrinkItem();
        }
    }

    public Room GenerateNext(Room source)
    {
        var prefab = GetNextPrefab();

        var instance = Instantiate(prefab, transform, false);
        var roomInstance = instance.GetComponent<Room>();

        if (ReferenceEquals(instance.GetComponent<Shrink>(), null))
        {
            instance.AddComponent<Shrink>();
        }

        roomInstance.name = "Room " + (_generatedRooms.Count + 1);

        var entrances = roomInstance.TransitionPoints.Where(d => d.IsViableEntrance).ToArray();

        if (entrances.Length == 0)
        {
            Debug.LogError($"Chosen room prefab {prefab.name} has no viable entrances.", prefab);
            return null;
        }

        var wallCollider = instance.GetComponentInChildren<CompositeCollider2D>();

        int entranceIndex = Random.Range(0, entrances.Length);
        instance.transform.position = new Vector3(_activeRoom.transform.position.x + wallCollider.bounds.size.x + 5, 0, 0);

        var chosenEntrance = entrances[entranceIndex];
        chosenEntrance.LinkedRoom = source;
        chosenEntrance.PlayerCanUse = true;
        chosenEntrance.EntryDialog = "What the FUCKing SHIT?!";

        _activeRoom = instance.GetComponent<Room>();
        _generatedRooms.Add(_activeRoom);
        return _activeRoom;
    }

	// Update is called once per frame
	void Update ()
    {
		
	}
}
