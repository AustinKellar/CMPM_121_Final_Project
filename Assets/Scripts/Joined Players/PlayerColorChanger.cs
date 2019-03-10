using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerColorChanger : MonoBehaviour
{
    private PlayerSelectBlockSpawner _blockSpawner;

    private void Awake()
    {
        _blockSpawner = GetComponent<PlayerSelectBlockSpawner>();
    }

    public Material AssignRandomMaterial(PlayerIndex index, Vector3 playerSpawnPosition)
    {
        Material material = _blockSpawner.GetRandomAvailableMaterial();
        return material;
    }

    public void SetColor(GameObject player, Material material)
    {
        PlayerInput input = player.GetComponent<PlayerInput>();
        player.GetComponentInChildren<SkinnedMeshRenderer>().material = material;
        ActivePlayers.UpdateMaterial((int)player.GetComponent<PlayerInput>().Index, material);
        PlayerSelectUIManager.Instance.UpdateColor(input.Index, material);
    }
}
