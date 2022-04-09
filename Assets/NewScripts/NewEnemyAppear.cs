using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyAppear : MonoBehaviour
{
    public static NewEnemyAppear Instance;

    Vector2 enemy;
    bool isBig = true;
    public float upEnemy;
    public float sizeEnemy;

    public int enemyNumber;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    [System.Obsolete]
    void Update()
    {
        GameObject select = GameObject.Find("Story2Manager");
        if (enemyNumber == select.GetComponent<SelectQ>().Indvidual)
        {
            if (enemy.x > sizeEnemy)
            {
                isBig = false;
            }
            if (isBig)
            {
                enemy = gameObject.transform.localScale;
                enemy.x += 0.01f;
                enemy.y += 0.01f;
                gameObject.transform.localScale = enemy;

                transform.Translate(0, upEnemy, 0);
            }
            if (!isBig)
            {
                enemy = gameObject.transform.localScale;
                enemy.x += 0.002f;
                enemy.y += 0.002f;
                gameObject.transform.localScale = enemy;

                transform.Translate(0, 0.001f, 0);
            }
        }

        // 時間内に撃破
        if (enemyNumber == select.GetComponent<SelectQ>().Indvidual && select.GetComponent<SelectQ>().isKill)
        {
            StartCoroutine(KillEnemy());
        }
        // タイムオーバー
        if (enemyNumber == select.GetComponent<SelectQ>().Indvidual && select.GetComponent<SelectQ>().isDamage)
        {
            StartCoroutine(TimeOver());
        }
    }

    IEnumerator KillEnemy()
    {
        GetComponent<ParticleSystem>().Play();
        GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 0);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    [System.Obsolete]
    IEnumerator TimeOver()
    {
        GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 0);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
