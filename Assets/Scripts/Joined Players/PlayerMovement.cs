using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerGrounded))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _spawnFallSpeed;

    [SerializeField]
    private float _normalFallSpeed;

    private PlayerInput _input;
    private PlayerGrounded _grounded;
    private Rigidbody _rigidbody;

    private bool _isSpawning;

    public bool HasMoved { get; private set; }

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        _grounded = GetComponent<PlayerGrounded>();
        _isSpawning = true;
        Physics.gravity = new Vector3(0, -_spawnFallSpeed, 0);
    }

    private void Start()
    {
        HasMoved = false;
    }

    private void FixedUpdate()
    {
        if (_isSpawning && _grounded.IsGrounded)
        {
            _isSpawning = false;
            Physics.gravity = new Vector3(0, -_normalFallSpeed, 0);
        }

        if (!HasMoved && Mathf.Abs(_input.HorizontalMovement) > 0.1f)
        {
            HasMoved = true;
        }

        _rigidbody.position = new Vector3(transform.position.x + _input.HorizontalMovement * _speed, transform.position.y, transform.position.z);
    }
}
