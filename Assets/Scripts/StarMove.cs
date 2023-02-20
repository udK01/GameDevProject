using System.Collections;
using UnityEngine;

public class StarMove : MonoBehaviour
{

    private GameManager gm;

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
            yield return new WaitForSeconds(2);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            gm.SetScore(gm.GetScore() + 10);
            Destroy(this.gameObject);
        }
    }

}
