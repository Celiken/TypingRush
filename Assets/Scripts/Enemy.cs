using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _targetTMP;

    [Header("Enemy stats")]
    [SerializeField] private float _speed;
    private float _timeSinceSpawn;

    [Header("Word info")]
    [SerializeField] private string _target;
    [SerializeField] private int _index;


    private void Start()
    {
        do
        {
            _target = WordsList.Instance.GetRandomWord();
        } while (WordManager.Instance.IsWordUsed(_target));
        UpdateWordVisual();
        SpawnManager.Instance.AddEnemy(this);
        _index = 0;
        _timeSinceSpawn = 0f;
    }

    private void Update()
    {
        _timeSinceSpawn += Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, Time.deltaTime);
        _canvas.sortingOrder = 2500 - Mathf.RoundToInt(Vector3.Distance(transform.position, Vector3.zero) * 100);
    }

    private void OnDestroy() => SpawnManager.Instance.RemoveEnemy(this);

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
