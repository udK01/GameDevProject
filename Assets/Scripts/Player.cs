using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 initialPos;
    [SerializeField] private int blockDistance = 1;
    [SerializeField] private bool obstacleImmunity;

    private bool reverseMove = false;
    private KeyCode forward = KeyCode.W;
    private KeyCode left = KeyCode.A;
    private KeyCode right = KeyCode.D;
    private KeyCode backward = KeyCode.S;

    private void Awake()
    {
        initialPos = transform.position;
    }

    private void Update()
    {
        if (!reverseMove)
        {
            MovePlayer(forward, left, backward, right);
        } else
        {
            MovePlayer(backward, right, forward, left);
        }
    }

    private void MovePlayer(KeyCode forward, KeyCode left, KeyCode backward, KeyCode right)
    {
        if (Input.GetKeyDown(forward))
        {
            Move(Vector3.up * blockDistance);
        }
        if (Input.GetKeyDown(left))
        {
            Move(Vector3.left * blockDistance);
        }
        if (Input.GetKeyDown(right))
        {
            Move(Vector3.right * blockDistance);
        }
        if (Input.GetKeyDown(backward))
        {
            Move(Vector3.down * blockDistance);
        }
    }

    private void Move(Vector3 direction)
    {
        Vector3 destination = transform.position + direction;

        Collider2D barrier = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Barrier"));
        Collider2D platform = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Platform"));
        Collider2D obstacle = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Obstacle"));

        FindObjectOfType<ProceduralGeneration>().GenerateLoot(transform.position, "Star");
        FindObjectOfType<ProceduralGeneration>().GenerateLoot(transform.position, "PowerUp");

        if (barrier != null)
        {
            return;
        }
        if (platform != null)
        {
            transform.SetParent(platform.transform);
        }
        else
        {
            transform.SetParent(null);
        }
        transform.position += direction;
    }

    public void SetReverseMove(bool reverseMove)
    {
        this.reverseMove = reverseMove;
    }

    public bool GetReverseMove()
    {
        return reverseMove;
    }

    public void SetObstacleImmunity(bool obstacleImmunity)
    {
        this.obstacleImmunity = obstacleImmunity;
    }

    public bool GetObstacleImmunity()
    {
        return obstacleImmunity;
    }

    public void ChangeBlockDistance(int blockDistance)
    {
        this.blockDistance = blockDistance;
    }

    public void Death()
    {
        enabled = false;
        FindObjectOfType<GameManager>().GameOver();
    }

    public void Respawn()
    {
        transform.position = initialPos;
        gameObject.SetActive(true);
        enabled = true;
        SetReverseMove(false);
        FindObjectOfType<ProceduralGeneration>().ResetMap();
        FindObjectOfType<GameManager>().CleansePowerUps();
    }
}
  
