using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class Stage5 : MonoBehaviour
{
    [SerializeField] Text fText;
    [SerializeField] Text qText;
    [SerializeField] Text aText;

    // 問題を用意しておく
    private string[] _furigana = { "さいごまであそんでくれてありがとう", "たしかにゆるされないことをした", "こんなことになるとはおもわなかった", "どんなにいたかっただろうか", "あやまってもあやまりきれないよ", "なぜいしをなげるなんてことをしたんだ", "きみはわたしをうらんでいる", "あのころのわたしはまだこどもだった", "わたしのいしきがひくかったんだ", "ぜったいににどとしないとちかう", "こころからすまなかったとおもっている", "なぜあんなことをしてしまったのか", "あのときはどうかしていた", "ほんとうにもうしわけない" };
    private string[] _question = { "最後まで遊んでくれてありがとう", "確かに許されないことをした", "こんな事になるとは思わなかった", "どんなに痛かっただろうか", "謝っても謝りきれないよ", "なぜ石を投げるなんてことをしたんだ", "君は私を恨んでいる", "あの頃の私はまだ子供だった", "私の意識が低かったんだ", "絶対に二度としないと誓う", "心からすまなかったと思っている", "何故あんなことをしてしまったのか", "あの時はどうかしていた", "本当に申し訳ない" };

    // 何番目か指定するためのstring
    private string _fString;
    private string _qString;
    private string _aString;

    // 何番目の問題か選び出すint
    private int _qNum;

    // 2回続けて同じ問題が出ないようにする保存用
    private int beforeRand;

    // 問題の何文字目か
    private int _aNum;

    // 合ってるかどうかの判断
    bool isCorrect;

    // 入力可否
    public bool isInput;

    // 辞書の宣言
    private ChangeDictionary cd;
    private List<string> _romSliceList = new List<string>();
    // 何文字目のふりがなを入力しているか
    private List<int> _fCountList = new List<int>();
    private List<int> _romNumList = new List<int>();

    public Text talkText;
    public Text enterText;

    public Text titleText;
    public Text titleText1;
    public Text explanationText;

    AudioSource audioSource;
    public AudioClip enterSound;
    public AudioClip typingSound;
    public AudioClip missSound;
    public AudioClip killSound;
    public AudioClip damageSound;
    public AudioClip amazingSound;

    int storyProgress;

    public SpriteRenderer zizou;
    public SpriteRenderer cLine;
    public Image frame;
    public Image backFrame;
    public SpriteRenderer panel;
    public SpriteRenderer life1;
    public SpriteRenderer life2;
    public SpriteRenderer life3;
    public SpriteRenderer blind;
    public ParticleSystem ps;

    public Slider slider;
    public Image sliderImage;
    public Image sliderBack;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // 柔軟な入力のための辞書を読み込む
        cd = GetComponent<ChangeDictionary>();
        ChangeQ();

        isInput = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && storyProgress != 101)
        {
            audioSource.PlayOneShot(enterSound);

            if (storyProgress == 4)
            {
                storyProgress = 5;
                StartCoroutine(StartGame());
            }
            if (storyProgress == 3)
            {
                storyProgress = 100;
                StartCoroutine(ConcentrationLine());
            }
            if (storyProgress == 2)
            {
                storyProgress = 3;
                talkText.text = "あやまってすんだらねぇ！";
            }
            if (storyProgress == 1)
            {
                storyProgress = 2;
                talkText.text = "(す...すまなかった...)";
            }
            if (storyProgress == 0)
            {
                storyProgress = 1;
                talkText.text = "そうだ、君はあのとき石を投げてきた！";
            }
            if (storyProgress == 34)
            {
                storyProgress = 101;
                StartCoroutine(FinishGame());
            }
            if (storyProgress == 33)
            {
                storyProgress = 101;
                StartCoroutine(ZizouGoodBye());
            }
            if (storyProgress == 32)
            {
                storyProgress = 33;
                talkText.text = "(本当に申し訳なかった...)";
            }
            if (storyProgress == 31)
            {
                storyProgress = 32;
                talkText.text = "恨みが弱まって動けなくなりそうだ";
            }
            if (storyProgress == 30)
            {
                storyProgress = 31;
                talkText.text = "僕は石を投げられた恨みで動けるようになった";
            }
            if (storyProgress == 35)
            {
                storyProgress = 101;
                StartCoroutine(ChangeGame());
            }
        }

        // 入力されたときに判断する
        if (Input.anyKeyDown && isInput)
        {
            isCorrect = false;
            int fCount = _fCountList[_aNum];

            if (Input.GetKeyDown(_aString[_aNum].ToString()))
            {
                // 正しい入力
                isCorrect = true;
                Correct();

                // 最後の文字に正解した時の処理
                if (_aNum >= _aString.Length)
                {
                    // 問題を変更する
                    StartCoroutine(WaitChangeQ());
                    StartCoroutine(DamageZizou());
                }
            }
            else if (Input.GetKeyDown("n") && fCount > 0 && _romSliceList[fCount - 1] == "n")
            {
                // nnの対応
                _romSliceList[fCount - 1] = "nn";
                _aString = string.Join("", GetRomSliceListWithoutSkip());

                ReCreateList(_romSliceList);

                // 正しい入力
                isCorrect = true;
                Correct();

                // 最後の文字に正解した時の処理
                if (_aNum >= _aString.Length)
                {
                    // 問題を変更する
                    StartCoroutine(WaitChangeQ());
                    StartCoroutine(DamageZizou());
                }
            }
            else
            {
                // 他に正しい入力があるか
                string currentF = _fString[fCount].ToString();

                if (fCount < _fString.Length - 1)
                {
                    // 2文字を考慮した候補検索
                    string addNextMoji = _fString[fCount].ToString() + _fString[fCount + 1].ToString();
                    CheckIrregularType(addNextMoji, fCount, false);
                }

                if (!isCorrect)
                {
                    // 通常の候補検索
                    string moji = _fString[fCount].ToString();
                    CheckIrregularType(moji, fCount, true);
                }
            }

            // 不正解
            if (!isCorrect)
            {
                Miss();
                GameObject limitFrame = GameObject.Find("frame");
                limitFrame.GetComponent<LimitFrame5>().MissDamage();
            }
        }
    }

    IEnumerator FinishGame()
    {
        enterText.text = "";
        for (int i = 0; i < 255; i++)
        {
            blind.color += new Color32(0, 0, 0, 1);
            frame.color -= new Color32(0, 0, 0, 1);
            backFrame.color -= new Color32(0, 0, 0, 1);
            talkText.color -= new Color32(0, 0, 0, 1);
            panel.color -= new Color32(0, 0, 0, 1);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 255; i++)
        {
            titleText.color += new Color32(0, 0, 0, 1);
            titleText1.color += new Color32(0, 0, 0, 1);
            explanationText.color += new Color32(0, 0, 0, 1);
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(3f);
        explanationText.text = "効果音・BGM素材：ポケットサウンド";
        yield return new WaitForSeconds(3f);
        explanationText.text = "イラスト素材：いらすとや";
        yield return new WaitForSeconds(3f);
        explanationText.text = "製作者：石井(あせ)";
        yield return new WaitForSeconds(3f);
        explanationText.text = "タイトル画面で『８１５』と入力すると\r\nミニゲームが遊べます！";
        enterText.text = "ENTER▼";
        yield return new WaitForSeconds(1f);
        storyProgress = 35;
    }

    IEnumerator ChangeGame()
    {
        explanationText.text = "";
        enterText.text = "";

        for (int i = 0; i < 255; i++)
        {
            blind.color -= new Color32(0, 0, 0, 1);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("StartScreen");
    }

    IEnumerator StartGame()
    {
        talkText.text = "";
        enterText.text = "";
        cLine.color = new Color32(255, 255, 255, 0);
        GameObject limitFrame = GameObject.Find("frame");
        limitFrame.GetComponent<LimitFrame5>().UndoFrame();
        frame.color = new Color32(255, 255, 255, 0);
        backFrame.color = new Color32(90, 90, 90, 0);
        panel.color = new Color32(0, 0, 0, 0);

        sliderImage.color = new Color32(0, 255, 0, 255);
        sliderBack.color = new Color32(170, 170, 170, 255);

        yield return new WaitForSeconds(1f);

        life1.color = new Color32(255, 0, 0, 255);
        life2.color = new Color32(255, 0, 0, 255);
        life3.color = new Color32(255, 0, 0, 255);
        fText.color = new Color32(255, 255, 255, 255);
        qText.color = new Color32(255, 255, 255, 255);
        aText.color = new Color32(255, 255, 255, 255);
        limitFrame.GetComponent<LimitFrame5>().isStop = true;
        frame.color = new Color32(255, 255, 255, 255);
        backFrame.color = new Color32(90, 90, 90, 255);
        panel.color = new Color32(0, 0, 0, 230);

        isInput = true;
    }

    IEnumerator ZizouGoodBye()
    {
        talkText.text = "もう二度とあんなことするんじゃな..い...ぞ...";
        zizou.color = new Color32(0, 0, 0, 0);
        yield return new WaitForSeconds(3f);
        GameObject zizouScript = GameObject.Find("zizou1");
        zizouScript.GetComponent<ZizouAppear>().StartCoroutine("ZizouGoodbye");
        yield return new WaitForSeconds(3f);
        talkText.text = "(お地蔵様...)";
        storyProgress = 34;
    }

    IEnumerator FinishTyping()
    {
        fText.text = "";
        qText.text = "";
        aText.text = "";
        frame.color = new Color32(255, 255, 255, 0);
        backFrame.color = new Color32(90, 90, 90, 0);
        panel.color = new Color32(0, 0, 0, 0);
        Destroy(GameObject.Find("Life1"));
        Destroy(GameObject.Find("Life2"));
        Destroy(GameObject.Find("Life3"));
        Destroy(GameObject.Find("Slider"));

        GameObject director = GameObject.Find("GameDirector");
        director.GetComponent<GameDirector>().AudioStop();

        for (int i = 0; i < 20; i++)
        {
            zizou.color = new Color32(255, 0, 0, 255);
            yield return new WaitForSeconds(0.1f);
            zizou.color = new Color32(255, 255, 255, 255);
            yield return new WaitForSeconds(0.1f);

            audioSource.PlayOneShot(damageSound);
        }

        audioSource.Play();
        ParticleSystem.EmissionModule emission = ps.emission;
        emission.enabled = false;

        yield return new WaitForSeconds(1f);

        frame.color = new Color32(255, 255, 255, 255);
        backFrame.color = new Color32(90, 90, 90, 255);
        panel.color = new Color32(0, 0, 0, 230);
        talkText.text = "ぐっ！そんなに謝られるなんて...";
        enterText.text = "ENTER▼";

        storyProgress = 30;
    }

    IEnumerator ConcentrationLine()
    {
        audioSource.Stop();
        GameObject director = GameObject.Find("GameDirector");
        director.GetComponent<GameDirector>().Audio();
        audioSource.PlayOneShot(amazingSound);

        talkText.text = "警察はいらないんだよ！！！";
        cLine.color = new Color32(255, 255, 255, 255);
        yield return new WaitForSeconds(0.1f);
        cLine.color = new Color32(255, 255, 255, 0);
        yield return new WaitForSeconds(0.1f);
        cLine.color = new Color32(255, 255, 255, 255);
        yield return new WaitForSeconds(0.1f);
        cLine.color = new Color32(255, 255, 255, 0);
        yield return new WaitForSeconds(0.1f);
        cLine.color = new Color32(255, 255, 255, 255);
        yield return new WaitForSeconds(0.1f);
        cLine.color = new Color32(255, 255, 255, 0);
        yield return new WaitForSeconds(0.1f);
        cLine.color = new Color32(255, 255, 255, 255);

        storyProgress = 4;
    }

    void SliderManager()
    {
        slider.value -= 1f;
        if (slider.value == 5)
        {
            sliderImage.color = new Color32(255, 255, 0, 255);
        }
        if (slider.value == 2)
        {
            sliderImage.color = new Color32(255, 0, 0, 255);
        }
    }

    IEnumerator WaitChangeQ()
    {
        isInput = false;

        audioSource.PlayOneShot(killSound);

        SliderManager();

        fText.text = "";
        qText.text = "";
        aText.text = "";
        GameObject limitFrame = GameObject.Find("frame");
        limitFrame.GetComponent<LimitFrame5>().UndoFrame();
        frame.color = new Color32(255, 255, 255, 0);
        backFrame.color = new Color32(90, 90, 90, 0);
        panel.color = new Color32(0, 0, 0, 0);
        if (storyProgress < 19)
        {
            yield return new WaitForSeconds(1f);

            limitFrame.GetComponent<LimitFrame5>().isStop = true;
            frame.color = new Color32(255, 255, 255, 255);
            backFrame.color = new Color32(90, 90, 90, 255);
            panel.color = new Color32(0, 0, 0, 230);

            ChangeQ();
        }
        if (storyProgress >= 19)
        {
            StartCoroutine(FinishTyping());
        }
        storyProgress++;
    }

    public IEnumerator DamageChangeQ()
    {
        isInput = false;

        audioSource.PlayOneShot(damageSound);

        fText.text = "";
        qText.text = "";
        aText.text = "";
        GameObject limitFrame = GameObject.Find("frame");
        limitFrame.GetComponent<LimitFrame5>().UndoFrame();
        frame.color = new Color32(255, 255, 255, 0);
        backFrame.color = new Color32(90, 90, 90, 0);
        panel.color = new Color32(0, 0, 0, 0);
        if (storyProgress < 25)
        {
            yield return new WaitForSeconds(1f);

            limitFrame.GetComponent<LimitFrame5>().isStop = true;
            frame.color = new Color32(255, 255, 255, 255);
            backFrame.color = new Color32(90, 90, 90, 255);
            panel.color = new Color32(0, 0, 0, 230);
            ChangeQ();
        }
    }

    IEnumerator DamageZizou()
    {
        zizou.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(0.1f);
        zizou.color = new Color32(255, 255, 255, 255);
        yield return new WaitForSeconds(0.1f);
        zizou.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(0.1f);
        zizou.color = new Color32(255, 255, 255, 255);
        yield return new WaitForSeconds(0.1f);
        zizou.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(0.1f);
        zizou.color = new Color32(255, 255, 255, 255);
    }

    void CheckIrregularType(string currentF, int fCount, bool addSmallMoji)
    {
        if (cd.dictionary.ContainsKey(currentF))
        {
            List<string> stringList = cd.dictionary[currentF];

            // stringList
            for (int i = 0; i < stringList.Count; i++)
            {
                string rom = stringList[i];
                int romNum = _romNumList[_aNum];

                bool preCheck = true;

                for (int j = 0; j < romNum; j++)
                {
                    if (rom[j] != _romSliceList[fCount][j])
                    {
                        preCheck = false;
                    }
                }

                if (preCheck && Input.GetKeyDown(rom[romNum].ToString()))
                {
                    _romSliceList[fCount] = rom;
                    _aString = string.Join("", GetRomSliceListWithoutSkip());

                    ReCreateList(_romSliceList);

                    // 正しい入力
                    isCorrect = true;

                    if (addSmallMoji)
                    {
                        AddSmallMoji();
                    }

                    Correct();

                    // 最後の文字に正解した時の処理
                    if (_aNum >= _aString.Length)
                    {
                        // 問題を変更する
                        StartCoroutine(WaitChangeQ());
                        StartCoroutine(DamageZizou());
                    }
                    break;
                }
            }
        }
    }

    // 問題を出すための関数
    void ChangeQ()
    {
        // 0番目の文字に戻す
        _aNum = 0;

        while (beforeRand == _qNum)
        {
            // _qNumに0～問題数の数までのランダムな数字を1つ入れる
            _qNum = Random.Range(1, _question.Length);
        }
        beforeRand = _qNum;

        if (storyProgress >= 18)
        {
            _qNum = 0;
        }

        _fString = _furigana[_qNum];
        _qString = _question[_qNum];

        CreateRomSliceList(_fString);
        _aString = string.Join("", GetRomSliceListWithoutSkip());

        // 文字を変更する
        fText.text = _fString;
        qText.text = _qString;
        aText.text = _aString;

        isInput = true;
    }

    // 正解用の関数
    void Correct()
    {
        //正解した時の処理
        _aNum++;

        aText.text = "<color=#6A6A6A>" + _aString.Substring(0, _aNum) + "</color>" + _aString.Substring(_aNum);

        audioSource.PlayOneShot(typingSound);
    }

    // 不正解用の関数
    void Miss()
    {
        // 不正解の時の処理
        aText.text = "<color=#6A6A6A>" + _aString.Substring(0, _aNum) + "</color>"
            + "<color=#FF0000>" + _aString.Substring(_aNum, 1) + "</color>"
            + _aString.Substring(_aNum + 1);

        audioSource.PlayOneShot(missSound);
    }

    // 柔軟な入力をしたときに次の文字が小文字なら小文字を挿入
    void AddSmallMoji()
    {
        int nextMojiNum = _fCountList[_aNum] + 1;
        // 次の文字がなければ処理しない
        if (_fString.Length - 1 < nextMojiNum)
        {
            return;
        }

        string nextMoji = _fString[nextMojiNum].ToString();
        string a = cd.dictionary[nextMoji][0];

        // string aの0番目が"x"でも"l"でもなければ処理しない
        if (a[0] != 'x' && a[0] != 'l')
        {
            return;
        }

        // romSliceListに挿入と表示の反映
        _romSliceList.Insert(nextMojiNum, a);
        // SKIPを削除
        _romSliceList.RemoveAt(nextMojiNum + 1);

        // 変更したリストの再表示
        ReCreateList(_romSliceList);
        _aString = string.Join("", GetRomSliceListWithoutSkip());
    }

    void CreateRomSliceList(string moji)
    {
        _romSliceList.Clear();
        _fCountList.Clear();
        _romNumList.Clear();

        for (int i = 0; i < moji.Length; i++)
        {
            string a = cd.dictionary[moji[i].ToString()][0];

            if (moji[i].ToString() == "ゃ" || moji[i].ToString() == "ゅ" || moji[i].ToString() == "ょ" || moji[i].ToString() == "ぁ" || moji[i].ToString() == "ぃ" || moji[i].ToString() == "ぅ" || moji[i].ToString() == "ぇ" || moji[i].ToString() == "ぉ")
            {
                a = "SKIP";
            }
            else if (moji[i].ToString() == "っ" && i + 1 < moji.Length)
            {
                a = cd.dictionary[moji[i + 1].ToString()][0][0].ToString();
            }
            // 「ん」の処理
            else if (moji[i].ToString() == "ん")
            {
                // 「ん」が最後の文字なら"nn"と入力
                if (i == moji.Length - 1)
                {
                    a = "nn";
                }
                // 「ん」の次の文字があ行orな行orや行orん
                else if (moji[i + 1].ToString() == "あ" || moji[i + 1].ToString() == "い" || moji[i + 1].ToString() == "う" || moji[i + 1].ToString() == "え" || moji[i + 1].ToString() == "お" || moji[i + 1].ToString() == "な" || moji[i + 1].ToString() == "に" || moji[i + 1].ToString() == "ぬ" || moji[i + 1].ToString() == "ね" || moji[i + 1].ToString() == "の" || moji[i + 1].ToString() == "や" || moji[i + 1].ToString() == "ゆ" || moji[i + 1].ToString() == "よ" || moji[i + 1].ToString() == "ん")
                {
                    a = "nn";
                }
            }
            else if (i + 1 < moji.Length)
            {
                // 次の文字も含めて辞書から探す
                string addNextMoji = moji[i].ToString() + moji[i + 1].ToString();
                if (cd.dictionary.ContainsKey(addNextMoji))
                {
                    a = cd.dictionary[addNextMoji][0];
                }
            }

            _romSliceList.Add(a);

            if (a == "SKIP")
            {
                continue;
            }
            for (int j = 0; j < a.Length; j++)
            {
                _fCountList.Add(i);
                _romNumList.Add(j);
            }
        }
    }

    void ReCreateList(List<string> romList)
    {
        _fCountList.Clear();
        _romNumList.Clear();

        for (int i = 0; i < romList.Count; i++)
        {
            string a = romList[i];
            if (a == "SKIP")
            {
                continue;
            }
            for (int j = 0; j < a.Length; j++)
            {
                _fCountList.Add(i);
                _romNumList.Add(j);
            }
        }
    }

    // SKIP無しの表示をさせるためのListを作り直す
    List<string> GetRomSliceListWithoutSkip()
    {
        List<string> returnList = new List<string>();
        foreach (string rom in _romSliceList)
        {
            if (rom == "SKIP")
            {
                continue;
            }
            returnList.Add(rom);
        }
        return returnList;
    }
}
