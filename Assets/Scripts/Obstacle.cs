using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Vector2 direction { get; set; }
    public float speed { get; set; }
    public double size { get; set; }

    /// <summary>
    /// On Update, move object based on direction, if they exceed an outer edge, loop around.
    /// </summary>
    private void Update()
    {
        if (direction.x > 0 && (transform.position.x - size) > GameManager.Instance.rightEdge.x)
        {
            Vector3 position = transform.position;
            position.x = GameManager.Instance.leftEdge.x - (int)size;
            transform.position = position;
        }
        else if (direction.x < 0 && (transform.position.x + size) < GameManager.Instance.leftEdge.x)
        {
            Vector3 position = transform.position;
            position.x = GameManager.Instance.rightEdge.x + (int)size;
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
        GameManager.Instance.ChangeImageOpacity(GameManager.Instance.immunityImage, 0.5f);
        Player.Instance.obstacleImmunity = false;
    }

    /// <summary>
    /// If game object is water, play water death sound effect and kill player.
    /// </summary>
    private void WaterDeath()
    {
        if (this.gameObject.layer == LayerMask.NameToLayer("Water") && Player.Instance.transform.parent == null)
        {
            SoundManager.Instance.PlaySound("WaterDeath");
            GameManager.Instance.waterDeathCount++;
            Player.Instance.Death();
        }
    }

    /// <summary>
    /// If collided with player, player is not attached to a platform,
    /// obstacle immunity is false and not dead then play car death and kill player.
    /// </summary>
    /// <param name="collision"> Object Collided With </param>
    private void CarDeath(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && Player.Instance.transform.parent == null)
        {
            if (Player.Instance.obstacleImmunity == false && Player.Instance.enabled == true)
            {
                SoundManager.Instance.PlaySound("CarDeath");
                GameManager.Instance.carDeathCount++;
                Player.Instance.Death();
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
}
