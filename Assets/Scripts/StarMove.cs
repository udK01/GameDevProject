using System.Collections;
using UnityEngine;

public class StarMove : MonoBehaviour
{

    private int AmountToGive = 10;

    private void Awake()
    {
        StopAllCoroutines();
        StartCoroutine(nameof(StartMove), 0f);
    }

    /// <summary>
    /// Waits for 2seconds and advance the star by 1 lane.
    /// For every advancement, remove 1 point awarded.
    /// </summary>
    /// <returns> Nothing </returns>
    IEnumerator StartMove()
    {
        for (int i = 0; i < 10; i++)
        {
            transform.position = transform.position + Vector3.up;
            AmountToGive--;
            yield return new WaitForSeconds(2);
        }
        Destroy(this.gameObject);
    }

    /// <summary>
    /// On collision, if collided with player set score to current score 
    /// and bonus score awarded from star and destroy this object.
    /// </summary>
    /// <param name="collision"> Object Collided With </param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            SoundManager.Instance.PlaySound("StarPickUp");
            GameManager.Instance.bonusScore = (GameManager.Instance.bonusScore + AmountToGive);
            StartCoroutine(nameof(DisplayPoints), 0f);
        }
    }

    IEnumerator DisplayPoints()
    {
        GameManager.Instance.SetNotificationText("+" + AmountToGive.ToString());
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(2);
        GameManager.Instance.DisableNotificationText();
        Destroy(this.gameObject);
    }

}
