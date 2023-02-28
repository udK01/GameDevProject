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

    /// <summary>
    /// On Update, move object based on direction, if they exceed an outer edge, loop around.
    /// </summary>
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

    /// <summary>
    /// After 2 seconds it changes the powerup icon and removes its effect.
    /// </summary>
    /// <returns> Waits For 2 Seconds </returns>
    IEnumerator ImmunityOver()
    {
        yield return new WaitForSeconds(2);
        gm.ChangeImageOpacity(gm.GetImmunityImage(), 0.5f);
        player.SetObstacleImmunity(false);
    }

    /// <summary>
    /// If game object is water, play water death sound effect and kill player.
    /// </summary>
    private void WaterDeath()
    {
        if (this.gameObject.layer == LayerMask.NameToLayer("Water") && player.transform.parent == null)
        {
            FindObjectOfType<GameManager>().GetSoundManager().PlaySound("WaterDeath");
            player.Death();
        }
    }


    /// <summary>
    /// If collided with player, player is not attached to a platform,
    /// obstacle immunity is false and not dead then play car death and kill player.
    /// </summary>
    /// <param name="collision"> Object Collided With </param>
    private void CarDeath(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && player.transform.parent == null)
        {
            if (player.GetObstacleImmunity() == false && player.enabled == true)
            {
                FindObjectOfType<GameManager>().GetSoundManager().PlaySound("CarDeath");
                player.Death();
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(nameof(ImmunityOver), 0f);
            }
        }
    }

    /// <summary>
    /// On collision check for water death then car death.
    /// </summary>
    /// <param name="collision"> Object Collided With </param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        WaterDeath();
        CarDeath(collision);
    }

    /// <summary>
    /// On collision stay, check for water death continuously.
    /// </summary>
    /// <param name="collision"> Object Collided With </param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        WaterDeath();
    }

    /// <summary>
    /// Setter for private variable.
    /// </summary>
    /// <param name="direction"> Object Direction </param>
    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    /// <summary>
    /// Setter for private variable.
    /// </summary>
    /// <param name="speed"> Object Speed </param>
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    /// <summary>
    /// Setter for private variable.
    /// </summary>
    /// <param name="size"> Object Size </param>
    public void SetSize(double size)
    {
        this.size = size;
    }

}
