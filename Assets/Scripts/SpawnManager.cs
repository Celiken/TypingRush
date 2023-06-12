using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] private GameObject _pfEnemy;
    public List<Enemy> _enemyList;

    [SerializeField] private Vector2 _minMaxSpawnDelay;
    [SerializeField] private float _spawnDistance;

    [SerializeField] private int _maxSpawnedEnemy;

    private float _nextSpawn = 0f;

    private void Awake()
    {
        Instance = this;
        _nextSpawn = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        _nextSpawn -= Time.deltaTime;
        if (_nextSpawn <= 0f)
        {
            if (_enemyList.Count >= _maxSpawnedEnemy) return;
            _nextSpawn = Random.Range(_minMaxSpawnDelay.x, _minMaxSpawnDelay.y);
            GameObject go = Instantiate(_pfEnemy);
            Vector3 pos = new Vector3(Random.Range(-1f,1f), 0f, Random.Range(-1f, 1f)).normalized * _spawnDistance;
            go.transform.position = pos;
        }
    }

    public void AddEnemy(Enemy enemy) => _enemyList.Add(enemy);
    public void RemoveEnemy(Enemy enemy) => _enemyList.Remove(enemy);

}
