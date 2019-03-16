using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControllerSignIn : MonoBehaviour
{
    [SerializeField]
    private float _timeToHoldB;

    private PlayerIndex _index;
    private PlayerSpawner _playerSpawner;
    private float _pressedBTime;

    public bool HasJoined { get; private set; }

    private void Awake()
    {
        HasJoined = false;
        _playerSpawner = FindObjectOfType<PlayerSpawner>();
    }

    private void Update()
    {
        GamePadState state = GamePad.GetState(_index);
        if (state.Pressed(CButton.A))
        {
            _playerSpawner.Spawn(_index, true);
        }
        if (state.Pressed(CButton.B))
        {
            _pressedBTime = Time.time;
            PlayerSelectUIManager.Instance.RemovePlayer(_index);
        }
        if (state.B && Time.time > _pressedBTime + _timeToHoldB)
        {
            if (!PlayerSelectUIManager.Instance.ReadyToStartMatch)
            {
                _playerSpawner.Despawn(_index, true);
            }
        }
        if (state.Back)
        {
            Application.Quit();
        }
        if (state.Pressed(CButton.Start))
        {
            if (PlayerSelectUIManager.Instance.ReadyToStartMatch)
            {
                PlayerSelectUIManager.Instance.StartMatch();
            }

            PlayerInfo player = ActivePlayers.Players.FirstOrDefault(p => p.ControllerNumber == (int)_index);
            if (player != null)
            {
                PlayerSelectUIManager.Instance.ReadyPlayer(_index);
            }
        }

        CaptureKeyboardInput();
    }

    private void CaptureKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _playerSpawner.Spawn(PlayerIndex.One, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _playerSpawner.Spawn(PlayerIndex.Two, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _playerSpawner.Spawn(PlayerIndex.Three, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _playerSpawner.Spawn(PlayerIndex.Four, true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PlayerSelectUIManager.Instance.RemovePlayer(PlayerIndex.One);
            _playerSpawner.Despawn(PlayerIndex.One, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            PlayerSelectUIManager.Instance.RemovePlayer(PlayerIndex.Two);
            _playerSpawner.Despawn(PlayerIndex.Two, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            PlayerSelectUIManager.Instance.RemovePlayer(PlayerIndex.Three);
            _playerSpawner.Despawn(PlayerIndex.Three, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            PlayerSelectUIManager.Instance.RemovePlayer(PlayerIndex.Four);
            _playerSpawner.Despawn(PlayerIndex.Four, true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (PlayerSelectUIManager.Instance.ReadyToStartMatch)
            {
                PlayerSelectUIManager.Instance.StartMatch();
            }

            PlayerInfo player = ActivePlayers.Players.FirstOrDefault(p => p.ControllerNumber == (int)PlayerIndex.One);
            if (player != null)
            {
                PlayerSelectUIManager.Instance.ReadyPlayer(PlayerIndex.One);
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (PlayerSelectUIManager.Instance.ReadyToStartMatch)
            {
                PlayerSelectUIManager.Instance.StartMatch();
            }

            PlayerInfo player = ActivePlayers.Players.FirstOrDefault(p => p.ControllerNumber == (int)PlayerIndex.Two);
            if (player != null)
            {
                PlayerSelectUIManager.Instance.ReadyPlayer(PlayerIndex.Two);
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (PlayerSelectUIManager.Instance.ReadyToStartMatch)
            {
                PlayerSelectUIManager.Instance.StartMatch();
            }

            PlayerInfo player = ActivePlayers.Players.FirstOrDefault(p => p.ControllerNumber == (int)PlayerIndex.Three);
            if (player != null)
            {
                PlayerSelectUIManager.Instance.ReadyPlayer(PlayerIndex.Three);
            }
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (PlayerSelectUIManager.Instance.ReadyToStartMatch)
            {
                PlayerSelectUIManager.Instance.StartMatch();
            }

            PlayerInfo player = ActivePlayers.Players.FirstOrDefault(p => p.ControllerNumber == (int)PlayerIndex.Four);
            if (player != null)
            {
                PlayerSelectUIManager.Instance.ReadyPlayer(PlayerIndex.Four);
            }
        }
    }

    public void InitializeController(int index)
    {
        _index = (PlayerIndex)index;
    }
}
