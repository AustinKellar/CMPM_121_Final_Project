using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerGrounded))]
public class PlayerInput : MonoBehaviour
{
    private bool _movementLocked = true;
    private PlayerGrounded _grounded;

    public PlayerIndex Index { get; set; }
    public float HorizontalMovement { get; private set; }
    public bool Jump { get; private set; }
    public bool Dash { get; private set; }

    private void Awake()
    {
        _grounded = GetComponent<PlayerGrounded>();
    }

    private void Update()
    {
        if (Index == PlayerIndex.Three || Index == PlayerIndex.Four)
        {
            return;
        }

        if (_movementLocked)
        {
            if (!_grounded.IsGrounded)
            {
                return;
            }
            else
            {
                _movementLocked = false;
            }
        }

        GamePadState state = GamePad.GetState(Index);

        if (Index == PlayerIndex.One)
        {
            if (Input.GetKey(KeyCode.D))
            {
                HorizontalMovement = 1f;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                HorizontalMovement = -1f;
            }
            else
            {
                HorizontalMovement = 0f;
            }
        }
        else if (Index == PlayerIndex.Two)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                HorizontalMovement = 1f;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                HorizontalMovement = -1f;
            }
            else
            {
                HorizontalMovement = 0f;
            }
        }

        if (HorizontalMovement != 1 && HorizontalMovement != -1)
        {
            HorizontalMovement = state.GetAxis(CAxis.LX);
        }


        Jump = state.Pressed(CButton.A) || (Index == PlayerIndex.One && Input.GetKeyDown(KeyCode.Space) || (Index == PlayerIndex.Two && Input.GetKeyDown(KeyCode.UpArrow)));
        Dash = state.Pressed(CButton.X) || (Index == PlayerIndex.One && Input.GetKeyDown(KeyCode.LeftShift) || (Index == PlayerIndex.Two && Input.GetKeyDown(KeyCode.RightShift)));
    }
}
