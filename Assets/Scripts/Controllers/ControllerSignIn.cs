using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControllerSignIn : MonoBehaviour
{
    private PlayerIndex _index;
    private PlayerSpawner _playerSpawner;

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
            _playerSpawner.Despawn(_index);
        }
    }

    public void InitializeController(int index)
    {
        _index = (PlayerIndex)index;
    }
}
