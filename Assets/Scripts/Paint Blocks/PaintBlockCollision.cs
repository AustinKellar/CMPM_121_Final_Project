using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBlockCollision : MonoBehaviour
{
    private bool _alreadyCollided = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject player = collision.gameObject;
            HandleCollision(player);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject player = collision.gameObject;
            HandleCollision(player);
        }
    }


    private void HandleCollision(GameObject player)
    {
        if (!_alreadyCollided && ((!player.GetComponent<PlayerMovement>().HasMoved) && !player.GetComponent<PlayerGrounded>().TouchingFloor) || player.GetComponent<PlayerDash>().IsDashing)
        {
            _alreadyCollided = true;
            Material playerMaterial = player.GetComponentInChildren<SkinnedMeshRenderer>().material;
            gameObject.GetComponentInParent<PlayerSelectBlockSpawner>().DespawnPaintBlock(gameObject, playerMaterial);
            player.GetComponentInParent<PlayerSelectColorController>().SetPlayerColor(player, gameObject.GetComponent<Renderer>().material);
        }
    }
}
