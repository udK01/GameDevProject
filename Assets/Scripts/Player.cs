using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public bool reverseMove { get; set; } = false;
    public bool obstacleImmunity { get; set; } = false;
    public int blockDistance { get; set; } = 1;

    private Vector3 initialPos;
    private KeyCode forward = KeyCode.W;
    private KeyCode backward = KeyCode.S;
    private KeyCode left = KeyCode.A;
    private KeyCode right = KeyCode.D;

    private void Awake()
    {
        initialPos = transform.position;
        Instance = this;
    }

    private void Start()
    {
        InitialiseKeys();
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
            ProceduralGeneration.Instance.GenerateBarrier((int)transform.position.y - 9);
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

        ProceduralGeneration.Instance.GenerateLoot(transform.position, "Star");
        ProceduralGeneration.Instance.GenerateLoot(transform.position, "PowerUp");

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
    /// Disables player and shows the game over ui.
    /// </summary>
    public void Death()
    {
        enabled = false;
        GameManager.Instance.DisableNotificationText();
        GameManager.Instance.GameOver();
    }

    /// <summary>
    /// Enables player and resets relevant ui, effects and map.
    /// </summary>
    public void Respawn()
    {
        transform.position = initialPos;
        gameObject.SetActive(true);
        enabled = true;
        reverseMove = false;
        ProceduralGeneration.Instance.ResetMap();
        GameManager.Instance.CleansePowerUps();
    }
    // 1 to 4 forward, back, left, right
    public void SetKey(int direction, KeyCode key)
    {
        switch(direction)
        {
            case 1:
                forward = key;
                PlayerPrefs.SetString("forward", key.ToString());
                break;
            case 2:
                backward = key;
                PlayerPrefs.SetString("backward", key.ToString());
                break;
            case 3:
                left = key;
                PlayerPrefs.SetString("left", key.ToString());
                break;
            case 4:
                right = key;
                PlayerPrefs.SetString("right", key.ToString());
                break;
        }
    }

    private void InitialiseKeys()
    {
        if (PlayerPrefs.HasKey("forward"))
        {
            forward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("forward"));
        }
        if (PlayerPrefs.HasKey("backward"))
        {
            backward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("backward"));
        }
        if (PlayerPrefs.HasKey("left"))
        {
            left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("left"));
        }
        if (PlayerPrefs.HasKey("right"))
        {
            right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("right"));
        }
    }
}
  
