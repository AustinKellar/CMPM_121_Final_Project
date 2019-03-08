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
    private BlockSpawner _blockSpawner;
    private GameObject _startBlock = null;
    private List<PlayerIndex> _playersToRespawn = new List<PlayerIndex>();

    private List<PlayerInput> _players = new List<PlayerInput>();

    private void Awake()
    {
        _colorController = GetComponent<PlayerColorController>();
        _blockSpawner = GetComponent<BlockSpawner>();
        
        if (_spawnLocations.Count < _players.Count)
        {
            throw new System.Exception("There are not enough player spawn locations specified.");
        }
    }

    private void Start()
    {
        if (!AudioManager.Instance.IsPlaying("Menu Theme"))
        {
            AudioManager.Instance.FadeInSound("Menu Theme", 1f);
        }
    }

    public void Spawn(PlayerIndex index, bool addActivePlayer)
    {
        if (!_players.FirstOrDefault(p => p.Index == index))
        {
            GameObject player = Instantiate(_playerPrefab);
            player.GetComponent<PlayerInput>().Index = index;
            player.transform.parent = gameObject.transform;
            player.transform.position = _spawnLocations[(int)index-1];
            player.GetComponentInChildren<SkinnedMeshRenderer>().material = _colorController.AssignDefaultMaterial(index);
            _players.Add(player.GetComponent<PlayerInput>());
            AudioManager.Instance.PlayOneShot("Character Spawn");

            if (addActivePlayer)
            {
                ActivePlayers.AddPlayer(new Player((int)index, player.GetComponentInChildren<SkinnedMeshRenderer>().material));
            }

            if (_players.Count >= 2)
            {
                Invoke("SpawnStartBlock", 0.5f);
            }
        }
    }

    public void Despawn(PlayerIndex index, bool removeActivePlayer)
    {
        PlayerInput player = _players.FirstOrDefault(p => p.Index == index);

        if (player)
        {
            Transform playerTransform = player.transform;
            Destroy(player.gameObject);

            ParticleSystem particleEmitter = Instantiate(_despawnParticleEffect);
            particleEmitter.transform.parent = gameObject.transform;
            particleEmitter.transform.position = playerTransform.position;
            particleEmitter.GetComponent<Renderer>().material = player.GetComponentInChildren<SkinnedMeshRenderer>().material;
            int removedPlayers = _players.RemoveAll(p => p.Index == index);

            if (removedPlayers > 0)
            {
                AudioManager.Instance.PlayOneShot("Character Death");

                if (removeActivePlayer)
                {
                    ActivePlayers.RemovePlayer(new Player((int)index, player.GetComponentInChildren<SkinnedMeshRenderer>().material));
                }
            }

            if (_players.Count <= 1)
            {
                Invoke("DespawnStartBlock", 0.5f);
            }
        }
    }

    public void Respawn(PlayerIndex index)
    {
        Despawn(index, false);

        _playersToRespawn.Add(index);
        Invoke("SpawnSquishedPlayers", 1f);
    }

    private void SpawnStartBlock()
    {
        _blockSpawner.SpawnStartBlock();
    }

    private void DespawnStartBlock()
    {
        _blockSpawner.DespawnStartBlock();
    }

    private void SpawnSquishedPlayers()
    {
        foreach (PlayerIndex index in _playersToRespawn)
        {
            Spawn(index, false);
        }
        _playersToRespawn.Clear();
    }
}

public class Player
{
    public int PlayerNumber;
    public Material Material;

    public Player(int playerNumber, Material material)
    {
        PlayerNumber = playerNumber;
        Material = material;
    }
}
