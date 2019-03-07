using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBlockCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject player = collision.gameObject;
            if (!player.GetComponent<PlayerMovement>().HasMoved || player.GetComponent<PlayerDash>().IsDashing)
            {
                Material playerMaterial = player.GetComponentInChildren<SkinnedMeshRenderer>().material;
                player.GetComponentInParent<PlayerColorController>().SetColor(player, gameObject.GetComponent<Renderer>().material);
                gameObject.GetComponentInParent<BlockSpawner>().DespawnPaintBlock(gameObject, playerMaterial);
            }
        }
    }
}
