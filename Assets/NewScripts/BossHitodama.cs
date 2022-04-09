using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitodama : MonoBehaviour
{
    bool isHigh;

    void Update()
    {
        if (transform.position.y > 2)
        {
            transform.Translate(0, -0.01f, 0);
            isHigh = true;
        }
        if (transform.position.y < 1)
        {
            transform.Translate(0, 0.01f, 0);
            isHigh = false;
        }
        if (transform.position.y >= 1 && transform.position.y <= 2 && isHigh)
        {
            transform.Translate(0, -0.01f, 0);
        }
        else
        {
            transform.Translate(0, 0.01f, 0);
        }
    }
}
