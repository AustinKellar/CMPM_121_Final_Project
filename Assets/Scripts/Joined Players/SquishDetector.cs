using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquishDetector : MonoBehaviour
{
    private PlayerSpawner _playerSpawner;
    private PlayerIndex _index;

    private void Awake()
    {
        _playerSpawner = FindObjectOfType<PlayerSpawner>();
    }

    private void Start()
    {
        _index = GetComponentInParent<PlayerInput>().Index;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Block")
        {
            if (Mathf.Abs(other.gameObject.GetComponent<Rigidbody>().velocity.y) > 0.1f)
            {
                _playerSpawner.Respawn(_index);
            }
        }
    }
}
