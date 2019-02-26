using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    ControllerSignIn[] _controllers;

    private void Awake()
    {
        _controllers = GetComponentsInChildren<ControllerSignIn>();
        
        for(int i=0; i<_controllers.Length; i++)
        {
            _controllers[i].InitializeController(i+1);
        }
    }
}
