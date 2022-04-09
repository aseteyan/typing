using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewSelectQ4 : MonoBehaviour
{
    [SerializeField] Text qText; // 問題用のテキスト
    [SerializeField] Text aText; // 解答用のテキスト

    [SerializeField] Text qText1; // 問題用のテキスト
    [SerializeField] Text aText1; // 解答用のテキスト

    private string[] _question = { "車", "曲", "氷", "金", "川", "神", "竹", "弓", "山", "円", "闇", "今", "凶", "玉", "轟", "籠", "肉", "妃", "芸", "厄", "妹", "上" };
    private string[] _answer = { "kuruma", "kyoku", "koori", "kinn", "kawa", "kami", "take", "yumi", "yama", "enn", "yami", "ima", "kyou", "tama", "todoroki", "kago", "niku", "kisaki", "gei", "yaku", "imouto", "ue" };

    private string[] _question1 = { "愛", "兄", "男", "窓", "耳", "右", "寸", "丸", "女", "千", "水", "公", "夫", "升", "左", "赤", "弟", "丼", "汗", "肌", "仏", "皿" };
    private string[] _answer1 = { "ai", "ani", "otoko", "mado", "mimi", "migi", "sunn", "maru", "onnna", "senn", "mizu", "ooyake", "otto", "masu", "hidari", "aka", "otouto", "donn", "ase", "hada", "hotoke", "sara" };

    private string _qString;
    private string _aString;

    private string _qString1;
    private string _aString1;

    // 何番目の問題か選び出すint
    private int _qNum;
    private int _qNum1;

    int beforeQ;
    int beforeQ1;

    // 問題の何文字目か
    private int _aNum;
    private int _aNum1;

    [SerializeField] Text talkText;
    [SerializeField] Text enterText;

    int storyProgress;
    public bool stopInput;

    AudioSource audioSource;
    public AudioClip enterSound;
    public AudioClip typingSound;
    public AudioClip missSound;
    public AudioClip killSound;
    public AudioClip damageSound;
    public AudioClip BGM;
    public AudioClip BGM1;

    public Image frame;
    public SpriteRenderer panel;
    public SpriteRenderer zizou;
    public SpriteRenderer hitodama1;
    public SpriteRenderer hitodama2;
    public SpriteRenderer sceneBlind;
    public SpriteRenderer forestScene;
    public SpriteRenderer forestScene1;
    public SpriteRenderer forestScene2;
    public SpriteRenderer forestScene3;
    public SpriteRenderer forestScene4;
    public SpriteRenderer forestScene5;
    public SpriteRenderer forestScene6;
    public SpriteRenderer forestScene7;

    public Slider slider;
    public Image sliderImage;
    public Image sliderBack;

    void Start()
    {
        ChangeQ();
        ChangeQ1();
        audioSource = GetComponent<AudioSource>();
        stopInput = false;
        StartCoroutine(StartDelay());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && storyProgress >= 60)
        {
            audioSource.PlayOneShot(enterSound);
            if (storyProgress == 63)
            {
                storyProgress = 100;
                StartCoroutine(Recollection());
            }
            if (storyProgress == 62)
            {
                storyProgress = 63;
                talkText.text = "はぁ...君は前にあんなことをしたのに...";
            }
            if (storyProgress == 61)
            {
                storyProgress = 62;
                talkText.text = "(何のことだ...)";
            }
            if (storyProgress == 60)
            {
                storyProgress = 61;
                talkText.text = "君は覚えていないのかい？";
            }
        }

        if (Input.GetKeyDown(_aString[_aNum].ToString()) && _aNum1 == 0 && stopInput)
        {
            Correct();

            if (_aNum >= _aString.Length)
            {
                StartCoroutine(DamageHitodama1());
                StartCoroutine(WaitChangeQ1());
            }
        }
        else if (Input.anyKeyDown && _aNum != 0 && stopInput)
        {
            Miss();
            GameObject limitFrame1 = GameObject.Find("flame1");
            limitFrame1.GetComponent<DirectingTime4>().MissDamage();
        }
        if (Input.GetKeyDown(_aString1[_aNum1].ToString()) && _aNum == 0 && stopInput)
        {
            Correct1();

            if (_aNum1 >= _aString1.Length)
            {
                StartCoroutine(DamageHitodama2());
                StartCoroutine(WaitChangeQ2());
            }
        }
        else if (Input.anyKeyDown && _aNum1 != 0 && stopInput)
        {
            Miss1();
            GameObject limitFrame2 = GameObject.Find("flame2");
            limitFrame2.GetComponent<DirectingTime4>().MissDamage();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _aNum = 0;
            _aNum1 = 0;
            aText.text = "<color=#FFFFFF>" + _aString + "</color>";
            aText1.text = "<color=#FFFFFF>" + _aString1 + "</color>";
        }
    }

    IEnumerator Recollection()
    {
        Destroy(GameObject.Find("zizou"));
        frame.color = new Color32(255, 255, 255, 0);
        panel.color = new Color32(0, 0, 0, 0);
        talkText.text = "";
        enterText.text = "";

        for (int i = 0; i < 255; i++)
        {
            sceneBlind.color += new Color32(0, 0, 0, 1);
            yield return new WaitForSeconds(0.01f);
        }
        audioSource.Stop();
        audioSource.PlayOneShot(BGM1);
        for (int i = 0; i < 255; i++)
        {
            forestScene.color += new Color32(0, 0, 0, 1);
            forestScene1.color += new Color32(0, 0, 0, 1);
            forestScene2.color += new Color32(0, 0, 0, 1);
            forestScene3.color += new Color32(0, 0, 0, 1);
            forestScene4.color += new Color32(0, 0, 0, 1);
            forestScene5.color += new Color32(0, 0, 0, 1);
            forestScene6.color += new Color32(0, 0, 0, 1);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.5f);
        GameObject stone = GameObject.Find("stone");
        stone.GetComponent<ZizouAppear>().StartCoroutine("Stone");
        yield return new WaitForSeconds(6f);
        for (int i = 0; i < 255; i++)
        {
            forestScene.color -= new Color32(0, 0, 0, 1);
            forestScene1.color -= new Color32(0, 0, 0, 1);
            forestScene2.color -= new Color32(0, 0, 0, 1);
            forestScene3.color -= new Color32(0, 0, 0, 1);
            forestScene4.color -= new Color32(0, 0, 0, 1);
            forestScene6.color -= new Color32(0, 0, 0, 1);
            forestScene7.color -= new Color32(0, 0, 0, 1);
            yield return new WaitForSeconds(0.01f);
        }
        SceneManager.LoadScene("Stage5");
    }

    IEnumerator DamageHitodama1()
    {
        hitodama1.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(0.1f);
        hitodama1.color = new Color32(255, 255, 255, 255);
        yield return new WaitForSeconds(0.1f);
        hitodama1.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(0.1f);
        hitodama1.color = new Color32(255, 255, 255, 255);
    }
    IEnumerator DamageHitodama2()
    {
        hitodama2.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(0.1f);
        hitodama2.color = new Color32(255, 255, 255, 255);
        yield return new WaitForSeconds(0.1f);
        hitodama2.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(0.1f);
        hitodama2.color = new Color32(255, 255, 255, 255);
    }
    void SliderManager()
    {
        slider.value -= 1f;
        if (slider.value == 15)
        {
            sliderImage.color = new Color32(255, 255, 0, 255);
        }
        else if (slider.value == 7)
        {
            sliderImage.color = new Color32(255, 0, 0, 255);
        }
    }

    public IEnumerator WaitChangeQ1()
    {
        audioSource.PlayOneShot(killSound);

        qText.text = "";
        aText.text = "";
        GameObject limitFrame1 = GameObject.Find("flame1");
        limitFrame1.GetComponent<DirectingTime4>().UndoFrame();
        GameObject director = GameObject.Find("GameDirector");
        director.GetComponent<GameDirector>().QuestionDisappear4();
        if (storyProgress < 49)
        {
            SliderManager();

            limitFrame1.GetComponent<DirectingTime4>().isStop = true;
            director.GetComponent<GameDirector>().QuestionAppear4();
            yield return new WaitForSeconds(0.001f);
            ChangeQ();
            storyProgress++;
        }
        if (storyProgress >= 49)
        {
            StartCoroutine(NextStory());
        }
    }

    public IEnumerator WaitChangeQ2()
    {
        audioSource.PlayOneShot(killSound);

        qText1.text = "";
        aText1.text = "";
        GameObject limitFrame2 = GameObject.Find("flame2");
        limitFrame2.GetComponent<DirectingTime4>().UndoFrame();
        GameObject director = GameObject.Find("GameDirector");
        director.GetComponent<GameDirector>().QuestionDisappear41();
        if (storyProgress < 49)
        {
            SliderManager();

            limitFrame2.GetComponent<DirectingTime4>().isStop = true;
            director.GetComponent<GameDirector>().QuestionAppear41();
            yield return new WaitForSeconds(0.001f);
            ChangeQ1();
            storyProgress++;
        }
        if (storyProgress >= 49)
        {
            StartCoroutine(NextStory());
        }
    }

    public void DamageChangeQ1()
    {
        audioSource.PlayOneShot(damageSound);

        qText.text = "";
        aText.text = "";
        GameObject limitFrame1 = GameObject.Find("flame1");
        limitFrame1.GetComponent<DirectingTime4>().UndoFrame();
        GameObject director = GameObject.Find("GameDirector");
        director.GetComponent<GameDirector>().QuestionDisappear4();
        if (storyProgress < 59)
        {
            limitFrame1.GetComponent<DirectingTime4>().isStop = true;
            director.GetComponent<GameDirector>().QuestionAppear4();
            ChangeQ();
        }
    }

    public void DamageChangeQ2()
    {
        audioSource.PlayOneShot(damageSound);

        qText1.text = "";
        aText1.text = "";
        GameObject limitFrame2 = GameObject.Find("flame2");
        limitFrame2.GetComponent<DirectingTime4>().UndoFrame();
        GameObject director = GameObject.Find("GameDirector");
        director.GetComponent<GameDirector>().QuestionDisappear41();
        if (storyProgress < 59)
        {
            limitFrame2.GetComponent<DirectingTime4>().isStop = true;
            director.GetComponent<GameDirector>().QuestionAppear41();
            ChangeQ1();
        }
    }

    IEnumerator StartDelay()
    {
        GameObject limitFrame1 = GameObject.Find("flame1");
        GameObject limitFrame2 = GameObject.Find("flame2");
        limitFrame1.GetComponent<DirectingTime4>().UndoFrame();
        limitFrame2.GetComponent<DirectingTime4>().UndoFrame();

        yield return new WaitForSeconds(3f);

        Destroy(GameObject.Find("Talk_Text1"));
        Destroy(GameObject.Find("LimitLine"));
        Destroy(GameObject.Find("LimitLineBackGround"));
        Destroy(GameObject.Find("OutputLocation"));

        yield return new WaitForSeconds(0.1f);

        limitFrame1.GetComponent<DirectingTime4>().isStop = true;
        limitFrame2.GetComponent<DirectingTime4>().isStop = true;
        
        GameObject director = GameObject.Find("GameDirector");
        director.GetComponent<GameDirector>().QuestionAppear4();
        director.GetComponent<GameDirector>().QuestionAppear41();

        qText.color = new Color32(255, 255, 255, 255);
        qText1.color = new Color32(255, 255, 255, 255);
        aText.color = new Color32(255, 255, 255, 255);
        aText1.color = new Color32(255, 255, 255, 255);

        stopInput = true;
    }

    void ChangeQ()
    {
        _aNum = 0;
        _aNum1 = 0;

        while (beforeQ == _qNum)
        {
            _qNum = Random.Range(0, _question.Length);
        }
        beforeQ = _qNum;
        
        _qString = _question[_qNum];
        _aString = _answer[_qNum];

        qText.text = _qString;
        aText.text = _aString;
    }
    void ChangeQ1()
    {
        _aNum = 0;
        _aNum1 = 0;

        while (beforeQ1 == _qNum1)
        {
            _qNum1 = Random.Range(0, _question1.Length);
        }
        beforeQ1 = _qNum1;

        _qString1 = _question1[_qNum1];
        _aString1 = _answer1[_qNum1];

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

    void Miss()
    {
        // 不正解の時の処理
        aText.text = "<color=#6A6A6A>" + _aString.Substring(0, _aNum) + "</color>"
            + "<color=#FF0000>" + _aString.Substring(_aNum, 1) + "</color>"
            + _aString.Substring(_aNum + 1);

        audioSource.PlayOneShot(missSound);
    }

    void Miss1()
    {
        // 不正解の時の処理
        aText1.text = "<color=#6A6A6A>" + _aString1.Substring(0, _aNum1) + "</color>"
            + "<color=#FF0000>" + _aString1.Substring(_aNum1, 1) + "</color>"
            + _aString1.Substring(_aNum1 + 1);

        audioSource.PlayOneShot(missSound);
    }

    IEnumerator NextStory()
    {
        stopInput = false;

        GameObject director = GameObject.Find("GameDirector");
        director.GetComponent<GameDirector>().QuestionDisappear4();
        director.GetComponent<GameDirector>().QuestionDisappear41();
        Destroy(GameObject.Find("flame1"));
        Destroy(GameObject.Find("flame2"));
        qText.text = "";
        aText.text = "";
        qText1.text = "";
        aText1.text = "";

        for (int i = 0; i < 10; i++)
        {
            zizou.color = new Color32(255, 0, 0, 0);
            hitodama1.color = new Color32(255, 0, 0, 0);
            hitodama2.color = new Color32(255, 0, 0, 0);
            yield return new WaitForSeconds(0.1f);
            zizou.color = new Color32(255, 0, 0, 255);
            hitodama1.color = new Color32(255, 0, 0, 255);
            hitodama2.color = new Color32(255, 0, 0, 255);
            yield return new WaitForSeconds(0.1f);

            audioSource.PlayOneShot(damageSound);
        }

        sliderImage.color = new Color32(0, 0, 0, 0);
        sliderBack.color = new Color32(0, 0, 0, 0);

        audioSource.Stop();
        audioSource.PlayOneShot(BGM);

        Destroy(GameObject.Find("Life1"));
        Destroy(GameObject.Find("Life2"));
        Destroy(GameObject.Find("Life3"));
        Destroy(hitodama1);
        Destroy(hitodama2);
        frame.color = new Color32(255, 255, 255, 255);
        panel.color = new Color32(0, 0, 0, 220);
        zizou.color = new Color32(255, 255, 255, 255);
        talkText.text = "ぐっ、なかなかやるね...";
        enterText.text = "ENTER▼";

        storyProgress = 60;
    }
}
