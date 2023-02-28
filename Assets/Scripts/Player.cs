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

    /// <summary>
    /// Reverse controls if reverseMove is true, otherwise move player regularly.
    /// </summary>
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

    /// <summary>
    /// Moves the player based on the input given.
    /// </summary>
    /// <param name="forward"> Key Assigned To Move Forward </param>
    /// <param name="left"> Key Assigned To Move Left </param>
    /// <param name="backward"> Key Assigned To Move Backward </param>
    /// <param name="right"> Key Assigned To Move Right </param>
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

    /// <summary>
    /// Calculates the destination of the player based on current position and direction.
    /// Handles on-move collisions based on destination.
    /// </summary>
    /// <param name="direction"> Move Direction </param>
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

    /// <summary>
    /// Setter for private variable.
    /// </summary>
    /// <param name="reverseMove"> Reverses Controls </param>
    public void SetReverseMove(bool reverseMove)
    {
        this.reverseMove = reverseMove;
    }

    /// <summary>
    /// Getter for private variable.
    /// </summary>
    /// <returns> Reverse Move </returns>
    public bool GetReverseMove()
    {
        return reverseMove;
    }

    /// <summary>
    /// Setter for private variable.
    /// </summary>
    /// <param name="obstacleImmunity"> Immune To Car Damage </param>
    public void SetObstacleImmunity(bool obstacleImmunity)
    {
        this.obstacleImmunity = obstacleImmunity;
    }

    /// <summary>
    /// Getter for private variable.
    /// </summary>
    /// <returns> Obstacle Immunity </returns>
    public bool GetObstacleImmunity()
    {
        return obstacleImmunity;
    }

    /// <summary>
    /// Setter for private variable.
    /// </summary>
    /// <param name="blockDistance"> Amount Of Spaces Moved. </param>
    public void SetBlockDistance(int blockDistance)
    {
        this.blockDistance = blockDistance;
    }

    /// <summary>
    /// Disables player and shows the game over ui.
    /// </summary>
    public void Death()
    {
        enabled = false;
        FindObjectOfType<GameManager>().GameOver();
    }

    /// <summary>
    /// Enables player and resets relevant ui, effects and map.
    /// </summary>
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
  
