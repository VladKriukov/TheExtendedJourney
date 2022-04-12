using UnityEngine;

public class MMCamera : MonoBehaviour
{
    [SerializeField] Transform player;

    private void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y + 5, player.position.z);
    }
}