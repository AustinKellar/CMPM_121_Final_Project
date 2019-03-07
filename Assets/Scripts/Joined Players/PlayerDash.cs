using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerAnimationDelegator))]
public class PlayerDash : MonoBehaviour
{
    [SerializeField]
    private float _dashForce;

    [SerializeField]
    private float _dashCooldown;

    private PlayerInput _input;
    private PlayerAnimationDelegator _animation;
    private Rigidbody _rigidbody;

    private float _previousDashTime;

    public bool IsDashing { get; private set; }

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _animation = GetComponent<PlayerAnimationDelegator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_input.Dash && !IsDashing && Time.time > _previousDashTime + _dashCooldown)
        {
            _rigidbody.useGravity = false;
            IsDashing = true;
            _previousDashTime = Time.time;
            _animation.SetTrigger("Dashing");
            AudioManager.Instance.PlayOneShot("Character Dash");

            if (_input.HorizontalMovement > 0)
            {
                _rigidbody.velocity = new Vector3(_dashForce, 0, 0);
            }
            else if (_input.HorizontalMovement < 0)
            {
                _rigidbody.velocity = new Vector3(-_dashForce, 0, 0);
            }
        }
    }

    private void FixedUpdate()
    {
        if (IsDashing)
        {
            _rigidbody.velocity = _rigidbody.velocity * 0.5f;
            if (Mathf.Abs(_rigidbody.velocity.x) < 0.1f)
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.useGravity = true;
                IsDashing = false;
            }
        }
    }
}
