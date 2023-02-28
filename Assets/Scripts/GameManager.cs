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

    /// <summary>
    /// Reset player, ui and map to default.
    /// </summary>
    public void NewGame()
    {
        ResetScore();
        ResetUI();
        Time.timeScale = 1f;
        player.Respawn();
        player.enabled = true;
    }

    /// <summary>
    /// Exit the game.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    /// <summary>
    /// Setter for private variable.
    /// </summary>
    /// <param name="score"> Score From Advancing Lanes </param>
    public void SetScore(int score)
    {
        this.score = score;
        highscoreText.text = score.ToString();
    }

    /// <summary>
    /// Resets the score.
    /// </summary>
    private void ResetScore()
    {
        SetScore(0);
        currentHighestScore = 0;
        bonusScore = 0;
    }

    /// <summary>
    /// Resets UI.
    /// </summary>
    private void ResetUI()
    {
        gameOverUI.SetActive(false);
        gamePlayUI.SetActive(true);
    }

    /// <summary>
    /// Getter for private variable.
    /// </summary>
    /// <returns> Score </returns>
    public int GetScore()
    {
        return score;
    }

    /// <summary>
    /// Calculates score based on player's current position and bonus points.
    /// If a new highscore is reached it saves that.
    /// </summary>
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

    /// <summary>
    /// Setter for private variable.
    /// </summary>
    /// <param name="bonusScore"> Bonus Score From Stars </param>
    public void SetBonusScore(int bonusScore)
    {
        this.bonusScore = bonusScore;
    }

    /// <summary>
    /// Getter for private variable.
    /// </summary>
    /// <returns> Bonus Score </returns>
    public int GetBonusScore()
    {
        return bonusScore;
    }

    /// <summary>
    /// Disables the player, activates the game over UI, 
    /// and starts listening for the replay key.
    /// </summary>
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

    /// <summary>
    /// Listens to the replay key (R) and 
    /// starts a new game once pressed.
    /// </summary>
    /// <returns> Nothing </returns>
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

    /// <summary>
    /// Freezes the game and shows the Menu.
    /// </summary>
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

    /// <summary>
    /// Enables the player, and unfreezes the game.
    /// </summary>
    public void UnPauseGame()
    {
        Time.timeScale = 1f;
        player.enabled = true;
    }

    /// <summary>
    /// Clears all powerups effects, and resets their display.
    /// </summary>
    public void CleansePowerUps()
    {
        Time.timeScale = 1f;
        player.SetBlockDistance(1);
        player.SetObstacleImmunity(false);
        ChangeImageOpacity(doubleJumpImage, 0.5f);
        ChangeImageOpacity(timeSlowImage, 0.5f);
        ChangeImageOpacity(immunityImage, 0.5f);
        doubleJumpText.enabled = false;
        timeSlowText.enabled = false;
    }

    /// <summary>
    /// Getter for private variable.
    /// </summary>
    /// <returns> Sound Manager </returns>
    public SoundManager GetSoundManager()
    {
        return sm;
    }

    /// <summary>
    /// Getter for private variable.
    /// </summary>
    /// <returns> Double Jump Image </returns>
    public Image GetDoubleJumpImage()
    {
        return doubleJumpImage;
    }

    /// <summary>
    /// Getter for private variable.
    /// </summary>
    /// <returns> Time Slow Image </returns>
    public Image GetTimeSlowImage()
    {
        return timeSlowImage;
    }

    /// <summary>
    /// Getter for private variable.
    /// </summary>
    /// <returns> Immunity Image </returns>
    public Image GetImmunityImage()
    {
        return immunityImage;
    }

    /// <summary>
    /// Getter for private variable.
    /// </summary>
    /// <returns> Double Jump Text </returns>
    public Text GetDoubleJumpText()
    {
        return doubleJumpText;
    }

    /// <summary>
    /// Getter for private variable.
    /// </summary>
    /// <returns> Time Slow Text </returns>
    public Text GetTimeSlowText()
    {
        return timeSlowText;
    }

    /// <summary>
    /// Setter for private variable.
    /// </summary>
    /// <param name="text"> String To Display </param>
    public void SetNotificationText(string text)
    {
        notificationText.gameObject.SetActive(true);
        notificationText.text = text;
        Vector2 notificationPos = new Vector2(player.transform.position.x, player.transform.position.y+1);
        notificationText.transform.position = notificationPos;
        StopAllCoroutines();
        StartCoroutine(nameof(WipeText), 0f);
    }

    /// <summary>
    /// Changes the image's opacity to the specified amount.
    /// </summary>
    /// <param name="img"> Image To Change </param>
    /// <param name="opacity"> Opacity To Change To </param>
    public void ChangeImageOpacity(Image img, float opacity)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, opacity);
    }

    /// <summary>
    /// Waits for 2seconds and clears notification text.
    /// </summary>
    /// <returns> Nothing </returns>
    IEnumerator WipeText()
    {
        yield return new WaitForSeconds(2);
        notificationText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Checks if player is on the map and disables 
    /// player if not, re-enables them if they are.
    /// </summary>
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

    /// <summary>
    /// Every Update checks if player wants to pause, 
    /// calculates score and ensures player is on the map.
    /// </summary>
    private void Update()
    {
        CheckPlayerOnMap();
        CalculateScore();
        PauseGame();
    }
}
