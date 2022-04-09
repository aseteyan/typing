using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitodamaMove : MonoBehaviour
{
    bool isHigh;

    void Update()
    {
        if (transform.position.x < -12)
        {
            transform.position = new Vector3(12f, 1.5f, 0);
        }

        if (transform.position.y > 2)
        {
            transform.Translate(-0.01f, -0.01f, 0);
            isHigh = true;
        }
        if (transform.position.y < 1)
        {
            transform.Translate(-0.015f, 0.01f, 0);
            isHigh = false;
        }
        if (transform.position.y >= 1 && transform.position.y <= 2 && isHigh)
        {
            transform.Translate(-0.01f, -0.01f, 0);
        }
        else
        {
            transform.Translate(-0.015f, 0.01f, 0);
        }
    }
}
