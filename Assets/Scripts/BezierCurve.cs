using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    [SerializeField] private List<Vector3> _points = new();
    private float _length;

    public void Init(Vector3 origin, Vector3 target)
    {
        _length = 0f;
        _points.Add(origin);
        _points.Add(target);
    }

    public void AddControlPoints(Vector3 control) => _points.Insert(_points.Count - 1, control);

    public void ApproximatePath()
    {
        for (int i = 0; i < 10; i++)
            _length += Vector3.Distance(Evaluate(_points, i / 10f)[0], Evaluate(_points, (i + 1) / 10f)[0]);
    }

    public Vector3 Evaluate(float t, float speed) => Evaluate(_points, t * speed / _length)[0];

    public List<Vector3> Evaluate(List<Vector3> step, float t)
    {
        List<Vector3> nextStep = new();
        for (int i = 0; i < step.Count - 1; i++)
            nextStep.Add(Vector3.Lerp(step[i], step[i + 1], t));
        if (nextStep.Count > 1) nextStep = Evaluate(nextStep, t);
        return nextStep;
    }
}
