using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverChange : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip sound;
    bool isStart = true;

    public Text over;
    public Text retry;
    public Text title;

    public Text start1;
    public Text start2;
    public Text start3;
    public Text start4;

    public SpriteRenderer forest;
    public SpriteRenderer zizou;
    public SpriteRenderer stone;
    public SpriteRenderer stone1;
    public SpriteRenderer stone2;

    public SpriteRenderer Stage2Back;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isStart)
        {
            isStart = false;
            StartCoroutine(TitleReturn());
        }
        if (Input.GetKeyDown(KeyCode.Return) && isStart)
        {
            isStart = false;
            StartCoroutine(StageRetry());
        }
    }

    IEnumerator TitleReturn()
    {
        audioSource.PlayOneShot(sound);
        for (int i = 0; i < 254; i++)
        {
            forest.color += new Color32(0, 0, 0, 1);
            zizou.color += new Color32(0, 0, 0, 1);
            stone.color += new Color32(0, 0, 0, 1);
            stone1.color += new Color32(0, 0, 0, 1);
            stone2.color += new Color32(0, 0, 0, 1);
            start1.color += new Color32(0, 0, 0, 1);
            start2.color += new Color32(0, 0, 0, 1);
            start3.color += new Color32(0, 0, 0, 1);
            start4.color += new Color32(0, 0, 0, 1);

            over.color -= new Color32(0, 0, 0, 1);
            retry.color -= new Color32(0, 0, 0, 1);
            title.color -= new Color32(0, 0, 0, 1);

            yield return new WaitForSeconds(0.006f);
        }
        SceneManager.LoadScene("StartScreen");
    }

    IEnumerator StageRetry()
    {
        audioSource.PlayOneShot(sound);
        for (int i = 0; i < 255; i++)
        {
            Stage2Back.color += new Color32(0, 0, 0, 1);

            over.color -= new Color32(0, 0, 0, 1);
            retry.color -= new Color32(0, 0, 0, 1);
            title.color -= new Color32(0, 0, 0, 1);

            yield return new WaitForSeconds(0.006f);
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Stage2");
    }
}
