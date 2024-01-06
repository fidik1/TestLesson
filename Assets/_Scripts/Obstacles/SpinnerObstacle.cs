using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpinnerObstacle : MonoBehaviour
{
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private GameObject _particles;
    [SerializeField] private Transform _object;
    [SerializeField] private bool _isMirrored;
    [SerializeField] private float _speed = 2;

    private float _startRotation;

    private void Start()
    {
        if (_isMirrored)
        {
            _startRotation = -360;
        }
        else
        {
            _startRotation = 360;
        }
        _object.eulerAngles = new(0, -_startRotation, 0);
        _object.DOLocalRotate(new(0, _startRotation, 0), _speed, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetRelative().SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
            PlayerRagdoll ragdoll = other.GetComponent<PlayerRagdoll>();
            ragdoll.SetActiveRagdoll(true);
            ragdoll.AddForce(new(knockbackDirection.x * knockbackForce, other.transform.position.y + 5, knockbackDirection.z * knockbackForce));
            Instantiate(_particles, new Vector3(other.transform.position.x, other.transform.position.y + 1f, other.transform.position.z), Quaternion.identity);
        }
    }

    public void OnChildTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
            PlayerRagdoll ragdoll = other.GetComponent<PlayerRagdoll>();
            ragdoll.SetActiveRagdoll(true);
            ragdoll.AddForce(new(knockbackDirection.x * knockbackForce, other.transform.position.y + 5, knockbackDirection.z * knockbackForce));
            Instantiate(_particles, new Vector3(other.transform.position.x, other.transform.position.y + 1f, other.transform.position.z), Quaternion.identity);
        }
    }
}
