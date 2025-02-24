using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    public float speed = 5f;

    private void Update()
    {
        if (!isLocalPlayer) return;

        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        transform.Translate(moveX, 0, moveZ);
    }
}
