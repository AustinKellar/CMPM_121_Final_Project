using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerGrounded))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerAnimationDelegator))]
public class PlayerJump : MonoBehaviour
{
    [SerializeField]
    private float _jumpForce;

    private PlayerGrounded _grounded;
    private PlayerInput _input;
    private PlayerAnimationDelegator _animation;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _grounded = GetComponent<PlayerGrounded>();
        _input = GetComponent<PlayerInput>();
        _animation = GetComponent<PlayerAnimationDelegator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_input.Jump && _grounded.IsGrounded)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _animation.SetTrigger("Jumping");
        }
    }
}
