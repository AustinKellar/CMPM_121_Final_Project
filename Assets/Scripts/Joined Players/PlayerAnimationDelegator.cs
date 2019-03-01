using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerGrounded))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerAnimationDelegator : MonoBehaviour
{
    [SerializeField]
    private Animator _gooController;

    [SerializeField]
    private Animator _bonesController;

    private PlayerGrounded _playerGrounded;
    private PlayerInput _input;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _playerGrounded = GetComponent<PlayerGrounded>();
        _input = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        SetBool("Grounded", _playerGrounded.IsGrounded);

        bool running = Mathf.Abs(_input.HorizontalMovement) > 0.0001f;
        SetBool("Running", running);
    }

    public void SetTrigger(string anim)
    {
        _gooController.SetTrigger(anim);
        _bonesController.SetTrigger(anim);
    }

    private void SetBool(string anim, bool value)
    {
        _gooController.SetBool(anim, value);
        _bonesController.SetBool(anim, value);
    }
}
