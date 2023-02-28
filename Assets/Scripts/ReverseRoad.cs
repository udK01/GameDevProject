using UnityEngine;

public class ReverseRoad : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    /// <summary>
    /// On collision, reverse controls.
    /// </summary>
    /// <param name="collision"> Object Collided With </param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.SetReverseMove(true);
    }

    /// <summary>
    /// On collision stay, reverse controls.
    /// </summary>
    /// <param name="collision"> Object Collided With </param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        player.SetReverseMove(true);
    }

    /// <summary>
    /// On collision exit, return controls to regular.
    /// </summary>
    /// <param name="collision"> Object Collided With </param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        player.SetReverseMove(false);
    }
}
