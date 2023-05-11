using System.Collections;
using UnityEngine;

public class NearHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(nameof(IsAlive), 0f);
        }
    }

    IEnumerator IsAlive()
    {
        yield return new WaitForSeconds(1);
        if (Player.Instance.gameObject.activeSelf)
        {
            GameManager.Instance.nearDeathCount++;
        }
    }
}
