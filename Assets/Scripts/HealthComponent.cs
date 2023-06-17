using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private List<GameObject> _lights;

    private int _currentLife = 5;
    private int _maxLife;

    private void Awake()
    {
        _maxLife = _lights.Count;
    }

    void Start()
    {
        _currentLife = _maxLife;
        foreach (var l in  _lights)
            l.SetActive(true);
    }

    public bool GetHit()
    {
        if (_currentLife <= 0) return true;
        Debug.Log("Player got hit");
        _currentLife--;
        _lights[_currentLife].SetActive(false);
        return _currentLife <= 0;
    }
}
