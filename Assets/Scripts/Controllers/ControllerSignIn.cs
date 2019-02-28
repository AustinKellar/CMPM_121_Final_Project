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
    }

    public void InitializeController(int index)
    {
        _index = (PlayerIndex)index;
    }
}
