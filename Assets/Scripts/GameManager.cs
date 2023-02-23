using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int score;
    private int currentHighestScore = 0;
    private Vector3 startPos;
    private Player player;
    private Vector3 leftEdge;
    private Vector3 rightEdge;

    [SerializeField] Text highscoreText;

    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject menuUI;

    [SerializeField] private Image doubleJumpImage;
    [SerializeField] private Image timeSlowImage;
    [SerializeField] private Image immunityImage;
    [SerializeField] private Text doubleJumpText;
    [SerializeField] private Text timeSlowText;
    [SerializeField] private GameObject notificationObject;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        startPos = player.transform.position;
        player.enabled = false;
        Time.timeScale = 0f;
        leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
    }

    public void NewGame()
    {
        SetScore(0);
        gameOverUI.SetActive(false);
        currentHighestScore = 0;
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
        if (currentScore > currentHighestScore)
        {
            SetScore(currentScore + bonusScore);
            currentHighestScore = currentScore;
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
            menuUI.transform.GetChild(0).gameObject.SetActive(false);
            menuUI.transform.GetChild(1).gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1f;
        player.enabled = true;
    }

    public Image GetDoubleJumpImage()
    {
        return doubleJumpImage;
    }

    public Image GetTimeSlowImage()
    {
        return timeSlowImage;
    }

    public Image GetImmunityImage()
    {
        return immunityImage;
    }

    public Text GetDoubleJumpText()
    {
        return doubleJumpText;
    }

    public Text GetTimeSlowText()
    {
        return timeSlowText;
    }

    public void SetNotificationText(string text)
    {
        notificationObject.SetActive(true);
        notificationObject.GetComponentInChildren<Text>().text = text;
        StopAllCoroutines();
        StartCoroutine(nameof(WipeText), 0f);
    }

    public void ChangeImageOpacity(Image img, float opacity)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, opacity);
    }

    IEnumerator WipeText()
    {
        yield return new WaitForSeconds(2);
        notificationObject.SetActive(false);
    }

    private void CheckPlayerOnMap()
    {
        if (player.transform.position.x < leftEdge.x)
        {
            player.enabled = false;
        }
        else
        {
            player.enabled = true;
        }
        if (player.transform.position.x > rightEdge.x)
        {
            player.enabled = false;
        }
        else
        {
            player.enabled = true;
        }
    }

    private void Update()
    {
        CheckPlayerOnMap();
        CalculateScore();
        PauseGame();
    }
}
