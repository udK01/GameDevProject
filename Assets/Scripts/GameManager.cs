using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int bonusScore { get; set; }
    public int score { get; private set; }
    public float starCount { get; set; } = 0;
    public float powerUpCount { get; set; } = 0;
    public Vector3 leftEdge { get; private set; }
    public Vector3 rightEdge { get; private set; }

    private int currentHighestScore = 0; // In that run.
    private int highestScoreAchieved = 0; // Overall highscore
    private Vector3 startPos;

    public Image doubleJumpImage { get; set; }
    public Image timeSlowImage { get; set; }
    public Image immunityImage { get; set; }

    public Text doubleJumpText { get; set; }
    public Text timeSlowText { get; set; }

    [Header("Animator")]
    [SerializeField] private Animator transition;
    [Header("Game Objects")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject menuBackground;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject gamePlayUI;
    [SerializeField] private GameObject achievementUI;
    [Header("Images")]
    [SerializeField] private Image _doubleJumpImage;
    [SerializeField] private Image _timeSlowImage;
    [SerializeField] private Image _immunityImage;
    [Header("Texts")]
    [SerializeField] private Text notificationText;
    [SerializeField] private Text endScoreText;
    [SerializeField] private Text highestScoreAchievedText;
    [SerializeField] private Text highscoreText;
    [SerializeField] private Text _doubleJumpText;
    [SerializeField] private Text _timeSlowText;

    private void Awake()
    {
        Instance = this;
        InitializeUI();
        Time.timeScale = 0f;
        leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        highestScoreAchieved = PlayerPrefs.GetInt("Score");
    }

    private void Start()
    {
        startPos = Player.Instance.transform.position;
        Player.Instance.enabled = false;
    }

    /// <summary>
    /// Reset player, ui and map to default.
    /// </summary>
    public void NewGame()
    {
        ResetScore();
        ResetUI();
        Time.timeScale = 1f;
        Player.Instance.Respawn();
        Player.Instance.enabled = true;
        //StopAllCoroutines();
    }

    public void LoadGame()
    {
        Time.timeScale = 1f;
        Player.Instance.gameObject.SetActive(true);
        Player.Instance.enabled = true;
        CleansePowerUps();
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
        achievementUI.GetComponent<CanvasGroup>().alpha = 0;
        notificationText.gameObject.SetActive(true);
        gameOverUI.SetActive(false);
        //gamePlayUI.SetActive(true);
    }

    /// <summary>
    /// Calculates score based on player's current position and bonus points.
    /// If a new highscore is reached it saves that.
    /// </summary>
    private void CalculateScore()
    {
        Vector3 currentPos = Player.Instance.transform.position;
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
    /// Disables the player, activates the game over UI, 
    /// and starts listening for the replay key.
    /// </summary>
    public void GameOver()
    {
        Player.Instance.gameObject.SetActive(false);
        gameOverUI.SetActive(true);
        gamePlayUI.SetActive(false);
        notificationText.gameObject.SetActive(false);
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
            if (Input.GetKey(KeyCode.Escape))
            {
                menuUI.SetActive(true);
                menuBackground.SetActive(true);
                menuUI.transform.GetChild(0).gameObject.SetActive(true);
                menuUI.transform.GetChild(1).gameObject.SetActive(false);
                gameOverUI.SetActive(false);
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
        if (Input.GetKey(KeyCode.Escape) && Player.Instance.enabled == true)
        {
            menuUI.SetActive(true);
            menuBackground.SetActive(true);
            gamePlayUI.SetActive(false);
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
        Player.Instance.enabled = true;
    }

    /// <summary>
    /// Clears all powerups effects, and resets their display.
    /// </summary>
    public void CleansePowerUps()
    {
        Time.timeScale = 1f;
        Player.Instance.blockDistance = 1;
        Player.Instance.obstacleImmunity = false;
        ChangeImageOpacity(doubleJumpImage, 0.5f);
        ChangeImageOpacity(timeSlowImage, 0.5f);
        ChangeImageOpacity(immunityImage, 0.5f);
        doubleJumpText.enabled = false;
        timeSlowText.enabled = false;
    }

    /// <summary>
    /// Setter for private variable.
    /// </summary>
    /// <param name="text"> String To Display </param>
    public void SetNotificationText(string text)
    {
        notificationText.gameObject.SetActive(true);
        notificationText.text = text;
        Vector2 notificationPos = new Vector2(Player.Instance.transform.position.x, Player.Instance.transform.position.y+1);
        notificationText.transform.position = notificationPos;
    }

    /// <summary>
    /// Disables notification text game object.
    /// </summary>
    public void DisableNotificationText()
    {
        notificationText.text = ""; 
        notificationText.gameObject.SetActive(false);
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
    /// Initializes the UI elements.
    /// </summary>
    private void InitializeUI()
    {
        doubleJumpImage = _doubleJumpImage;
        timeSlowImage = _timeSlowImage;
        immunityImage = _immunityImage;
        doubleJumpText = _doubleJumpText;
        timeSlowText = _timeSlowText;
    }

    /// <summary>
    /// Checks if player is on the map and disables 
    /// player if not, re-enables them if they are.
    /// </summary>
    private void CheckPlayerOnMap()
    {
        if (Player.Instance.transform.position.x < leftEdge.x)
        {
            Player.Instance.enabled = false;
        }
        else
        {
            Player.Instance.enabled = true;
        }
        if (Player.Instance.transform.position.x > rightEdge.x)
        {
            Player.Instance.enabled = false;
        }
        else
        {
            Player.Instance.enabled = true;
        }
    }

    public void MenuTransition()
    {
        StartCoroutine(nameof(Transition), 0f);
    }

    IEnumerator Transition()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(0.35f);
        gamePlayUI.SetActive(true);
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
