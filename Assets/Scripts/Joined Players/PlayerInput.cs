using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public PlayerIndex Index { get; set; }
    public float HorizontalMovement { get; private set; }
    public bool Jump { get; private set; }
    public bool Dash { get; private set; }

    private void Update()
    {
        GamePadState state = GamePad.GetState(Index);
        HorizontalMovement = state.GetAxis(CAxis.LX);
        Jump = state.Pressed(CButton.A);
        Dash = state.Pressed(CButton.X);
    }
}
