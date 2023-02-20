using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int score;
    private int highestScore = 0;
    private Vector3 startPos;
    private Player player;

    [SerializeField] GameObject gameOverUI;
    [SerializeField] Text highscoreText;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        startPos = player.transform.position;
    }

    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        SetScore(0);
        gameOverUI.SetActive(false);
        highestScore = 0;
        player.Respawn();
    }

    private void SetScore(int score)
    {
        this.score = score;
        highscoreText.text = score.ToString();
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

    public void GameOver()
    {
        player.gameObject.SetActive(false);
        gameOverUI.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(PlayAgain());
    }

    private IEnumerator PlayAgain()
    {
        bool playAgain = false;

        while (!playAgain)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                playAgain = true;
            }

            yield return null;
        }

        Invoke(nameof(NewGame), 1f);
    }

    private void Update()
    {
        CalculateScore();
    }
}
