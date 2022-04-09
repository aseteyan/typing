using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAppear : MonoBehaviour
{
    public static EnemyAppear Instance;

    Vector2 enemy;
    bool isBig = true;
    bool isKill = true;
    public float upEnemy;
    public float sizeEnemy;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (enemy.x > sizeEnemy)
        {
            isBig = false;
        }
        if (isBig && isKill)
        {
            enemy = gameObject.transform.localScale;
            enemy.x += 0.01f;
            enemy.y += 0.01f;
            gameObject.transform.localScale = enemy;

            transform.Translate(0, upEnemy, 0);
        }
    }

    public IEnumerator EnemyErase()
    {
        while (enemy.x > 0)
        {
            enemy = gameObject.transform.localScale;
            enemy.x -= 0.01f;
            enemy.y -= 0.01f;
            gameObject.transform.localScale = enemy;

            transform.Translate(0, -upEnemy, 0);
            yield return new WaitForSeconds(0.001f);
        }
    }
    public IEnumerator ZizouZoom()
    {
        while (enemy.x > 0)
        {
            enemy = gameObject.transform.localScale;
            enemy.x += 0.01f;
            enemy.y += 0.01f;
            gameObject.transform.localScale = enemy;
            transform.Translate(0, -0.01f, 0);
            yield return new WaitForSeconds(0.001f);
        }

    }
}
