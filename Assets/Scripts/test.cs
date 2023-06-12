using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private GameObject pfAxis;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = transform.position, target = Vector3.zero;

        Vector3 dir = target - pos;
        Vector3 normalizedDir = dir.normalized;
        Vector3 axisDir = Vector3.Cross(normalizedDir, Vector3.up);

        Vector3 pA = Vector3.Lerp(pos, target, .25f);
        Vector3 pB = Vector3.Lerp(pos, target, .5f);
        Vector3 pC = Vector3.Lerp(pos, target, .75f);

        pA += axisDir;
        pB -= axisDir;
        pC += axisDir;

        Instantiate(pfAxis, pA, Quaternion.identity);
        Instantiate(pfAxis, pB, Quaternion.identity);
        Instantiate(pfAxis, pC, Quaternion.identity);
    }
}
