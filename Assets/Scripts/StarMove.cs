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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            gm.SetBonusScore(gm.GetBonusScore()+AmountToGive);
            Destroy(this.gameObject);
        }
    }

}
