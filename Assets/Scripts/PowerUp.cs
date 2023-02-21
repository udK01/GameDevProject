using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    public void GivePowerUp()
    {
        switch (Random.Range(1, 4))
        {
            case 1:
                DoubleJump();
                break;
            case 2:
                TimeSlow();
                break;
            case 3:
                ObstacleImmunity();
                break;
        }
    }

    private void DoubleJump()
    {
        StopAllCoroutines();
        StartCoroutine(nameof(DeactivateDoubleJump), 0f);
    }

    IEnumerator DeactivateDoubleJump()
    {
        player.ChangeBlockDistance(2);
        Debug.Log("Double Jump Given!");
        yield return new WaitForSeconds(5);
        player.ChangeBlockDistance(1);
    }

    private void TimeSlow()
    {
        StopAllCoroutines();
        StartCoroutine(nameof(DeactivateTimeSlow), 0f);
    }

    IEnumerator DeactivateTimeSlow()
    {
        Time.timeScale = 0.5f;
        Debug.Log("Time Slow Given!");
        yield return new WaitForSeconds(3);
        Time.timeScale = 1f;
    }

    private void ObstacleImmunity()
    {
        player.SetObstacleImmunity(true);
        Debug.Log("Obstacle Immunity Given!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GivePowerUp();
            Destroy(this.gameObject);
        }
    }
}
