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

    private PlayerColorChanger _playerColorController;
    private List<Material> _materialsInUse = new List<Material>();
    private List<Vector3> _blocksToRespawn;
    private List<GameObject> _despawningBlocks = new List<GameObject>();
    private List<Vector3> _spawnedPaintBlocks = new List<Vector3>();
    private int _spawnCount;

    private void Awake()
    {
        _blocksToRespawn = new List<Vector3>(_paintBlockSpawnLocations.OrderBy(v => v.x));
        _playerColorController = GetComponent<PlayerColorChanger>();
    }

    private void Start()
    {
        for (int i = 0; i < _paintBlockSpawnLocations.Count; i++)
        {
            Invoke("SpawnPaintBlockFromStack", Convert.ToSingle(i) / 10f);
        }

        StartCoroutine(RespawnPaintBlocks());
    }

    public bool MaterialInUse(Material material)
    {
        return _materialsInUse.FirstOrDefault(m => m.color == material.color) != null;
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

        _materialsInUse.RemoveAll(m => m.color == playerMaterial.color);
        _spawnedPaintBlocks.RemoveAll(m => m == newPaintBlockPosition);
    }

    public Material GetRandomAvailableMaterial()
    {
        List<Material> availableMaterials = Materials.Instance.availableMaterials.Where(m => !_materialsInUse.Contains(m)).ToList();
        Material material = availableMaterials[UnityEngine.Random.Range(0, availableMaterials.Count)];
        _materialsInUse.Add(material);

        return material;
    }

    public void ReturnMaterialToPool(Material material)
    {
        _materialsInUse.RemoveAll(m => m.color == material.color);
    }

    private void SpawnPaintBlockFromStack()
    {
        if (_blocksToRespawn.Count < 1)
        {
            return;
        }

        Vector3 position = _blocksToRespawn[0];
        _blocksToRespawn.RemoveAt(0);

        SpawnPaintBlock(position, GetRandomAvailableMaterial());
    }

    private IEnumerator RespawnPaintBlocks()
    {
        while (true)
        {
            if (_blocksToRespawn.Count > 0 &&
                !PlayerSelectUIManager.Instance.ReadyToStartMatch &&
                _materialsInUse.Count < Materials.Instance.availableMaterials.Length &&
                _spawnCount >= 4)
            {
                SpawnPaintBlockFromStack();
            }
            yield return new WaitForSeconds(_paintBlockRespawnTimer);
        }
    }

    private void SpawnPaintBlock(Vector3 position, Material material)
    {
        _spawnedPaintBlocks.Add(position);
        GameObject block = Instantiate(_paintBlockPrefab);
        block.transform.parent = gameObject.transform;
        block.transform.position = position;
        block.GetComponent<Renderer>().material = material;
        block.GetComponentInChildren<TextMeshPro>().text = material.name;
        block.GetComponent<Rigidbody>().velocity = new Vector3(0, _blockGravityModifier, 0);
        _spawnCount++;
    }
}
