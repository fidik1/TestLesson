using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTriggerObstacle : MonoBehaviour
{
    [SerializeField] private SpinnerObstacle _spinnerObstacle;

    private void OnTriggerEnter(Collider other)
    {
        _spinnerObstacle.OnChildTriggerEnter(other);
    }
}
