using System.Collections;
using UnityEngine;

public class LavaRoad : MonoBehaviour
{
    private const int BASE_LAVA_TIME = 3;
    private Player player;
    private int LavaTime = BASE_LAVA_TIME;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    /// <summary>
    /// On collision, invokes the "LavaDamage" method every 10seconds, which
    /// kills the player if they stayed on the lava road for 10seconds.
    /// </summary>
    /// <param name="collision"> Object Collided With </param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        InvokeRepeating(nameof(LavaDamage), 0f, 1f);
    }

    /// <summary>
    /// On collision exit cancels the repeating invoke.
    /// </summary>
    /// <param name="collision"> Object Collided With </param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        CancelInvoke();
        LavaTime = BASE_LAVA_TIME;
        FindObjectOfType<GameManager>().DisableNotificationText();
    }

    /// <summary>
    /// Kills the player if they stayed on the lava road for 10seconds.
    /// </summary>
    /// <returns> Nothing </returns>
    private void LavaDamage()
    {
        if (LavaTime == 0)
        {
            player.Death();
            CancelInvoke();
        }
        else
        {
            FindObjectOfType<GameManager>().SetNotificationText(LavaTime.ToString());
        }
        LavaTime--;
    }
}
