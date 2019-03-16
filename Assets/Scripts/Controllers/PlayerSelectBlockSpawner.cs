using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerSelectBlockSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _paintBlockPrefab;

    [SerializeField]
    private ParticleSystem _despawnParticleEffect;

    [SerializeField]
    private List<Vector3> _paintBlockSpawnLocations;

    [SerializeField]
    private float _paintBlockRespawnTimer;

    [SerializeField]
    private float _blockGravityModifier;

    private PlayerSpawner _playerSpawner;
    private PlayerSelectColorController _colorController;
    private List<Vector3> _blocksToRespawn;
    private int _spawnCount;
    private float _paintBlockLastDestroyedTime;
    private bool _spawnedStartingBlocks = false;

    private void Awake()
    {
        _blocksToRespawn = new List<Vector3>(_paintBlockSpawnLocations.OrderBy(v => v.x));
        _playerSpawner = GetComponent<PlayerSpawner>();
        _colorController = GetComponent<PlayerSelectColorController>();
    }

    private void Start()
    {
        StartCoroutine(SpawnStartingBlocks());
        StartCoroutine(RespawnPaintBlocks());
    }

    public void Reset()
    {
        List<GameObject> blocks = GetComponentsInChildren<PaintBlockCollision>().Select(o => o.gameObject).ToList();
        blocks.ForEach(b => DestroyPaintBlock(b));
        _spawnedStartingBlocks = false;
        _blocksToRespawn = new List<Vector3>(_paintBlockSpawnLocations.OrderBy(v => v.x));
        _spawnCount = 0;
    }

    public void DestroyPaintBlock(GameObject block)
    {
        Material particleMaterial = block.GetComponent<Renderer>().material;
        Transform blockTransform = block.transform;

        Destroy(block);

        ParticleSystem particleEmitter = Instantiate(_despawnParticleEffect, blockTransform.position, Quaternion.identity);
        particleEmitter.transform.parent = gameObject.transform;
        particleEmitter.GetComponent<Renderer>().material = particleMaterial;
        AudioManager.Instance.PlayOneShot("Block Break Rock");
        _colorController.ReturnMaterialToPool(particleMaterial);
    }

    public void DespawnPaintBlock(GameObject block, Material playerMaterial)
    {
        Material particleMaterial = block.GetComponent<Renderer>().material;
        Transform blockTransform = block.transform;

        Vector3 newPaintBlockPosition = new Vector3(blockTransform.position.x, _paintBlockSpawnLocations.First().y, blockTransform.position.z);

        if (_blocksToRespawn.Contains(newPaintBlockPosition))
        {
            return;
        }

        Destroy(block);

        ParticleSystem particleEmitter = Instantiate(_despawnParticleEffect, blockTransform.position, Quaternion.identity);
        particleEmitter.transform.parent = gameObject.transform;
        particleEmitter.GetComponent<Renderer>().material = particleMaterial;
        AudioManager.Instance.PlayOneShot("Block Break Rock");

        _blocksToRespawn.Add(newPaintBlockPosition);
        _colorController.ReturnMaterialToPool(playerMaterial);
        _paintBlockLastDestroyedTime = Time.time;
    }

    private void SpawnPaintBlockFromStack()
    {
        if (_blocksToRespawn.Count < 1)
        {
            return;
        }

        Vector3 position = _blocksToRespawn[0];
        _blocksToRespawn.RemoveAt(0);

        SpawnPaintBlock(position, _colorController.GetRandomAvailableMaterial());
    }

    private IEnumerator SpawnStartingBlocks()
    {
        while(true)
        {
            if (!_spawnedStartingBlocks)
            {
                if (LobbyStateManager.Instance.State == LobbyStateManager.LobbyState.ColorSelect)
                {
                    for (int i = 0; i < _paintBlockSpawnLocations.Count; i++)
                    {
                        Invoke("SpawnPaintBlockFromStack", Convert.ToSingle(i) / 10f);
                    }
                    _spawnedStartingBlocks = true;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator RespawnPaintBlocks()
    {
        while (true)
        {
            if (_blocksToRespawn.Count > 0 && 
                LobbyStateManager.Instance.State == LobbyStateManager.LobbyState.ColorSelect &&
                !PlayerSelectUIManager.Instance.ReadyToStartMatch &&
                _colorController.TrackedMaterialCount < Materials.Instance.availableMaterials.Length &&
                _spawnCount >= 4 &&
                _playerSpawner.DeadPlayerCount == 0 &&
                Time.time > _paintBlockLastDestroyedTime + (_paintBlockRespawnTimer * 0.5f))
            {
                SpawnPaintBlockFromStack();
            }
            yield return new WaitForSeconds(_paintBlockRespawnTimer);
        }
    }

    private void SpawnPaintBlock(Vector3 position, Material material)
    {
        GameObject block = Instantiate(_paintBlockPrefab);
        block.transform.parent = gameObject.transform;
        block.transform.position = position;
        block.GetComponent<Renderer>().material = material;
        block.GetComponent<Rigidbody>().velocity = new Vector3(0, _blockGravityModifier, 0);
        _spawnCount++;
    }
}
