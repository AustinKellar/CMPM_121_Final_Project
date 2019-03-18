using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMenuInput : MonoBehaviour
{
    private PlayerIndex _index;
    private bool _downPressedLastFrame;
    private bool _upPressedLastFrame;
    private bool _executePressedLastFrame;
    private GamePadState _gamePadState;

    public bool CloseMenuPressed { get; private set; }
    public bool UpButtonPressed { get; private set; }
    public bool DownButtonPressed { get; private set; }
    public bool ExecuteButtonPressed { get; private set; }

    private void Update()
    {
        if (LobbyStateManager.Instance.State == LobbyStateManager.LobbyState.ColorSelect || !LobbyStateManager.Instance.CameraAtDestination)
        {
            CloseMenuPressed = UpButtonPressed = DownButtonPressed = ExecuteButtonPressed = false;
            return;
        }

        _gamePadState = GamePad.GetState(_index);
        CaptureInput();
    }
    public void SetPlayerIndex(PlayerIndex index)
    {
        _index = index;
    }

    private void CaptureInput()
    {
        CloseMenuPressed = _gamePadState.Pressed(CButton.B);

        bool pressedDown = _gamePadState.GetAxis(CAxis.DY) == -1f || _gamePadState.GetAxis(CAxis.LY) == 1f;
        DownButtonPressed = ((pressedDown && !_downPressedLastFrame) || Input.GetKeyDown(KeyCode.DownArrow));
        _downPressedLastFrame = pressedDown;

        bool pressedUp = _gamePadState.GetAxis(CAxis.DY) == 1f || _gamePadState.GetAxis(CAxis.LY) == -1f;
        UpButtonPressed = ((pressedUp && !_upPressedLastFrame) || Input.GetKeyDown(KeyCode.UpArrow));
        _upPressedLastFrame = pressedUp;

        ExecuteButtonPressed = _gamePadState.Pressed(CButton.A) || Input.GetKeyDown(KeyCode.RightShift);
    }
}
