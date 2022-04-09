using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewSelectQ3 : MonoBehaviour
{
    [SerializeField] Text qText; // 問題用のテキスト
    [SerializeField] Text aText; // 解答用のテキスト

    [SerializeField] Text qText1; // 問題用のテキスト
    [SerializeField] Text aText1; // 解答用のテキスト

    private string[] _question = { "妖怪を倒す", "倒す", "帰る" };
    private string[] _answer = { "youkaiwotaosu", "taosu", "kaeru" };

    private string[] _question1 = { "帰る", "帰る", "まだ倒す" };
    private string[] _answer1 = { "kaeru", "kaeru", "madataosu" };

    private string _qString;
    private string _aString;

    private string _qString1;
    private string _aString1;

    // 何番目の問題か選び出すint
    private int _qNum;

    // 問題の何文字目か
    private int _aNum;
    private int _aNum1;

    [SerializeField] Text talkText;
    [SerializeField] Text enterText;

    int storyProgress;

    AudioSource audioSource;
    public AudioClip enterSound;
    public AudioClip typingSound;
    public AudioClip powerUpSound;

    SpriteRenderer zizou1;
    SpriteRenderer zizouStone;
    SpriteRenderer zizouStone1;
    SpriteRenderer Blind;

    public Animator animator;

    void Start()
    {
        ChangeQ();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //story進行
        if (Input.GetKeyDown(KeyCode.Return))
        {
            audioSource.PlayOneShot(enterSound);
            if (storyProgress == 3)
            {
                storyProgress = 4;
                talkText.text = "君はこれからどうしたい？";
                enterText.text = "";

                GameObject director = GameObject.Find("GameDirector");
                director.GetComponent<GameDirector>().QuestionAppear();
                qText.color = new Color32(255, 255, 255, 255);
                qText1.color = new Color32(255, 255, 255, 255);
                aText.color = new Color32(255, 255, 255, 255);
                aText1.color = new Color32(255, 255, 255, 255);
            }
            if (storyProgress == 2)
            {
                storyProgress = 3;
                talkText.text = "まさか夜を乗り切るなんてなぁ...";
            }
            if (storyProgress == 1)
            {
                storyProgress = 100;

                GameObject zizou = GameObject.Find("zizou");
                zizou.GetComponent<ZizouAppear>().StartCoroutine("ZizouZoom");

                StartCoroutine(TalkDelay());
            }
            if (storyProgress == 0)
            {
                storyProgress = 1;
                talkText.text = "(あの地蔵はどこへ行ったんだ...)";
            }
            if (storyProgress == 5)
            {
                storyProgress = 7;

                enterText.text = "";

                GameObject director = GameObject.Find("GameDirector");
                director.GetComponent<GameDirector>().QuestionAppear();
                qText.color = new Color32(255, 255, 255, 255);
                qText1.color = new Color32(255, 255, 255, 255);
                aText.color = new Color32(255, 255, 255, 255);
                aText1.color = new Color32(255, 255, 255, 255);

                _qNum = 1;
                ChangeQ();
            }
            if (storyProgress == 6)
            {
                storyProgress = 8;

                enterText.text = "";

                GameObject director = GameObject.Find("GameDirector");
                director.GetComponent<GameDirector>().QuestionAppear();
                qText.color = new Color32(255, 255, 255, 255);
                qText1.color = new Color32(255, 255, 255, 255);
                aText.color = new Color32(255, 255, 255, 255);
                aText1.color = new Color32(255, 255, 255, 255);

                _qNum = 2;
                ChangeQ();
            }
            if (storyProgress == 12)
            {
                storyProgress = 100;
                StartCoroutine(GameChange());
            }
            if (storyProgress == 11)
            {
                storyProgress = 100;
                StartCoroutine(DirectZizou());
            }
            if (storyProgress == 10)
            {
                storyProgress = 11;
                talkText.text = "(どういうことだ...？)";
            }
            if (storyProgress == 9)
            {
                storyProgress = 10;
                talkText.text = "ならば私が君を止めるしか無いね！";
            }
        }

        if (Input.GetKeyDown(_aString[_aNum].ToString()) && _aNum1 == 0 && (storyProgress == 4 || storyProgress == 7 || storyProgress == 8))
        {
            Correct();

            if (_aNum >= _aString.Length)
            {
                Story1();
                
                if (storyProgress == 4)
                {
                    storyProgress = 5;
                    talkText.text = "本当にまだ妖怪を倒すのかい？";
                }
                if (storyProgress == 7)
                {
                    storyProgress = 9;
                    talkText.text = "そうか、君はまだ妖怪を倒す気なのか。";
                }
                if (storyProgress == 8)
                {
                    storyProgress = 9;
                    talkText.text = "そうか、君は帰ってしまうのか。";
                }
            }
        }
        if (Input.GetKeyDown(_aString1[_aNum1].ToString()) && _aNum == 0 && (storyProgress == 4 || storyProgress == 7 || storyProgress == 8))
        {
            Correct1();

            if (_aNum1 >= _aString1.Length)
            {
                Story1();

                if (storyProgress == 4)
                {
                    storyProgress = 6;
                    talkText.text = "本当に帰ってしまうのかい？";
                }
                if (storyProgress == 7)
                {
                    storyProgress = 9;
                    talkText.text = "そうか、君は帰ってしまうのか。";
                }
                if (storyProgress == 8)
                {
                    storyProgress = 9;
                    talkText.text = "そうか、君はまだ妖怪を倒す気なのか。";
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && (storyProgress == 4 || storyProgress == 7 || storyProgress == 8))
        {
            _aNum = 0;
            _aNum1 = 0;
            aText.text = "<color=#FFFFFF>" + _aString + "</color>";
            aText1.text = "<color=#FFFFFF>" + _aString1 + "</color>";
        }
    }


    IEnumerator TalkDelay()
    {
        yield return new WaitForSeconds(1.3f);
        talkText.text = "(昨日の地蔵だ...)";
        storyProgress = 2;
    }

    IEnumerator DirectZizou()
    {
        audioSource.PlayOneShot(powerUpSound);
        animator.SetTrigger("AngerTrigger");
        zizouStone = GameObject.Find("zizoustone").GetComponent<SpriteRenderer>();
        zizouStone.color = new Color32(255, 0, 0, 255);

        for (int i = 0; i < 120; i++)
        {
            zizouStone1 = GameObject.Find("zizoustone1").GetComponent<SpriteRenderer>();
            zizouStone1.color = new Color32(255, 0, 0, 150);
            yield return new WaitForSeconds(0.01f);
            zizouStone1.color = new Color32(255, 0, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.3f);
        talkText.text = "ふふっ、生きて帰れるかな！";
        storyProgress = 12;
    }

    IEnumerator GameChange()
    {
        zizou1 = GameObject.Find("zizou1").GetComponent<SpriteRenderer>();
        zizou1.color = new Color32(255, 255, 255, 255);
        Destroy(GameObject.Find("zizou"));
        Destroy(GameObject.Find("zizoustone"));

        GameObject zizou2 = GameObject.Find("zizou1");
        zizou2.GetComponent<ZizouAppear>().StartCoroutine("ZizouZooming");

        for (int i = 0; i < 255; i++)
        {
            Blind = GameObject.Find("DeathBlind").GetComponent<SpriteRenderer>();
            Blind.color += new Color32(0, 0, 0, 1);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Stage4");
    }

    void ChangeQ()
    {
        _aNum = 0;
        _aNum1 = 0;

        _qString = _question[_qNum];
        _aString = _answer[_qNum];

        qText.text = _qString;
        aText.text = _aString;

        _qString1 = _question1[_qNum];
        _aString1 = _answer1[_qNum];

        qText1.text = _qString1;
        aText1.text = _aString1;
    }

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
        enterText.text = "ENTER▼";

        audioSource.PlayOneShot(enterSound);
    }
}
