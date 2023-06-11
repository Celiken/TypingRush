using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Vector3 _speed;

    // Update is called once per frame
    void Update() => transform.Rotate(_speed * Time.deltaTime);
}
