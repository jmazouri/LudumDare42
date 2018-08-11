using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _amountOfEnemiesToSpawn;
    [SerializeField] private float _cooldownBetweenSpawns;
    [SerializeField] private bool _shouldSpawn;
    private float _timeSinceLastSpawn;

    public bool ShouldSpawn
    {
        get { return _shouldSpawn; }
        set { _shouldSpawn = value; }
    }


    void Start()
    {
        _timeSinceLastSpawn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ShouldSpawn || _amountOfEnemiesToSpawn == 0) return;

        if (_timeSinceLastSpawn >= _cooldownBetweenSpawns)
        {
            Instantiate(_enemyPrefab, transform.position, transform.rotation);
            _timeSinceLastSpawn = 0f;
            _amountOfEnemiesToSpawn--;
        }
        else
        {
            _timeSinceLastSpawn += Time.deltaTime;
        }
    }
}