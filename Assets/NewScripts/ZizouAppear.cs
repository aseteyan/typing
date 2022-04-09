using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZizouAppear : MonoBehaviour
{
    Vector2 zizou;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public IEnumerator ZizouZoom()
    {
        while (zizou.x < 2)
        {
            zizou = gameObject.transform.localScale;
            zizou.x += 0.01f;
            zizou.y += 0.01f;
            gameObject.transform.localScale = zizou;

            transform.Translate(0, 0.015f, 0);
            yield return new WaitForSeconds(0.001f);
        }
    }

    public IEnumerator ZizouZooming()
    {
        while (zizou.x != 100)
        {
            zizou = gameObject.transform.localScale;
            zizou.x += 0.02f;
            zizou.y += 0.02f;
            gameObject.transform.localScale = zizou;

            transform.Translate(0, -0.022f, 0);
            yield return new WaitForSeconds(0.0005f);
        }
    }

    public IEnumerator Stone()
    {
        while (transform.position.x > -5)
        {
            transform.Translate(-0.02f, 0, 0);
            yield return new WaitForSeconds(0.005f);
        }

        audioSource.Play();

        SpriteRenderer zizou = GameObject.Find("ozizou").GetComponent<SpriteRenderer>();
        zizou.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(0.1f);
        zizou.color = new Color32(255, 255, 255, 255);
        yield return new WaitForSeconds(0.1f);
        zizou.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(0.1f);
        zizou.color = new Color32(255, 255, 255, 255);
        yield return new WaitForSeconds(0.2f);

        Destroy(GameObject.Find("man"));
        SpriteRenderer man = GameObject.Find("man1").GetComponent<SpriteRenderer>();
        man.color = new Color32(255, 255, 255, 255);

        while (transform.position.y > -10)
        {
            transform.Translate(0, -0.03f, 0);
            yield return new WaitForSeconds(0.005f);
        }
    }

    public IEnumerator ZizouGoodbye()
    {
        while (transform.position.x >= -5.72f)
        {
            zizou = gameObject.transform.localScale;
            zizou.x -= 0.009f;
            zizou.y -= 0.009f;
            gameObject.transform.localScale = zizou;

            transform.Translate(-0.05f, -0.006f, 0);
            yield return new WaitForSeconds(0.001f);
        }
    }
}
