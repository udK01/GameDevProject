using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Vector2 direction = Vector2.right;
    [SerializeField] private float speed = 1f;
    [SerializeField] private double size = 1f;

    private Vector3 leftEdge;
    private Vector3 rightEdge;
    private Player player;
    private GameManager gm;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        gm = FindObjectOfType<GameManager>();
        leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
    }

    private void Start()
    {
        InvokeRepeating(nameof(OutOfBounds), 0f, 1f);
    }

    private void Update()
    {
        if (direction.x > 0 && (transform.position.x - size) > rightEdge.x)
        {
            Vector3 position = transform.position;
            position.x = leftEdge.x - (int)size;
            transform.position = position;
        }
        else if (direction.x < 0 && (transform.position.x + size) < leftEdge.x)
        {
            Vector3 position = transform.position;
            position.x = rightEdge.x + (int)size;
            transform.position = position;
        }
        else
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    private void OutOfBounds()
    {
        int playerAway = (int)player.transform.position.y - 10;
        if (playerAway >= gameObject.transform.position.y)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator ImmunityOver()
    {
        yield return new WaitForSeconds(2);
        gm.ChangeImageOpacity(gm.GetImmunityImage(), 0.5f);
        player.SetObstacleImmunity(false);
    }

    private void WaterDeath()
    {
        if (this.gameObject.layer == LayerMask.NameToLayer("Water") && player.transform.parent == null)
        {
            player.Death();
        }
    }

    private void CarDeath(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && player.transform.parent == null)
        {
            if (player.GetObstacleImmunity() == false)
            {
                player.Death();
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(nameof(ImmunityOver), 0f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CarDeath(collision);
        WaterDeath();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        WaterDeath();
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetSize(double size)
    {
        this.size = size;
    }

}
