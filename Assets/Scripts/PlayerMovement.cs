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
        Vector3 destination = transform.position + direction;

        Collider2D barrier = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Barrier"));
        Collider2D platform = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Platform"));
        Collider2D obstacle = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Obstacle"));

        if (barrier != null)
        {
            return;
        }
        if (platform != null)
        {
            transform.SetParent(platform.transform);
        } else
        {
            transform.SetParent(null);
        }
        if (obstacle != null && platform == null)
        {
            transform.position += direction;
            Death();
        } else
        {
            transform.position += direction;
        }
    }

    public void Death()
    {
        enabled = false;
        Debug.Log("Death");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") && transform.parent == null)
        {
            Death();
        }
    }
}
