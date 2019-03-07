using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _blockPrefab;

    [SerializeField]
    private GameObject _paintBlockPrefab;

    [SerializeField]
    private ParticleSystem _despawnParticleEffect;

    [SerializeField]
    private List<Vector3> _paintBlockSpawnLocations;

    private PlayerColorController _playerColorController;
    private GameObject _startBlock;
    private List<Material> _materialsInUse = new List<Material>();
    private Stack<Vector3> _spawnStack;
    private List<GameObject> _despawningBlocks = new List<GameObject>();

    private void Awake()
    {
        _spawnStack = new Stack<Vector3>(_paintBlockSpawnLocations.OrderByDescending(v => v.x));
        _playerColorController = GetComponent<PlayerColorController>();
    }

    private void Start()
    {
        for(int i=0; i<_paintBlockSpawnLocations.Count; i++)
        {
            Invoke("SpawnPaintBlockInDefaultPosition", Convert.ToSingle(i) / 10f);
        }
    }

    public void SpawnStartBlock()
    {
        if (_startBlock != null)
        {
            return;
        }

        _startBlock = Instantiate(_blockPrefab);
        _startBlock.transform.parent = gameObject.transform;
        _startBlock.transform.position = new Vector3(0, 20, -2);
        _startBlock.GetComponent<Rigidbody>().velocity = new Vector3(0, -80, 0);
        AudioManager.Instance.PlayOneShot("Block Spawn Rock");
    }

    public void DespawnStartBlock()
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
        AudioManager.Instance.PlayOneShot("Block Break Rock");
    }

    public void DespawnPaintBlock(GameObject block, Material playerMaterial)
    {
        Debug.Log(playerMaterial.name);

        Material particleMaterial = block.GetComponent<Renderer>().material;
        Transform blockTransform = block.transform;
        Destroy(block);

        ParticleSystem particleEmitter = Instantiate(_despawnParticleEffect);
        particleEmitter.transform.parent = gameObject.transform;
        particleEmitter.transform.position = blockTransform.position;
        particleEmitter.GetComponent<Renderer>().material = particleMaterial;
        AudioManager.Instance.PlayOneShot("Block Break Rock");

        Vector3 position = new Vector3(blockTransform.position.x, 12, blockTransform.position.z);
        _spawnStack.Push(position);
        Invoke("SpawnPaintBlockInDefaultPosition", 2f);

        Debug.Log(playerMaterial.name);
        _materialsInUse.RemoveAll(m => m.color == playerMaterial.color);
    }

    private Material GetRandomAvailableMaterial()
    {
        List<Material> availableMaterials = Materials.Instance.availableMaterials.Where(m => !_materialsInUse.Contains(m)).ToList();
        Material material = availableMaterials[UnityEngine.Random.Range(0, availableMaterials.Count)];
        _materialsInUse.Add(material);

        return material;
    }

    private void SpawnPaintBlockInDefaultPosition()
    {
        SpawnPaintBlock(_spawnStack.Pop(), GetRandomAvailableMaterial());
    }

    private void SpawnPaintBlock(Vector3 position, Material material)
    {
        GameObject block = Instantiate(_paintBlockPrefab);
        block.transform.parent = gameObject.transform;
        block.transform.position = position;
        block.GetComponent<Renderer>().material = material;
        block.GetComponentInChildren<TextMeshPro>().text = material.name;
    }
}
