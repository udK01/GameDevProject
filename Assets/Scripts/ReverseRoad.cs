using UnityEngine;

public class ReverseRoad : MonoBehaviour
{
    /// <summary>
    /// On collision, reverse controls.
    /// </summary>
    /// <param name="collision"> Object Collided With </param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player.Instance.reverseMove = true;
    }

    /// <summary>
    /// On collision exit, return controls to regular.
    /// </summary>
    /// <param name="collision"> Object Collided With </param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        Player.Instance.reverseMove = false;
    }
}
