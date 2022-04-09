using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewSelectQ : MonoBehaviour
{
    // 画面上のテキストを持ってくる
    [SerializeField] Text qText; // 問題用のテキスト
    [SerializeField] Text aText; // 解答用のテキスト

    [SerializeField] Text qText1; // 問題用のテキスト
    [SerializeField] Text aText1; // 解答用のテキスト

    // 問題を用意しておく
    private string[] _question = { "いいよ", "いいね" };
    private string[] _answer = { "iiyo", "iine" };

    private string[] _question1 = { "やだ", "やだね" };
    private string[] _answer1 = { "yada", "yadane" };

    // 何番目か指定するためのstring
    private string _qString;
    private string _aString;

    private string _qString1;
    private string _aString1;

    // 何番目の問題か選び出すint
    private int _qNum;

    // 問題の何文字目か
    private int _aNum;
    private int _aNum1;


    // story進行度保存用のint
    int storyProgress;

    [SerializeField] Text talkText;
    [SerializeField] Text enterText;
    string enter = "ENTER▼";
    public Image frame;
    public SpriteRenderer output;

    AudioSource audioSource;
    public AudioClip deathSound;
    public AudioClip typingSound;
    public AudioClip enterSound;

    public SpriteRenderer background;
    SpriteRenderer deathBlind;

    public Animator animator;

    // ゲーム開始時に一度だけ実行されるもの
    void Start()
    {
        ChangeQ();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //story進行
        if (Input.GetKeyDown(KeyCode.Return))
        {
            audioSource.PlayOneShot(enterSound);

            if (storyProgress == 0)
            {
                talkText.text = "倒してくれるかい？";
                enterText.text = "";
                GameObject director = GameObject.Find("GameDirector");
                director.GetComponent<GameDirector>().QuestionAppear();
                qText.color = new Color32(255, 255, 255, 255);
                qText1.color = new Color32(255, 255, 255, 255);
                aText.color = new Color32(255, 255, 255, 255);
                aText1.color = new Color32(255, 255, 255, 255);
                storyProgress = 1;
            }
            if (storyProgress == 3)
            {
                storyProgress = 100; // 何度も入力できないようstoryProgressを適当に100にする
                StartCoroutine(GameOver());
            }
            if (storyProgress == 4)
            {
                storyProgress = 100;
                StartCoroutine(GameClear());
            }
            if (storyProgress == 2)
            {
                talkText.text = "いざ、妖怪退治へ！";
                storyProgress = 4;
            }
            if (storyProgress == 7)
            {
                storyProgress = 100;
                StartCoroutine(GameChange());
            }
            if (storyProgress == 6)
            {
                talkText.text = "(とりあえず妖怪とやらを倒すか...)";
                storyProgress = 7;
            }
            if (storyProgress == 5)
            {
                storyProgress = 100;
                EnemyAppear.Instance.StartCoroutine("EnemyErase");
                StartCoroutine(ZizouGoodBye());
            }
        }

        if (Input.GetKeyDown(_aString[_aNum].ToString()) && _aNum1 == 0 && storyProgress == 1)
        {
            Correct();
            if (_aNum >= _aString.Length)
            {
                Story1();
                storyProgress = 2;

                talkText.text = "ありがとう！";
            }
        }
        if (Input.GetKeyDown(_aString1[_aNum1].ToString()) && _aNum == 0 && storyProgress == 1)
        {
            Correct1();
            if (_aNum1 >= _aString1.Length)
            {
                Story1();
                storyProgress = 3;

                talkText.text = "君には失望したよ...";
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && storyProgress == 1)
        {
            _aNum = 0;
            _aNum1 = 0;
            aText.text = "<color=#FFFFFF>" + _aString + "</color>";
            aText1.text = "<color=#FFFFFF>" + _aString1 + "</color>";
        }
    }

    void ChangeQ()
    {
        _qNum = 0;

        _qString = _question[_qNum];
        _aString = _answer[_qNum];

        qText.text = _qString;
        aText.text = _aString;

        _qString1 = _question1[_qNum];
        _aString1 = _answer1[_qNum];

        qText1.text = _qString1;
        aText1.text = _aString1;
    }

    // 正解用の関数
    void Correct()
    {
        //正解した時の処理
        _aNum++;

        audioSource.PlayOneShot(typingSound);

        aText.text = "<color=#6A6A6A>" + _aString.Substring(0, _aNum) + "</color>" + _aString.Substring(_aNum);
    }

    void Correct1()
    {
        //正解した時の処理
        _aNum1++;

        audioSource.PlayOneShot(typingSound);

        aText1.text = "<color=#6A6A6A>" + _aString1.Substring(0, _aNum1) + "</color>" + _aString1.Substring(_aNum1);
    }

    void Story1()
    {
        _aNum = 0;
        _aNum1 = 0;
        aText.text = "<color=#FFFFFF>" + _aString + "</color>";
        aText1.text = "<color=#FFFFFF>" + _aString1 + "</color>";
        GameObject director = GameObject.Find("GameDirector");
        director.GetComponent<GameDirector>().QuestionDisappear();

        qText.text = "";
        qText1.text = "";
        aText.text = "";
        aText1.text = "";
        enterText.text = enter;

        audioSource.PlayOneShot(enterSound);
    }

    IEnumerator GameClear()
    {
        for (int i = 0; i < 255; i++)
        {
            background.color -= new Color32(0,0, 0, 1);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1f);
        talkText.text = "魑魅魍魎が跳梁跋扈する夜になったね！\r\nさあ倒していこう！";
        storyProgress = 5;
    }

    IEnumerator GameOver()
    {
        animator.SetTrigger("AngerTrigger");
        audioSource.PlayOneShot(deathSound);

        Destroy(GameObject.Find("frame"));
        Destroy(GameObject.Find("OutputLocation1"));
        talkText.text = "";
        enterText.text = "";

        EnemyAppear.Instance.StartCoroutine("ZizouZoom");

        for (int i = 0; i < 255; i++)
        {
            deathBlind = GameObject.Find("DeathBlind").GetComponent<SpriteRenderer>();
            deathBlind.color += new Color32(0, 0, 0, 1);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("StartScreen");
    }

    IEnumerator ZizouGoodBye()
    {
        yield return new WaitForSeconds(3f);
        talkText.text = "(行ってしまった...)";
        storyProgress = 6;
    }

    IEnumerator GameChange()
    {
        talkText.text = "";
        enterText.text = "";
        frame.color = new Color32(0, 0, 0, 0);
        output.color = new Color32(0, 0, 0, 0);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Stage2");
    }
}
