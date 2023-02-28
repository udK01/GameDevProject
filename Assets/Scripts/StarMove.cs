using System.Collections;
using UnityEngine;

public class StarMove : MonoBehaviour
{

    private GameManager gm;
    private int AmountToGive = 10;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
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
            gm.SetBonusScore(gm.GetBonusScore()+AmountToGive);
            Destroy(this.gameObject);
        }
    }

}
