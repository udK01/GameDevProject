using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) {
            Move(Vector3.up);
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            Move(Vector3.left);
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            Move(Vector3.right);
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            Move(Vector3.down);
        }
    }

    private void Move(Vector3 direction)
    {
        transform.position += direction;
    }
}
