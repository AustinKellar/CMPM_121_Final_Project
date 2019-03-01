using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class TemporaryRotator : MonoBehaviour
{
    [SerializeField]
    private Transform model;

    [SerializeField]
    private bool _facingRight = false;

    private Vector3 _startScale;
    private Rigidbody _rigidbody;
    private PlayerInput _input;

    private void Awake()
    {
        _startScale = model.transform.localScale;
        _rigidbody = GetComponent<Rigidbody>();
        _input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (_input.HorizontalMovement < -0.1f)
        {
            model.localScale = new Vector3(_startScale.x, _startScale.y, _startScale.z * -1f);
        }
        else if (_input.HorizontalMovement > 0.1f)
        {
            model.localScale = _startScale;
        }
    }
}
