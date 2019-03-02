using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerPrefab;

    [SerializeField]
    private GameObject _blockPrefab;

    [SerializeField]
    private ParticleSystem _despawnParticleEffect;

    [SerializeField]
    List<Vector3> _spawnLocations;

    private PlayerColorController _colorController;
    private GameObject _startBlock = null;
    private List<PlayerIndex> _playersToRespawn = new List<PlayerIndex>();

    private List<PlayerInput> _players = new List<PlayerInput>();

    private void Awake()
    {
        _colorController = GetComponent<PlayerColorController>();
        
        if (_spawnLocations.Count < _players.Count)
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
            player.GetComponentInChildren<SkinnedMeshRenderer>().material = _colorController.AssignStartingColor(index);
            _players.Add(player.GetComponent<PlayerInput>());

            if (_players.Count >= 2)
            {
                Invoke("SpawnStartBlock", 0.5f);
            }
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
            _players.RemoveAll(p => p.Index == index);

            if (_players.Count <= 1)
            {
                Invoke("DespawnStartBlock", 0.5f);
            }
        }
    }

    public void Respawn(PlayerIndex index)
    {
        Despawn(index);

        _playersToRespawn.Add(index);
        Invoke("SpawnSquishedPlayers", 1f);
    }

    private void SpawnSquishedPlayers()
    {
        foreach (PlayerIndex index in _playersToRespawn)
        {
            Spawn(index);
        }
        _playersToRespawn.Clear();
    }

    private void SpawnStartBlock()
    { 
        if (_startBlock != null)
        {
            return;
        }

        _startBlock = Instantiate(_blockPrefab);
        _startBlock.transform.parent = gameObject.transform;
        _startBlock.transform.position = new Vector3(0, 20, -2);
        _startBlock.GetComponent<Rigidbody>().velocity = new Vector3(0, -80, 0);
    }

    private void DespawnStartBlock()
    {
        if (_startBlock == null)
        {
            return;
        }

        Transform startBlockTransform = _startBlock.transform;
        Material particleMaterial = _startBlock.GetComponent<Renderer>().material;
        Destroy(_startBlock);

        ParticleSystem particleEmitter = Instantiate(_despawnParticleEffect);
        particleEmitter.transform.parent = gameObject.transform;
        particleEmitter.transform.position = startBlockTransform.position;
        particleEmitter.GetComponent<Renderer>().material = particleMaterial;
        _startBlock = null;
    }
}
