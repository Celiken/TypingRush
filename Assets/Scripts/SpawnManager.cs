using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] private GameObject _pfEnemy;
    public List<Enemy> _enemyList;

    [SerializeField] private float _minSpawnDelay;
    [SerializeField] private float _maxSpawnDelay;
    [SerializeField] private float _minSpawnDistance;
    [SerializeField] private float _maxSpawnDistance;

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
            if (_enemyList.Count > 50) return;
            _nextSpawn = Random.Range(_minSpawnDelay, _maxSpawnDelay);
            GameObject go = Instantiate(_pfEnemy);
            Vector3 pos = new Vector3(Random.Range(-1f,1f), 0f, Random.Range(-1f, 1f)).normalized * Random.Range(_minSpawnDistance, _maxSpawnDistance);
            go.transform.position = pos;
        }
    }

    public void AddEnemy(Enemy enemy) => _enemyList.Add(enemy);
    public void RemoveEnemy(Enemy enemy) => _enemyList.Remove(enemy);

}
