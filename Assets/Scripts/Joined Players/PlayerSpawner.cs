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

    private PlayerSelectColorController _colorController;
    private List<PlayerIndex> _playersToRespawn = new List<PlayerIndex>();
    private List<PlayerInput> _players = new List<PlayerInput>();

    public int DeadPlayerCount { get { return _playersToRespawn.Count; } }

    private void Awake()
    {
        _colorController = GetComponent<PlayerSelectColorController>();
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
        ActivePlayers.Reset();
    }

    public void Reset()
    {
        List<GameObject> players = GetComponentsInChildren<PlayerInput>().Select(p => p.gameObject).ToList();
        players.ForEach(p => DestroyPlayer(p));
        ActivePlayers.Reset();
        _players.Clear();
        _playersToRespawn.Clear();
    }

    public void DestroyPlayer(GameObject player)
    {
        Destroy(player.gameObject);

        ParticleSystem particleEmitter = Instantiate(_despawnParticleEffect, player.transform.position, Quaternion.identity);
        particleEmitter.transform.parent = gameObject.transform;

        Material material = player.GetComponentInChildren<SkinnedMeshRenderer>().material;
        particleEmitter.GetComponent<Renderer>().material = material;
        _colorController.ReturnMaterialToPool(material);
    }

    public void Spawn(PlayerIndex index, bool addActivePlayer)
    {
        if (LobbyStateManager.Instance.State == LobbyStateManager.LobbyState.TitleScreen || !LobbyStateManager.Instance.CameraAtDestination)
        {
            return;
        }

        if (!_players.FirstOrDefault(p => p.Index == index))
        {
            Vector3 position = _spawnLocations[(int)index - 1];
            GameObject player = Instantiate(_playerPrefab, position, Quaternion.identity);
            player.GetComponent<PlayerInput>().Index = index;
            player.transform.parent = gameObject.transform;

            Material material = _colorController.AssignRandomPlayerMaterial(player);
            _players.Add(player.GetComponent<PlayerInput>());
            AudioManager.Instance.PlayOneShot("Player Jump");

            if (PlayerSelectUIManager.Instance.PlayerIsReady(index))
            {
                _colorController.ShowIcon(index);
            }
            else
            {
                _colorController.HideIcon(index);
            }

            if (addActivePlayer)
            {
                ActivePlayers.AddPlayer(new Player((int)index, material));
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

            ParticleSystem particleEmitter = Instantiate(_despawnParticleEffect, playerTransform.position, Quaternion.identity);
            particleEmitter.transform.parent = gameObject.transform;

            Material material = player.GetComponentInChildren<SkinnedMeshRenderer>().material;
            particleEmitter.GetComponent<Renderer>().material = material;
            int removedPlayers = _players.RemoveAll(p => p.Index == index);
            _colorController.ReturnMaterialToPool(material);

            if (removedPlayers > 0)
            {
                AudioManager.Instance.PlayOneShot("Character Death");
                if (removeActivePlayer)
                {
                    ActivePlayers.RemovePlayer(new Player((int)index, material));
                }
            }
        }
    }

    public void Respawn(PlayerIndex index)
    {
        Despawn(index, false);

        _playersToRespawn.Add(index);
        Invoke("SpawnSquishedPlayers", 1f);
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
