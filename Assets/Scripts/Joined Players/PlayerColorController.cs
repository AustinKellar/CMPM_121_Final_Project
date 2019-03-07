using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerColorController))]
public class PlayerColorController : MonoBehaviour
{
    [SerializeField]
    private Material _defaultMaterial;

    private Dictionary<PlayerIndex, Material> _playerMaterials;

    public Material DefaultMaterial { get { return _defaultMaterial; } }

    private void Start()
    {
        _playerMaterials = new Dictionary<PlayerIndex, Material>();

        for (int i = 1; i <= 4; i++)
        {
            _playerMaterials.Add((PlayerIndex)i, null);
        }
    }

    public Material AssignDefaultMaterial(PlayerIndex index)
    {
        if (_playerMaterials[index] != null)
        {
            return _playerMaterials[index];
        }

        return _defaultMaterial;
    }

    public void SetColor(GameObject player, Material material)
    {
        PlayerInput input = player.GetComponent<PlayerInput>();
        player.GetComponentInChildren<SkinnedMeshRenderer>().material = material;
        _playerMaterials[input.Index] = material;
    }

    public void FreeColorAtIndex(PlayerIndex index)
    {
        _playerMaterials[index] = null;
    }
}
