using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameOverChange : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip sound;
    bool isStart = true;

    public Text over;
    public Text retry;
    public Text title;
    public Text del;

    [SerializeField] Text scoreText;
    [SerializeField] Text highScoreText;
    [SerializeField] Text averageText;
    [SerializeField] Text rankingText;

    public Text start1;
    public Text start2;
    public Text start3;
    public Text start4;

    public SpriteRenderer forest;
    public SpriteRenderer zizou;
    public SpriteRenderer stone;
    public SpriteRenderer stone1;
    public SpriteRenderer stone2;

    public SpriteRenderer Stage4Back;
    public SpriteRenderer Stage4Back1;
    public SpriteRenderer Stage4Back2;
    public SpriteRenderer Stage4Back3;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        GameObject count = GameObject.Find("ScoreManager");
        count.GetComponent<ScoreManager>().SaveScore();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isStart)
        {
            isStart = false;
            GameObject count = GameObject.Find("ScoreManager");
            count.GetComponent<ScoreManager>().DelStatic();
            StartCoroutine(TitleReturn());
        }
        if (Input.GetKeyDown(KeyCode.Return) && isStart)
        {
            isStart = false;
            GameObject count = GameObject.Find("ScoreManager");
            count.GetComponent<ScoreManager>().DelStatic();
            StartCoroutine(StageRetry());
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            GameObject count = GameObject.Find("ScoreManager");
            count.GetComponent<ScoreManager>().DelScore();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SceneManager.LoadScene("RankingScene");
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
            del.color -= new Color32(0, 0, 0, 1);
            scoreText.color -= new Color32(0, 0, 0, 1);
            highScoreText.color -= new Color32(0, 0, 0, 1);
            averageText.color -= new Color32(0, 0, 0, 1);
            rankingText.color -= new Color32(0, 0, 0, 1);

            yield return new WaitForSeconds(0.006f);
        }
        SceneManager.LoadScene("StartScreen");
    }

    IEnumerator StageRetry()
    {
        audioSource.PlayOneShot(sound);
        for (int i = 0; i < 255; i++)
        {
            Stage4Back.color += new Color32(0, 0, 0, 1);
            Stage4Back1.color += new Color32(0, 0, 0, 1);
            Stage4Back2.color += new Color32(0, 0, 0, 1);
            Stage4Back3.color += new Color32(0, 0, 0, 1);

            over.color -= new Color32(0, 0, 0, 1);
            retry.color -= new Color32(0, 0, 0, 1);
            title.color -= new Color32(0, 0, 0, 1);
            del.color -= new Color32(0, 0, 0, 1);
            scoreText.color -= new Color32(0, 0, 0, 1);
            highScoreText.color -= new Color32(0, 0, 0, 1);
            averageText.color -= new Color32(0, 0, 0, 1);
            rankingText.color -= new Color32(0, 0, 0, 1);

            yield return new WaitForSeconds(0.006f);
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MiniGame");
    }
}
