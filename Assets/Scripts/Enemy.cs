using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Bezier settings")]
    [SerializeField] private BezierCurve _bezier;
    [SerializeField] private float _minRadiusControlPoint;
    [SerializeField] private float _maxRadiusControlPoint;

    [Header("UI")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _targetTMP;

    [Header("Enemy stats")]
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;
    private float _timeSinceSpawn;
    private float _speed;

    [Header("Word info")]
    [SerializeField] private string _target;
    [SerializeField] private int _index;


    private void Start()
    {
        InitBezier();
        do
        {
            _target = WordsList.Instance.GetRandomWord();
        } while (WordManager.Instance.IsWordUsed(_target));
        UpdateWordVisual();
        SpawnManager.Instance.AddEnemy(this);
        _index = 0;
        _speed = -0.346f * _target.Length + 5.346f; // 5-0.5
        _timeSinceSpawn = 0f;
    }

    private void Update()
    {
        _timeSinceSpawn += Time.deltaTime;
        transform.position = _bezier.Evaluate(_timeSinceSpawn, _speed);
        _canvas.sortingOrder = 2500 - Mathf.RoundToInt(Vector3.Distance(transform.position, Vector3.zero) * 100);
    }

    private void OnDestroy() => SpawnManager.Instance.RemoveEnemy(this);

    private void InitBezier()
    {
        _bezier.Init(transform.position, Vector3.zero);
        AddControlPoints();
        _bezier.ApproximatePath();
    }

    private void AddControlPoints()
    {
        Vector3 dir = (Vector3.zero - transform.position).normalized;
        Vector3 axis = Vector3.Cross(dir, Vector3.up);
        for (int i = 1; i < 4; i++)
        {
            Vector3 pt = Vector3.Lerp(transform.position, Vector3.zero, .25f * i);
            float side = Mathf.Sign(Random.Range(-1f, 1f));
            pt += side * Random.Range(_minRadiusControlPoint, _maxRadiusControlPoint) * axis;
            _bezier.AddControlPoints(pt);
        }
    }

    public (bool progress, int index) CheckNextLetter(char nextLetter)
    {
        if (_target[_index] == nextLetter)
        {
            _index++;
            return (true, _index);
        }
        return (false, 0);
    }

    public void UpdateWordVisual()
    {
        _targetTMP.text = "<color=yellow>" + _target.Insert(_index, "</color>");
        if (_index == _target.Length)
        {
            WordManager.Instance.ValidateWord();
            SelfKill();
        }
    }

    public void ResetWordCompletion()
    {
        _targetTMP.text = _target;
        _index = 0;
    }

    private void SelfKill() => Destroy(gameObject);
    public string GetWord() => _target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.Hit();
            SelfKill();
        }
    }
}
