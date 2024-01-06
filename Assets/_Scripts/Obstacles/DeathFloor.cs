using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFloor : MonoBehaviour
{
    [SerializeField] private Vector3 _spawnPosition;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.position = _spawnPosition;
        }
    }
}
