using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectColorController : MonoBehaviour
{
    private PlayerSelectBlockSpawner _blockSpawner;
    private List<Material> _materialsInUse = new List<Material>();
    Dictionary<PlayerIndex, Material> _playerMaterials = new Dictionary<PlayerIndex, Material>
    {
        { PlayerIndex.One, null },
        { PlayerIndex.Two, null },
        { PlayerIndex.Three, null },
        { PlayerIndex.Four, null }
    };

    private Tweener _iconTweener;

    public int TrackedMaterialCount { get { return _materialsInUse.Count; } }

    private void Awake()
    {
        _blockSpawner = GetComponent<PlayerSelectBlockSpawner>();
    }

    public bool MaterialInUse(Material material)
    {
        return _materialsInUse.FirstOrDefault(m => m.color == material.color) != null;
    }

    public void ReturnMaterialToPool(Material material)
    {
        Material materialToRemove = _materialsInUse.FirstOrDefault(m => m.color == material.color);
        if (materialToRemove)
        {
            _materialsInUse.Remove(materialToRemove);
        }
    }

    public Material AssignRandomPlayerMaterial(GameObject player)
    {
        PlayerIndex index = player.GetComponent<PlayerInput>().Index;
        Material material = GetPreferredOrRandomMaterial(_playerMaterials[index]);
        SetPlayerColor(player, material);
        return material;
    }

    public void SetPlayerColor(GameObject player, Material material)
    {
        PlayerInput input = player.GetComponent<PlayerInput>();
        player.GetComponentInChildren<SkinnedMeshRenderer>().material = material;
        ActivePlayers.UpdateMaterial((int)player.GetComponent<PlayerInput>().Index, material);
        PlayerSelectUIManager.Instance.UpdateColor(input.Index, material);
        _playerMaterials[input.Index] = material;
    }

    public Material GetPreferredOrRandomMaterial(Material preferredMaterial)
    {
        if (preferredMaterial == null)
        {
            return GetRandomAvailableMaterial();
        }

        Material material = _materialsInUse.FirstOrDefault(m => m.color == preferredMaterial.color);
        if (material == null)
        {
            _materialsInUse.Add(preferredMaterial);
            return preferredMaterial;
        }
        else
        {
            return GetRandomAvailableMaterial();
        }
    }

    public Material GetRandomAvailableMaterial()
    {
        List<Material> availableMaterials = Materials.Instance.availableMaterials.Where(m => !MaterialInUse(m)).ToList();
        Material material = availableMaterials[UnityEngine.Random.Range(0, availableMaterials.Count)];
        _materialsInUse.Add(material);

        return material;
    }

    public void ShowIcon(PlayerIndex index)
    {
        PlayerInput player = GetComponentsInChildren<PlayerInput>().FirstOrDefault(p => p.Index == index);
        if (player != null)
        {
            Image icon = player.gameObject.GetComponentInChildren<Image>(true);
            icon.gameObject.SetActive(true);
            _iconTweener.Complete();
            _iconTweener = icon.transform.DOPunchScale(icon.transform.localScale * 1.05f, 0.3f, 9);
        }
    }

    public void HideIcon(PlayerIndex index)
    {
        PlayerInput player = GetComponentsInChildren<PlayerInput>().FirstOrDefault(p => p.Index == index);
        if (player)
        {
            player.gameObject.GetComponentInChildren<Image>(true).gameObject.SetActive(false);
        }
    }
}
