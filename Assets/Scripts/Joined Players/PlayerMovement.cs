using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private PlayerInput _input;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rigidbody.position = new Vector3(transform.position.x + _input.HorizontalMovement * _speed, transform.position.y, transform.position.z);
    }
}
