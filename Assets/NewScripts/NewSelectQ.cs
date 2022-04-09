using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewSelectQ : MonoBehaviour
{
    // ��ʏ�̃e�L�X�g�������Ă���
    [SerializeField] Text qText; // ���p�̃e�L�X�g
    [SerializeField] Text aText; // �𓚗p�̃e�L�X�g

    [SerializeField] Text qText1; // ���p�̃e�L�X�g
    [SerializeField] Text aText1; // �𓚗p�̃e�L�X�g

    // ����p�ӂ��Ă���
    private string[] _question = { "������", "������" };
    private string[] _answer = { "iiyo", "iine" };

    private string[] _question1 = { "�₾", "�₾��" };
    private string[] _answer1 = { "yada", "yadane" };

    // ���Ԗڂ��w�肷�邽�߂�string
    private string _qString;
    private string _aString;

    private string _qString1;
    private string _aString1;

    // ���Ԗڂ̖�肩�I�яo��int
    private int _qNum;

    // ���̉������ڂ�
    private int _aNum;
    private int _aNum1;


    // story�i�s�x�ۑ��p��int
    int storyProgress;

    [SerializeField] Text talkText;
    [SerializeField] Text enterText;
    string enter = "ENTER��";
    public Image frame;
    public SpriteRenderer output;

    AudioSource audioSource;
    public AudioClip deathSound;
    public AudioClip typingSound;
    public AudioClip enterSound;

    public SpriteRenderer background;
    SpriteRenderer deathBlind;

    public Animator animator;

    // �Q�[���J�n���Ɉ�x�������s��������
    void Start()
    {
        ChangeQ();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //story�i�s
        if (Input.GetKeyDown(KeyCode.Return))
        {
            audioSource.PlayOneShot(enterSound);

            if (storyProgress == 0)
            {
                talkText.text = "�|���Ă���邩���H";
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
                storyProgress = 100; // ���x�����͂ł��Ȃ��悤storyProgress��K����100�ɂ���
                StartCoroutine(GameOver());
            }
            if (storyProgress == 4)
            {
                storyProgress = 100;
                StartCoroutine(GameClear());
            }
            if (storyProgress == 2)
            {
                talkText.text = "�����A�d���ގ��ցI";
                storyProgress = 4;
            }
            if (storyProgress == 7)
            {
                storyProgress = 100;
                StartCoroutine(GameChange());
            }
            if (storyProgress == 6)
            {
                talkText.text = "(�Ƃ肠�����d���Ƃ���|����...)";
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

                talkText.text = "���肪�Ƃ��I";
            }
        }
        if (Input.GetKeyDown(_aString1[_aNum1].ToString()) && _aNum == 0 && storyProgress == 1)
        {
            Correct1();
            if (_aNum1 >= _aString1.Length)
            {
                Story1();
                storyProgress = 3;

                talkText.text = "�N�ɂ͎��]������...";
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

    // ����p�̊֐�
    void Correct()
    {
        //�����������̏���
        _aNum++;

        audioSource.PlayOneShot(typingSound);

        aText.text = "<color=#6A6A6A>" + _aString.Substring(0, _aNum) + "</color>" + _aString.Substring(_aNum);
    }

    void Correct1()
    {
        //�����������̏���
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
        talkText.text = "鳖��鲂�������绂����ɂȂ����ˁI\r\n�����|���Ă������I";
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
        talkText.text = "(�s���Ă��܂���...)";
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
