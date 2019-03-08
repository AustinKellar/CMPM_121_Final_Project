using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBlockCollision : MonoBehaviour
{
    public string sceneToStart;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject player = collision.gameObject;
            if (player.GetComponent<PlayerDash>().IsDashing)
            {
                GetComponentInParent<BlockSpawner>().DespawnStartBlock();
                SceneManager.LoadScene(sceneToStart);
            }
        }
    }
}
