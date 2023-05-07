using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum PowerUps { DOUBLEJUMP, TIMESLOW, IMMUNITY }

public class PowerUp : MonoBehaviour
{
    private PowerUps powerUp;
    [SerializeField] private int jumpBoostDuration;
    [SerializeField] private int timeSlowDuration;

    private Image doubleJumpImage;
    private Image timeSlowImage;
    private Image immunityImage;
    private Text doubleJumpText;
    private Text timeSlowText;

    private void Awake()
    {
        doubleJumpImage = GameManager.Instance.doubleJumpImage;
        timeSlowImage = GameManager.Instance.timeSlowImage;
        immunityImage = GameManager.Instance.immunityImage;
        doubleJumpText = GameManager.Instance.doubleJumpText;
        timeSlowText = GameManager.Instance.timeSlowText;
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
        GameManager.Instance.ChangeImageOpacity(doubleJumpImage, 1f);
        GameManager.Instance.SetNotificationText("Double Jump!");
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
        Player.Instance.blockDistance = 2;
        StartCoroutine(nameof(CountDownDoubleJump), 0f);
        yield return new WaitForSeconds(jumpBoostDuration);
        GameManager.Instance.ChangeImageOpacity(doubleJumpImage, 0.5f);
        doubleJumpText.enabled = false;
        Player.Instance.blockDistance = 1;
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
        GameManager.Instance.ChangeImageOpacity(timeSlowImage, 1f);
        GameManager.Instance.SetNotificationText("Time Slow!");
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
        GameManager.Instance.ChangeImageOpacity(timeSlowImage, 0.5f);
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
        GameManager.Instance.ChangeImageOpacity(immunityImage, 1f);
        GameManager.Instance.SetNotificationText("Immunity!");
        Player.Instance.obstacleImmunity = true;
    }

    /// <summary>
    /// On collision, give powerup, disabled collider and sprite.
    /// </summary>
    /// <param name="collision"> Object Collided With </param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(nameof(PowerUpNotify), 0f);
        }
    }

    IEnumerator PowerUpNotify()
    {
        GivePowerUp();
        SoundManager.Instance.PlaySound("PowerUp");
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(2);
        GameManager.Instance.DisableNotificationText();
        // In case it wasn't destroyed.
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }

}
