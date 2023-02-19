using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    [SerializeField] private Vector2 direction = Vector2.right;
    [SerializeField] private float speed = 1f;
    [SerializeField] private double size = 1f;

    private Vector3 leftEdge;
    private Vector3 rightEdge;

    private void Awake()
    {
        leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero); 
        rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
    }

    private void Update()
    {
        if (direction.x > 0 && (transform.position.x - size) > rightEdge.x)
        {
            Vector3 position = transform.position;
            position.x = leftEdge.x - (int) size;
            transform.position = position;
        } else if (direction.x < 0 && (transform.position.x + size) < leftEdge.x)
        {
            Vector3 position = transform.position;
            position.x = rightEdge.x + (int) size;
            transform.position = position;
        } else
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

}
