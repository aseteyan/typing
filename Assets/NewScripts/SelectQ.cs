using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

// 画面上のテキストの文字を変更
public class SelectQ : MonoBehaviour
{
    // 画面上のテキストを持ってくる
    [SerializeField] Text fText;
    [SerializeField] Text qText;
    [SerializeField] Text aText;

    // 問題を用意しておく
    private string[] _furigana = { "めいじ", "かれい", "じょうもん", "せいじゃく", "しゃば", "げんそう", "ほしぞら", "ぎんが", "かっぱ", "ことだま", "たそがれ", "せつな", "しぐれ", "やよい", "あたまかくしてしりかくさず", "あぶはちとらず", "いしのうえにもさんねん", "いそがばまわれ", "こうかいさきにたたず", "さるもきからおちる", "なきっつらにはち", "はやおきはさんもんのとく", "でるくいはうたれる", "なさけはひとのためならず", "やけいしにみず", "よわりめにたたりめ", "らくあればくあり", "りょうやくはくちににがし" };
    private string[] _question = { "明治", "華麗", "縄文", "静寂", "娑婆", "幻想", "星空", "銀河", "河童", "言霊", "黄昏", "刹那", "時雨", "弥生", "頭隠して尻隠さず", "虻蜂とらず", "石の上にも三年", "急がば回れ", "後悔先に立たず", "猿も木から落ちる", "泣きっ面に蜂", "早起きは三文の徳", "出る杭は打たれる", "情けは人の為ならず", "焼け石に水", "弱り目に祟り目", "楽あれば苦あり", "良薬は口に苦し" };

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

    // 音の追加
    public AudioClip typeSound;
    public AudioClip missSound;
    public AudioClip killSound;
    public AudioClip damageSound;
    AudioSource audioSource;

    // 合ってるかどうかの判断
    bool isCorrect;

    // 辞書の宣言
    private ChangeDictionary cd;
    private List<string> _romSliceList = new List<string>();
    // 何文字目のふりがなを入力しているか
    private List<int> _fCountList = new List<int>();
    private List<int> _romNumList = new List<int>();

    // 文字が何も出ていないとき入力を妨げる用のint
    public int stopInput;

    // story進行用
    int storyProgress;

    // 敵の個体識別用
    public int Indvidual;
    // 敵の撃破用
    public bool isKill;
    public bool isDamage;

    public SpriteRenderer background;

    // ゲーム開始時に一度だけ実行されるもの
    void Start()
    {
        // 柔軟な入力のための辞書を読み込む
        cd = GetComponent<ChangeDictionary>();

        // 問題を出す
        ChangeQ();

        audioSource = GetComponent<AudioSource>();
        isKill = false;
        isDamage = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 入力されたときに判断する
        if (Input.anyKeyDown && stopInput == 1)
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
                GameObject limitFrame = GameObject.Find("LimitLine");
                limitFrame.GetComponent<DirectingTime>().MissDamage();
            }
        }
    }

    public IEnumerator WaitChangeQ()
    {
        stopInput = 0;

        audioSource.PlayOneShot(killSound);

        fText.text = "";
        qText.text = "";
        aText.text = "";
        GameObject limitFrame = GameObject.Find("LimitLine");
        limitFrame.GetComponent<DirectingTime>().UndoFrame();
        GameObject director = GameObject.Find("GameDirector");
        director.GetComponent<GameDirector>().QuestionDisappear1();
        isKill = true;
        if (storyProgress < 19)
        {
            yield return new WaitForSeconds(1f);

            limitFrame.GetComponent<DirectingTime>().isStop = true;
            director.GetComponent<GameDirector>().QuestionAppear1();
            isKill = false;
            Indvidual++;
            ChangeQ();
        }
        if (storyProgress == 19)
        {
            for (int i = 0; i < 255; i++)
            {
                background.color -= new Color32(0, 0, 0, 1);
                yield return new WaitForSeconds(0.03f);
            }
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("Stage3");
        }
        storyProgress++;
    }
    public IEnumerator DamageChangeQ()
    {
        stopInput = 0;

        audioSource.PlayOneShot(damageSound);

        fText.text = "";
        qText.text = "";
        aText.text = "";
        GameObject limitFrame = GameObject.Find("LimitLine");
        limitFrame.GetComponent<DirectingTime>().UndoFrame();
        GameObject director = GameObject.Find("GameDirector");
        director.GetComponent<GameDirector>().QuestionDisappear1();
        isDamage = true;
        if (storyProgress < 19)
        {
            yield return new WaitForSeconds(1f);

            limitFrame.GetComponent<DirectingTime>().isStop = true;
            director.GetComponent<GameDirector>().QuestionAppear1();
            isDamage = false;
            Indvidual++;
            ChangeQ();
        }
        if (storyProgress == 19)
        {
            for (int i = 0; i < 255; i++)
            {
                background.color -= new Color32(0, 0, 0, 1);
                yield return new WaitForSeconds(0.03f);
            }
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("Stage3");
        }
        storyProgress++;
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
            if (storyProgress < 9)
            {
                _qNum = Random.Range(0, 14);
            }
            else if (storyProgress >= 9)
            {
                _qNum = Random.Range(14, _question.Length);
            }
        }
        beforeRand = _qNum;

        _fString = _furigana[_qNum];
        _qString = _question[_qNum];

        CreateRomSliceList(_fString);
        _aString = string.Join("", GetRomSliceListWithoutSkip());

        // 文字を変更する
        fText.text = _fString;
        qText.text = _qString;
        aText.text = _aString;

        stopInput = 1;
    }

    // 正解用の関数
    void Correct()
    {
        //正解した時の処理
        _aNum++;

        aText.text = "<color=#6A6A6A>" + _aString.Substring(0, _aNum) + "</color>" + _aString.Substring(_aNum);

        audioSource.PlayOneShot(typeSound);
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
