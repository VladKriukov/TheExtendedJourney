using UnityEngine;

public class FollowPlayerChunk : MonoBehaviour
{
    [SerializeField] Transform playerTrain;
    [SerializeField] float zOffset;

    private void Update()
    {
        transform.position = new Vector3(0, -10, playerTrain.position.z - zOffset);
    }
}