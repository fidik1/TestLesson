using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PlayerRagdoll : MonoBehaviour
{
    [SerializeField] private ThirdPersonController _thirdPersonController;
    [SerializeField] private Animator _animator1;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _mainTransform;
    [SerializeField] private Transform _camera;

    [SerializeField] private Collider[] _colliders;
    [SerializeField] private Rigidbody[] _rigidBodies;

    private List<Vector3> _startPositions = new();
    private List<Vector3> _startRotations = new();

    public bool IsRagdoll => _isRagdoll;
    private bool _isRagdoll;

    private Vector3 _cameraOffset;

    private void Start()
    {
        _cameraOffset = _camera.position - _mainTransform.position;
        foreach (Collider collider in _colliders)
        {
            _startPositions.Add(collider.transform.localPosition);
            _startRotations.Add(collider.transform.localEulerAngles);
        }
        SetActiveRagdoll(false);
    }

    public void SetActiveRagdoll(bool active)
    {
        if (active)
        {
            _isRagdoll = true;
            _characterController.enabled = false;
            _animator.enabled = false;
            foreach (Collider collider in _colliders)
            {
                collider.enabled = true;
            }
            foreach (Rigidbody rb in _rigidBodies)
            {
                rb.isKinematic = false;
            }
            StartCoroutine(CheckGrounded());
        }
        else
        {
            for (int i = 0; i < _colliders.Length; i++)
            {
                _colliders[i].transform.SetLocalPositionAndRotation(_startPositions[i], Quaternion.Euler(_startRotations[i]));
            }
            _isRagdoll = false;
            _characterController.enabled = true;
            _animator.enabled = true;
            foreach (Collider collider in _colliders)
            {
                collider.enabled = false;
            }
            foreach (Rigidbody rb in _rigidBodies)
            {
                rb.isKinematic = true;
            }
        }
    }

    public void AddForce(Vector3 vector)
    {
        foreach (Rigidbody rb in _rigidBodies)
        {
            rb.AddForce(vector, ForceMode.Impulse);
        }
    }

    public bool Grounded;
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.28f;
    public LayerMask GroundLayers;

    private float _timeElapsed;

    private IEnumerator CheckGrounded()
    {
        if (!_thirdPersonController.enabled) yield break;
        _timeElapsed = 0;
        _thirdPersonController.enabled = false;
        while (!Grounded && _rigidBodies[0].velocity.y > 1)
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            _camera.localPosition = _mainTransform.position + _cameraOffset;
            yield return null;
        }

        while (_timeElapsed < 2)
        {
            _camera.position = _mainTransform.position + _cameraOffset;
            _timeElapsed += Time.deltaTime;
            yield return null;
        }

        _camera.localPosition = new(0, 1.375f, 0);
        transform.position = new(_mainTransform.position.x, _mainTransform.position.y, _mainTransform.position.z);
        SetActiveRagdoll(false);
        _thirdPersonController.enabled = true;
    }
}
