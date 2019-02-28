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

    [SerializeField]
    List<Vector3> _spawnLocations;

    private PlayerColorController _colorController;

    private PlayerInput[] _players
    {
        get { return GetComponentsInChildren<PlayerInput>(); }
    }

    private void Awake()
    {
        _colorController = GetComponent<PlayerColorController>();
        
        if (_spawnLocations.Count < _players.Length)
        {
            throw new System.Exception("There are not enough player spawn locations specified.");
        }
    }

    public void Spawn(PlayerIndex index)
    {
        if (!_players.FirstOrDefault(p => p.Index == index))
        {
            GameObject player = Instantiate(_playerPrefab);
            player.GetComponent<PlayerInput>().Index = index;
            player.transform.parent = gameObject.transform;
            player.transform.position = _spawnLocations[(int)index-1];
            player.GetComponent<Renderer>().material = _colorController.AssignStartingColor(index);
        }
    }

    public void Despawn(PlayerIndex index)
    {
        PlayerInput player = _players.FirstOrDefault(p => p.Index == index);

        if (player)
        {
            Transform playerTransform = player.transform;
            Destroy(player.gameObject);

            ParticleSystem particleEmitter = Instantiate(_despawnParticleEffect);
            particleEmitter.transform.parent = gameObject.transform;
            particleEmitter.transform.position = playerTransform.position;
            particleEmitter.GetComponent<Renderer>().material = _colorController.GetColor(index);
            _colorController.FreeColorAtIndex(index);
        }
    }
}
