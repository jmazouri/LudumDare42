using System.Collections;
using System.Collections.Generic;
using LD42.AI.Prototypes;
using UnityEngine;

public class EnemySpawnerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] public int _amountOfEnemiesToSpawn;
    [SerializeField] public float _cooldownBetweenSpawns;
    [SerializeField] private bool _shouldSpawn;
    private float _timeSinceLastSpawn;

    public List<BasicEnemyBehaviour> Enemies { get; set; } = new List<BasicEnemyBehaviour>();

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
            var enemy = Instantiate(_enemyPrefab, transform.position, transform.rotation);

            Enemies.Add(enemy.GetComponent<BasicEnemyBehaviour>());

            _timeSinceLastSpawn = 0f;
            _amountOfEnemiesToSpawn--;
        }
        else
        {
            _timeSinceLastSpawn += Time.deltaTime;
        }
    }
}