using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerColorController))]
public class PlayerColorController : MonoBehaviour
{
    private Dictionary<PlayerIndex, Material> _playerMaterials;

    private void Awake()
    {
        _playerMaterials = new Dictionary<PlayerIndex, Material>();
        for (int i = 1; i <= 4; i++)
        {
            _playerMaterials.Add((PlayerIndex)i, null);
        }
    }

    public Material AssignStartingColor(PlayerIndex index)
    {
        Material material = Materials.Instance.availableMaterials.First(m => !_playerMaterials.ContainsValue(m));
        _playerMaterials[index] = material;

        return material;
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
