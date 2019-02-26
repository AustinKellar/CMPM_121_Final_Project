using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerPrefab;

    [SerializeField]
    private ParticleSystem _despawnParticleEffect;

    private PlayerColorController _colorController;

    private PlayerInput[] _players
    {
        get { return GetComponentsInChildren<PlayerInput>(); }
    }

    private void Awake()
    {
        _colorController = GetComponent<PlayerColorController>();
    }

    public void Spawn(PlayerIndex index)
    {
        if (!_players.FirstOrDefault(p => p.Index == index))
        {
            GameObject player = Instantiate(_playerPrefab);
            player.GetComponent<PlayerInput>().Index = index;
            player.transform.parent = gameObject.transform;
            player.GetComponent<Renderer>().material = _colorController.AssignStartingColor(index);
        }
    }

    public void Despawn(PlayerIndex index)
    {
        GameObject player = _players.FirstOrDefault(p => p.Index == index).gameObject;

        if (player)
        {
            Transform playerTransform = player.transform;
            Destroy(player);

            ParticleSystem particleEmitter = Instantiate(_despawnParticleEffect);
            particleEmitter.transform.parent = gameObject.transform;
            particleEmitter.transform.position = playerTransform.position;
            particleEmitter.GetComponent<Renderer>().material = _colorController.GetColor(index);
            _colorController.FreeColorAtIndex(index);
        }
    }
}
