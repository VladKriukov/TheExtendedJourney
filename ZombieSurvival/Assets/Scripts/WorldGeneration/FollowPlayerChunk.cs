using UnityEngine;

public class FollowPlayerChunk : MonoBehaviour
{
    [SerializeField] Transform playerfollow;
    [SerializeField] float zOffset;

    private void OnDisable()
    {
        ChunkGenerator.OnMovingFromBehind -= EnableFollow;
    }

    private void OnEnable()
    {
        ChunkGenerator.OnMovingFromBehind += EnableFollow;
    }

    void EnableFollow()
    {
        if (name == "ChunkTriggerBackward")
        {
            GetComponent<BoxCollider>().enabled = true;
            zOffset = -200 + 10 * (Game.numberOfChunksToSpawn - 4);
        }
    }

    private void Update()
    {
        transform.position = new Vector3(0, -25, playerfollow.position.z - zOffset);
    }
}