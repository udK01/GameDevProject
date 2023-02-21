using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int score;
    private int highestScore = 0;
    private Vector3 startPos;
    private Player player;

    [SerializeField] Text highscoreText;

    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject menuUI;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        startPos = player.transform.position;
        player.enabled = false;
        Time.timeScale = 0f;
    }

    public void NewGame()
    {
        SetScore(0);
        gameOverUI.SetActive(false);
        highscoreText.gameObject.SetActive(true);
        highestScore = 0;
        player.Respawn();
        Time.timeScale = 1f;
        player.enabled = true;
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void SetScore(int score)
    {
        this.score = score;
        highscoreText.text = score.ToString();
    }

    public int GetScore()
    {
        return score;
    }

    private void CalculateScore()
    {
        Vector3 currentPos = player.transform.position;
        int currentScore = (int)(currentPos.y - startPos.y);
        int bonusScore = score - (currentScore - 1);
        if (currentScore > highestScore)
        {
            SetScore(currentScore + bonusScore);
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

    private void PauseGame()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            menuUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void Update()
    {
        CalculateScore();
        PauseGame();
    }
}
