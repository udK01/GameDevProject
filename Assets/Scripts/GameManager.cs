using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Player player;
    private SoundManager sm;

    private int score;
    private int bonusScore;
    private int currentHighestScore = 0; // In that run.
    private int highestScoreAchieved = 0; // Overall highscore

    private Vector3 startPos;
    private Vector3 leftEdge;
    private Vector3 rightEdge;

    [Header("Game Objects")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject gamePlayUI;
    [Header("Images")]
    [SerializeField] private Image doubleJumpImage;
    [SerializeField] private Image timeSlowImage;
    [SerializeField] private Image immunityImage;
    [Header("Texts")]
    [SerializeField] private Text notificationText;
    [SerializeField] private Text endScoreText;
    [SerializeField] private Text highestScoreAchievedText;
    [SerializeField] private Text doubleJumpText;
    [SerializeField] private Text timeSlowText;
    [SerializeField] private Text highscoreText;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        sm = FindObjectOfType<SoundManager>();
        startPos = player.transform.position;
        player.enabled = false;
        Time.timeScale = 0f;
        leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        highestScoreAchieved = PlayerPrefs.GetInt("Score");
    }

    public void NewGame()
    {
        ResetScore();
        ResetUI();
        Time.timeScale = 1f;
        player.Respawn();
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

    private void ResetScore()
    {
        SetScore(0);
        currentHighestScore = 0;
        bonusScore = 0;
    }

    private void ResetUI()
    {
        gameOverUI.SetActive(false);
        gamePlayUI.SetActive(true);
    }

    public int GetScore()
    {
        return score;
    }

    private void CalculateScore()
    {
        Vector3 currentPos = player.transform.position;
        int currentScore = (int)currentPos.y+bonusScore;
        if (currentScore > currentHighestScore)
        {
            SetScore(currentScore);
            currentHighestScore = currentScore;
            if (highestScoreAchieved < score)
            {
                highestScoreAchieved = score;
                PlayerPrefs.SetInt("Score", highestScoreAchieved);
            }
        }
    }

    public void SetBonusScore(int bonusScore)
    {
        this.bonusScore = bonusScore;
    }

    public int GetBonusScore()
    {
        return bonusScore;
    }

    public void GameOver()
    {
        player.gameObject.SetActive(false);
        gameOverUI.SetActive(true);
        gamePlayUI.SetActive(false);
        notificationText.enabled = false;
        endScoreText.text = score.ToString();
        highestScoreAchievedText.text = highestScoreAchieved.ToString();

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

    public void CleansePowerUps()
    {
        Time.timeScale = 1f;
        player.ChangeBlockDistance(1);
        player.SetObstacleImmunity(false);
        ChangeImageOpacity(doubleJumpImage, 0.5f);
        ChangeImageOpacity(timeSlowImage, 0.5f);
        ChangeImageOpacity(immunityImage, 0.5f);
        doubleJumpText.enabled = false;
        timeSlowText.enabled = false;
    }

    public SoundManager GetSoundManager()
    {
        return sm;
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
        notificationText.gameObject.SetActive(true);
        notificationText.text = text;
        Vector2 notificationPos = new Vector2(player.transform.position.x, player.transform.position.y+1);
        notificationText.transform.position = notificationPos;
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
        notificationText.gameObject.SetActive(false);
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
