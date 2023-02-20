using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int score;
    private int highestScore = 0;
    private Vector3 startPos;
    private PlayerMovement player;

    private void Awake()
    {
        SetScore(0);
        player = FindObjectOfType<PlayerMovement>();
        startPos = player.transform.position;
    }

    private void SetScore(int score)
    {
        this.score = score;
    }

    private void CalculateScore()
    {
        Vector3 currentPos = player.transform.position;
        int currentScore = (int)(currentPos.y - startPos.y);
        if (currentScore > highestScore)
        {
            SetScore(currentScore);
            highestScore = currentScore;
        }
    }

    private void Update()
    {
        CalculateScore();
    }
}
