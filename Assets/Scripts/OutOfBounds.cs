using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    /// <summary>
    /// Check if the object is out of bounds every 1second.
    /// </summary>
    private void Start()
    {
        InvokeRepeating(nameof(CheckBounds), 0f, 1f);
    }

    /// <summary>
    /// Check if the object is 10 blocks behind 
    /// the player and remove if it is.
    /// </summary>
    public void CheckBounds()
    {
        int playerAway = (int)Player.Instance.transform.position.y - 10;
        if (playerAway >= gameObject.transform.position.y)
        {
            Destroy(gameObject);
        }
    }
}
