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
        Jump = state.Pressed(CButton.A) || (Index == PlayerIndex.One && Input.GetKeyDown(KeyCode.Space) || (Index == PlayerIndex.Two && Input.GetKeyDown(KeyCode.UpArrow)));
        Dash = state.Pressed(CButton.X) || (Index == PlayerIndex.One && Input.GetKeyDown(KeyCode.LeftShift) || (Index == PlayerIndex.Two && Input.GetKeyDown(KeyCode.RightShift)));
    }
}
