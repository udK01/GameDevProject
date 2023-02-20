using UnityEngine;

public class Player : MonoBehaviour
{

    private Vector3 initialPos;
    private PowerUp powerUpObject;
    [SerializeField] private int blockDistance = 1;
    [SerializeField] private bool obstacleImmunity;

    private void Awake()
    {
        initialPos = transform.position;
        powerUpObject = FindObjectOfType<PowerUp>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) {
            Move(Vector3.up * blockDistance);
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            Move(Vector3.left * blockDistance);
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            Move(Vector3.right * blockDistance);
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            Move(Vector3.down * blockDistance);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(obstacleImmunity);
        }
    }

    private void Move(Vector3 direction)
    {
        Vector3 destination = transform.position + direction;

        Collider2D barrier = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Barrier"));
        Collider2D platform = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Platform"));
        Collider2D obstacle = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Obstacle"));
        Collider2D powerup = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("PowerUp"));

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
        if (obstacle != null && platform == null && obstacleImmunity == false)
        {
            transform.position += direction;
            Death();
        } else
        {
            transform.position += direction;
        }
        if (powerup != null)
        {
            powerUpObject.GivePowerUp();
        }
    }

    public void SetObstacleImmunity(bool obstacleImmunity)
    {
        this.obstacleImmunity = obstacleImmunity;
    }

    public void ChangeBlockDistance(int blockDistance)
    {
        this.blockDistance = blockDistance;
    }

    private void Death()
    {
        enabled = false;
        FindObjectOfType<GameManager>().GameOver();
    }

    public void Respawn()
    {
        transform.position = initialPos;
        gameObject.SetActive(true);
        enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") && transform.parent == null)
        {
            if (obstacleImmunity == false)
            {
                Death();
            } else
            {
                Destroy(collision.gameObject);
                SetObstacleImmunity(false);
            }
        } 
    }
}
  
