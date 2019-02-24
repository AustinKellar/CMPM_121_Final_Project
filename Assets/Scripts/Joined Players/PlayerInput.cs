using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float HorizontalMovement { get; private set; }
    public bool Jump { get; private set; }

    private void Update()
    {
        HorizontalMovement = Input.GetAxis("Horizontal");
        Jump = Input.GetKeyDown(KeyCode.Space);
    }
}
