using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _targetTMP;
    [SerializeField] private float _speed;

    private BezierCurve _curve;
    private float _timeSinceSpawn;
    private string _target;

    private void Start()
    {
        InitBezierCurve();
        do
        {
            _target = WordsList.Instance.GetRandomWord();
        } while (WordManager.Instance.IsWordUsed(_target));
        UpdateWordCompletion("");
        SpawnManager.Instance.AddEnemy(this);
        _timeSinceSpawn = 0f;
    }

    private void InitBezierCurve()
    {
        _curve = gameObject.AddComponent<BezierCurve>();
        _curve.Init(transform.position, Vector3.zero);

        // Add controlpoints here

        _curve.ApproximateLength();
    }

    private void Update()
    {
        _timeSinceSpawn += Time.deltaTime;
        transform.position = _curve.Evaluate(_timeSinceSpawn, _speed);
        _canvas.sortingOrder = 2500 - Mathf.RoundToInt(Vector3.Distance(transform.position, Vector3.zero) * 100);
    }

    private void OnDestroy() => SpawnManager.Instance.RemoveEnemy(this);

    public bool UpdateWordCompletion(string _typing)
    {
        if (_target.StartsWith(_typing))
        {
            _targetTMP.text = "<color=yellow>" + _target.Insert(_typing.Length, "</color>");
            if (_typing == _target)
            {
                WordManager.Instance.ValidateWord();
                SelfKill();
            }
            else 
                return true;
        }
        _targetTMP.text = _target;
        return false;
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
