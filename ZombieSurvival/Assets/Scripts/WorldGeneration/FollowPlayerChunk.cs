using UnityEngine;

public class FollowPlayerChunk : MonoBehaviour
{
    [SerializeField] Transform playerfollow;
    [SerializeField] Transform behindChunkTrigger;
    [SerializeField] Transform frontChunkTrigger;

    private void Awake()
    {
        behindChunkTrigger.position = new Vector3(0, 0, transform.position.z - Game.numberOfChunksToSpawn / 2 * 30 - 15);
        frontChunkTrigger.position = new Vector3(0, 0, transform.position.z + Game.numberOfChunksToSpawn / 2 * 30 + 15);
    }

    private void OnDisable()
    {
        ChunkGenerator.OnMovingFromBehind -= EnableFrontChunkTrigger;
    }

    private void OnEnable()
    {
        ChunkGenerator.OnMovingFromBehind += EnableFrontChunkTrigger;
    }

    void EnableFrontChunkTrigger()
    {
        frontChunkTrigger.GetComponent<BoxCollider>().enabled = true;
    }

    private void Update()
    {
        transform.position = new Vector3(0, 0, playerfollow.position.z);
    }
}