using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum PowerUps { DOUBLEJUMP, TIMESLOW, IMMUNITY }

public class PowerUp : MonoBehaviour
{
    private PowerUps powerUp;
    private Player player;
    private GameManager gm;
    [SerializeField] private int jumpBoostDuration;
    [SerializeField] private int timeSlowDuration;

    private Image doubleJumpImage;
    private Image timeSlowImage;
    private Image immunityImage;
    private Text doubleJumpText;
    private Text timeSlowText;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        gm = FindObjectOfType<GameManager>();
        doubleJumpImage = gm.GetDoubleJumpImage();
        timeSlowImage = gm.GetTimeSlowImage();
        immunityImage = gm.GetImmunityImage();
        doubleJumpText = gm.GetDoubleJumpText();
        timeSlowText = gm.GetTimeSlowText();
    }

    /// <summary>
    /// Randomly assigns a power up.
    /// </summary>
    public void GivePowerUp()
    {
        powerUp = (PowerUps)Random.Range(0, 3);
        switch (powerUp)
        {
            case PowerUps.DOUBLEJUMP:
                DoubleJump();
                break;
            case PowerUps.TIMESLOW:
                TimeSlow();
                break;
            case PowerUps.IMMUNITY:
                ObstacleImmunity();
                break;
        }
    }

    /// <summary>
    /// Clears the double jump text, changes the image's opacity,
    /// throws a notification and runs the deactivation method.
    /// </summary>
    private void DoubleJump()
    {
        doubleJumpText.text = "";
        gm.ChangeImageOpacity(doubleJumpImage, 1f);
        gm.SetNotificationText("Double Jump!");
        StopAllCoroutines();
        StartCoroutine(nameof(DeactivateDoubleJump), 0f);
    }

    /// <summary>
    /// Doubles the block distance, starts the powerup countdown,
    /// waits for jumpBoostDuration (5sec), turns off text and image, 
    /// resets player jump distance and destroys the powerup.
    /// </summary>
    /// <returns> Nothing </returns>
    IEnumerator DeactivateDoubleJump()
    {
        player.SetBlockDistance(2);
        StartCoroutine(nameof(CountDownDoubleJump), 0f);
        yield return new WaitForSeconds(jumpBoostDuration);
        gm.ChangeImageOpacity(doubleJumpImage, 0.5f);
        doubleJumpText.enabled = false;
        player.SetBlockDistance(1);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Every 1 second decrements the powerup counter once.
    /// </summary>
    /// <returns> Nothing </returns>
    IEnumerator CountDownDoubleJump()
    {
        doubleJumpText.enabled = true;
        for (int i = jumpBoostDuration; i > 0; i--)
        {
            doubleJumpText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
    }

    /// <summary>
    /// Clears time slow text, changes the image's opacity,
    /// throws a notification and runs the deactivation method.
    /// </summary>
    private void TimeSlow()
    {
        timeSlowText.text = "";
        gm.ChangeImageOpacity(timeSlowImage, 1f);
        gm.SetNotificationText("Time Slow!");
        StopAllCoroutines();
        StartCoroutine(nameof(DeactivateTimeSlow), 0f);
    }

    /// <summary>
    /// Halves the local  time, starts the powerup countdown,
    /// waits timeSlowDuration (3sec), turns off text and image,
    /// resets time and destroys the powerup.
    /// </summary>
    /// <returns> Nothing </returns>
    IEnumerator DeactivateTimeSlow()
    {
        Time.timeScale = 0.5f;
        StartCoroutine(nameof(CountDownTimeSlow), 0f);
        yield return new WaitForSeconds(timeSlowDuration);
        gm.ChangeImageOpacity(timeSlowImage, 0.5f);
        timeSlowText.enabled = false;
        Time.timeScale = 1f;
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Every 1 second decrements the powerup counter once.
    /// </summary>
    /// <returns> Nothing </returns>
    IEnumerator CountDownTimeSlow()
    {
        timeSlowText.enabled = true;
        for (int i = timeSlowDuration; i > 0; i--)
        {
            timeSlowText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
    }

    /// <summary>
    /// Changes the image's opacity, throws a notification 
    /// and destroys the powerup.
    /// </summary>
    private void ObstacleImmunity()
    {
        gm.ChangeImageOpacity(immunityImage, 1f);
        gm.SetNotificationText("Immunity!");
        player.SetObstacleImmunity(true);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// On collision, give powerup, disabled collider and sprite.
    /// </summary>
    /// <param name="collision"> Object Collided With </param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GivePowerUp();
            FindObjectOfType<GameManager>().GetSoundManager().PlaySound("PowerUp");
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
