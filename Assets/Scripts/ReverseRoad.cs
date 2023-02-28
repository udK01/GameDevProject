using UnityEngine;

public class ReverseRoad : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.SetReverseMove(true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        player.SetReverseMove(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player.SetReverseMove(false);
    }
}
