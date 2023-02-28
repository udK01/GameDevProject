using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(CheckBounds), 0f, 1f);
    }

    public void CheckBounds()
    {
        int playerAway = (int)player.transform.position.y - 10;
        if (playerAway >= gameObject.transform.position.y)
        {
            Destroy(gameObject);
        }
    }
}
