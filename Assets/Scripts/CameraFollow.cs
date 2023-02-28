using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    /// <summary>
    /// Smoothly adjusts the camera's y position 
    /// to match the player's current y position.
    /// </summary>
    private void Update()
    {
        float x = gameObject.transform.position.x;
        float y = Mathf.Lerp(gameObject.transform.position.y, player.transform.position.y, Time.deltaTime);
        gameObject.transform.position = new Vector3(x, y, -1); 
    }

}
