using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
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

    public void GivePowerUp()
    {
        switch (Random.Range(1, 4))
        {
            case 1:
                DoubleJump();
                break;
            case 2:
                TimeSlow();
                break;
            case 3:
                ObstacleImmunity();
                break;
        }
    }

    private void DoubleJump()
    {
        doubleJumpText.text = "";
        gm.ChangeImageOpacity(doubleJumpImage, 1f);
        gm.SetNotificationText("DOUBLE JUMP ACQUIRED!");
        StopAllCoroutines();
        StartCoroutine(nameof(DeactivateDoubleJump), 0f);
    }

    IEnumerator DeactivateDoubleJump()
    {
        player.ChangeBlockDistance(2);
        StartCoroutine(nameof(CountDownDoubleJump), 0f);
        yield return new WaitForSeconds(jumpBoostDuration);
        gm.ChangeImageOpacity(doubleJumpImage, 0.5f);
        doubleJumpText.enabled = false;
        player.ChangeBlockDistance(1);
        Destroy(this.gameObject);
    }

    IEnumerator CountDownDoubleJump()
    {
        doubleJumpText.enabled = true;
        for (int i = jumpBoostDuration; i > 0; i--)
        {
            doubleJumpText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
    }

    private void TimeSlow()
    {
        timeSlowText.text = "";
        gm.ChangeImageOpacity(timeSlowImage, 1f);
        gm.SetNotificationText("TIME SLOW ACQUIRED!");
        StopAllCoroutines();
        StartCoroutine(nameof(DeactivateTimeSlow), 0f);
    }

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

    IEnumerator CountDownTimeSlow()
    {
        timeSlowText.enabled = true;
        for (int i = timeSlowDuration; i > 0; i--)
        {
            timeSlowText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
    }

    private void ObstacleImmunity()
    {
        gm.ChangeImageOpacity(immunityImage, 1f);
        gm.SetNotificationText("IMMUNITY ACQUIRED!");
        player.SetObstacleImmunity(true);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GivePowerUp();
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
