using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerColorController))]
public class PlayerColorController : MonoBehaviour
{
    private Dictionary<PlayerIndex, Material> _defaultMaterials;
    private Dictionary<PlayerIndex, Material> _playerMaterials;

    private void Start()
    {
        _defaultMaterials = new Dictionary<PlayerIndex, Material>();
        _playerMaterials = new Dictionary<PlayerIndex, Material>();

        for (int i = 1; i <= 4; i++)
        {
            _playerMaterials.Add((PlayerIndex)i, null);
            _defaultMaterials.Add((PlayerIndex)i, Materials.Instance.availableMaterials[i-1]);
        }
    }

    public Material AssignStartingColor(PlayerIndex index)
    {
        Material defaultMaterial = _defaultMaterials[index];
        if (_playerMaterials.Where(m => m.Value == defaultMaterial).ToList().Count == 0)
        {
            _playerMaterials[index] = defaultMaterial;
            return defaultMaterial;
        }
        else
        {
            Material material = Materials.Instance.availableMaterials.First(m => !_playerMaterials.ContainsValue(m));
            _playerMaterials[index] = material;

            return material;
        }
    }

    public Material GetColor(PlayerIndex index)
    {
        return _playerMaterials[index];
    }

    public void FreeColorAtIndex(PlayerIndex index)
    {
        _playerMaterials[index] = null;
    }
}
