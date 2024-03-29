using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField] Text qText; // 問題用のテキスト
    [SerializeField] Text aText; // 解答用のテキスト

    [SerializeField] Text qText1; // 問題用のテキスト
    [SerializeField] Text aText1; // 解答用のテキスト

    // 最初の文字が右手から始まるもので揃える
    private readonly string[] _question = { "芋", "糸", "犬", "稲", "嫌", "色", "岩", "兎", "裏", "夫", "表", "女", "男", "帯", "親", "肉", "庭", "何", "謎", "沼", "猫", "右", "耳", "窓", "孫", "水", "桃", "仏", "肌", "左", "暇", "姫", "蛇", "蛍", "車", "曲", "氷", "川", "神", "凶", "肝", "獣", "弓", "山", "厄", "闇", "夜", "宿", "夢" };
    private readonly string[] _answer = { "imo", "ito", "inu", "ine", "iya", "iro", "iwa", "usagi", "ura", "otto", "omote", "onnna", "otoko", "obi", "oya", "niku", "niwa", "nani", "nazo", "numa", "neko", "migi", "mimi", "mado", "mago", "mizu", "momo", "hotoke", "hada", "hidari", "hima", "hime", "hebi", "hotaru", "kuruma", "kyoku", "koori", "kawa", "kami", "kyou", "kimo", "kemono", "yumi", "yama", "yaku", "yami", "yoru", "yado", "yume" };
    // 最初の文字が左手から始まるもので揃える
    private readonly string[] _question1 = { "茜", "兄", "汗", "泡", "頭", "油", "顎", "飴", "駅", "枝", "餌", "蝦", "皿", "里", "酒", "砂", "墨", "杉", "鈴", "外", "袖", "誰", "芸", "軍", "劇", "玉", "旅", "敵", "寺", "虎", "扉", "楽", "量", "龍", "六", "歴", "略", "例", "技", "脇", "枠", "財", "属", "象", "倍", "秒", "豚", "僕", "棒" };
    private readonly string[] _answer1 = { "akane", "ani", "ase", "awa", "atama", "abura", "ago", "ame", "eki", "eda", "esa", "ebi", "sara", "sato", "sake", "suna", "sumi", "sugi", "suzu", "soto", "sode", "dare", "gei", "gunn", "geki", "tama", "tabi", "teki", "tera", "tora", "tobira", "raku", "ryou", "ryuu", "roku", "reki", "ryaku", "rei", "waza", "waki", "waku", "zai", "zoku", "zou", "bai", "byou", "buta", "boku", "bou" };

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

    [SerializeField] Text lvText;
    int powerup;
    int powerLV;
    public bool stopInput;
    bool isStart;

    AudioSource audioSource;
    public AudioClip typingSound;
    public AudioClip missSound;
    public AudioClip killSound;
    public AudioClip damageSound;
    public AudioClip powerUP;
    public AudioClip startSound;

    public SpriteRenderer zizou;
    public SpriteRenderer hitodama1;
    public SpriteRenderer hitodama2;

    public Slider slider;
    public Image sliderImage;
    public Image sliderBack;

    public float average;
    float scoreNum;
    float time;
    bool isTime;

    void Start()
    {
        ChangeQ();
        ChangeQ1();
        audioSource = GetComponent<AudioSource>();
        stopInput = false;
        isStart = true;
        isTime = false;

        GameObject count = GameObject.Find("ScoreManager");
        count.GetComponent<ScoreManager>().SetScore();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isStart)
        {
            isStart = false;
            StartCoroutine(StartDelay());
        }

        if (isTime)
        {
            time += Time.deltaTime;
            average = scoreNum / time;
        }

        if (Input.GetKeyDown(_aString[_aNum].ToString()) && _aNum1 == 0 && stopInput)
        {
            Correct();
            scoreNum++;

            if (_aNum >= _aString.Length)
            {
                StartCoroutine(DamageHitodama1());
                StartCoroutine(WaitChangeQ1());

                GameObject count = GameObject.Find("ScoreManager");
                count.GetComponent<ScoreManager>().ScoreCount();
            }
        }
        else if (Input.anyKeyDown && _aNum != 0 && stopInput)
        {
            Miss();
            GameObject limitFrame1 = GameObject.Find("flame1");
            limitFrame1.GetComponent<MiniGameFrame>().MissDamage();
        }
        if (Input.GetKeyDown(_aString1[_aNum1].ToString()) && _aNum == 0 && stopInput)
        {
            Correct1();
            scoreNum++;

            if (_aNum1 >= _aString1.Length)
            {
                StartCoroutine(DamageHitodama2());
                StartCoroutine(WaitChangeQ2());

                GameObject count = GameObject.Find("ScoreManager");
                count.GetComponent<ScoreManager>().ScoreCount();
            }
        }
        else if (Input.anyKeyDown && _aNum1 != 0 && stopInput)
        {
            Miss1();
            GameObject limitFrame2 = GameObject.Find("flame2");
            limitFrame2.GetComponent<MiniGameFrame>().MissDamage();
        }
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
        if (slider.value == 0)
        {
            slider.value = 10f;
            sliderImage.color = new Color32(0, 255, 0, 255);
        }
        if (slider.value == 4)
        {
            sliderImage.color = new Color32(255, 255, 0, 255);
        }
        if (slider.value == 2)
        {
            sliderImage.color = new Color32(255, 0, 0, 255);
        }
    }

    public IEnumerator WaitChangeQ1()
    {
        powerup++;
        if (powerup % 10 == 0)
        {
            audioSource.PlayOneShot(powerUP);
            GameObject frame1 = GameObject.Find("flame1");
            GameObject frame2 = GameObject.Find("flame2");
            frame1.GetComponent<MiniGameFrame>().fillSpeed += 0.0005f;
            frame2.GetComponent<MiniGameFrame>().fillSpeed += 0.0005f;

            powerLV++;
            lvText.text = "Lv." + powerLV;
        }

        audioSource.PlayOneShot(killSound);

        qText.text = "";
        aText.text = "";
        GameObject limitFrame1 = GameObject.Find("flame1");
        limitFrame1.GetComponent<MiniGameFrame>().UndoFrame();
        GameObject director = GameObject.Find("GameDirector");
        director.GetComponent<GameDirector>().QuestionDisappear4();

        SliderManager();

        limitFrame1.GetComponent<MiniGameFrame>().isStop = true;
        director.GetComponent<GameDirector>().QuestionAppear4();
        yield return new WaitForSeconds(0.001f);
        ChangeQ();
    }

    public IEnumerator WaitChangeQ2()
    {
        powerup++;
        if (powerup % 10 == 0)
        {
            audioSource.PlayOneShot(powerUP);
            GameObject frame1 = GameObject.Find("flame1");
            GameObject frame2 = GameObject.Find("flame2");
            frame1.GetComponent<MiniGameFrame>().fillSpeed += 0.0005f;
            frame2.GetComponent<MiniGameFrame>().fillSpeed += 0.0005f;

            powerLV++;
            lvText.text = "Lv." + powerLV;
        }

        audioSource.PlayOneShot(killSound);

        qText1.text = "";
        aText1.text = "";
        GameObject limitFrame2 = GameObject.Find("flame2");
        limitFrame2.GetComponent<MiniGameFrame>().UndoFrame();
        GameObject director = GameObject.Find("GameDirector");
        director.GetComponent<GameDirector>().QuestionDisappear41();
        
        SliderManager();

        limitFrame2.GetComponent<MiniGameFrame>().isStop = true;
        director.GetComponent<GameDirector>().QuestionAppear41();
        yield return new WaitForSeconds(0.001f);
        ChangeQ1();
    }

    public void DamageChangeQ1()
    {
        audioSource.PlayOneShot(damageSound);

        qText.text = "";
        aText.text = "";
        GameObject limitFrame1 = GameObject.Find("flame1");
        limitFrame1.GetComponent<MiniGameFrame>().UndoFrame();
        GameObject director = GameObject.Find("GameDirector");
        director.GetComponent<GameDirector>().QuestionDisappear4();

        limitFrame1.GetComponent<MiniGameFrame>().isStop = true;
        director.GetComponent<GameDirector>().QuestionAppear4();
        ChangeQ();
    }

    public void DamageChangeQ2()
    {
        audioSource.PlayOneShot(damageSound);

        qText1.text = "";
        aText1.text = "";
        GameObject limitFrame2 = GameObject.Find("flame2");
        limitFrame2.GetComponent<MiniGameFrame>().UndoFrame();
        GameObject director = GameObject.Find("GameDirector");
        director.GetComponent<GameDirector>().QuestionDisappear41();

        limitFrame2.GetComponent<MiniGameFrame>().isStop = true;
        director.GetComponent<GameDirector>().QuestionAppear41();
        ChangeQ1();
    }

    IEnumerator StartDelay()
    {
        audioSource.PlayOneShot(startSound);

        GameObject limitFrame1 = GameObject.Find("flame1");
        GameObject limitFrame2 = GameObject.Find("flame2");
        limitFrame1.GetComponent<MiniGameFrame>().UndoFrame();
        limitFrame2.GetComponent<MiniGameFrame>().UndoFrame();

        Destroy(GameObject.Find("Talk_Text1"));
        Destroy(GameObject.Find("Talk_Text2"));
        Destroy(GameObject.Find("LimitLine"));
        Destroy(GameObject.Find("LimitLineBackGround"));
        Destroy(GameObject.Find("OutputLocation"));

        sliderImage.color = new Color32(0, 255, 0, 255);
        sliderBack.color = new Color32(170, 170, 170, 255);

        powerLV = 1;
        lvText.text = "Lv." + powerLV;

        yield return new WaitForSeconds(3.1f);

        limitFrame1.GetComponent<MiniGameFrame>().isStop = true;
        limitFrame2.GetComponent<MiniGameFrame>().isStop = true;
        
        GameObject director = GameObject.Find("GameDirector");
        director.GetComponent<GameDirector>().QuestionAppear4();
        director.GetComponent<GameDirector>().QuestionAppear41();

        qText.color = new Color32(255, 255, 255, 255);
        qText1.color = new Color32(255, 255, 255, 255);
        aText.color = new Color32(255, 255, 255, 255);
        aText1.color = new Color32(255, 255, 255, 255);

        stopInput = true;
        isTime = true;
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
}
