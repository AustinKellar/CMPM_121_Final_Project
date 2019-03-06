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
    private float _pressBTime;

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
            _playerSpawner.Spawn(_index);
        }
        if (state.Pressed(CButton.B))
        {
            _pressBTime = Time.time;
        }
        if (state.B && Time.time > _timeToHoldB + _pressBTime)
        {
            _playerSpawner.Despawn(_index);
        }

        CaptureKeyboardInput();
    }

    private void CaptureKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _playerSpawner.Spawn(PlayerIndex.One);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _playerSpawner.Spawn(PlayerIndex.Two);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _playerSpawner.Spawn(PlayerIndex.Three);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _playerSpawner.Spawn(PlayerIndex.Four);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _playerSpawner.Despawn(PlayerIndex.One);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            _playerSpawner.Despawn(PlayerIndex.Two);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            _playerSpawner.Despawn(PlayerIndex.Three);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            _playerSpawner.Despawn(PlayerIndex.Four);
        }
    }

    public void InitializeController(int index)
    {
        _index = (PlayerIndex)index;
    }
}
