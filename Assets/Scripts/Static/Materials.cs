using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Materials : MonoBehaviour
{
    public Material[] availableMaterials;

    public static Materials Instance;

    private void Awake()
    {
        KeepOnlyOneInstance();
    }

    private void KeepOnlyOneInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
