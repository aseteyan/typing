using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

// 画面上のテキストの文字を変更
public class TypingManager : MonoBehaviour
{
    // 画面上のテキストを持ってくる
    [SerializeField] Text fText; // ふりがな用のテキスト
    [SerializeField] Text qText; // 問題用のテキスト
    [SerializeField] Text aText; // 解答用のテキスト

    [SerializeField] Text timeText; // 経過時間

    // 時間
    public float seconds;
    
    // 問題を用意しておく
    /*
    private string[] _furigana = { "こくご", "すうがく", "おかざきたいいく", "りか", "ちみもうりょう", "きょうしつ", "けいたいでんわ", "しゃかい", "ないかくそうりだいじん", "えんざんし", "ぷろぐらみんぐ"};
    private string[] _question = { "国語", "数学", "岡崎体育", "理科", "魑魅魍魎", "教室", "携帯電話", "社会", "内閣総理大臣", "演算子", "プログラミング"};
    private string[] _answer = { "kokugo", "suugaku", "okazakitaiiku", "rika", "timimouryou", "kyousitu", "keitaidenwa", "syakai", "naikakusouridaizinn", "enzansi", "puroguramingu"};
    */

    // テキストデータを読み込む
    [SerializeField] TextAsset _furigana;
    [SerializeField] TextAsset _question;
    //[SerializeField] TextAsset _answer;

    //テキストデータを格納するためのリスト
    private List<string> _fList = new List<string>();
    private List<string> _qList = new List<string>();
    //private List<string> _aList = new List<string>();

    // 何番目か指定するためのstring
    private string _fString;
    private string _qString;
    private string _aString;

    // 何番目の問題か選び出すint
    private int _qNum;

    // 問題の何文字目か
    private int _aNum;

    // 音の追加
    public AudioClip typeSound;
    public AudioClip missSound;
    AudioSource audioSource;

    // 合ってるかどうかの判断
    bool isCorrect;

    // 辞書の宣言
    private ChangeDictionary cd;
    private List<string> _romSliceList = new List<string>();
    // 何文字目のふりがなを入力しているか
    private List<int> _fCountList = new List<int>();
    private List<int> _romNumList = new List<int>();

    // ゲーム開始時に一度だけ実行されるもの
    void Start()
    {
        // 柔軟な入力のための辞書を読み込む
        cd = GetComponent<ChangeDictionary>();

        //テキストデータをリストに入れる
        SetList();

        // 問題を出す
        ChangeQ();

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // 入力されたときに判断する
        if (Input.anyKeyDown)
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
                    ChangeQ();
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
                    ChangeQ();
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
            }
        }

        // タイマー
        seconds -= Time.deltaTime;
        timeText.text =seconds.ToString("00");

        if (seconds <= 0)
        {
            SceneManager.LoadScene("StartScene");
        }
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
                        ChangeQ();
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

        // _qNumに0～問題数の数までのランダムな数字を1つ入れる
        _qNum = Random.Range(0, _qList.Count);

        _fString = _fList[_qNum];
        _qString = _qList[_qNum];

        CreateRomSliceList(_fString);
        _aString = string.Join("", GetRomSliceListWithoutSkip());

        // 文字を変更する
        fText.text = _fString;
        qText.text = _qString;
        aText.text = _aString;
    }

    // 正解用の関数
    void Correct()
    {
        //正解した時の処理
        _aNum++;

        aText.text = "<color=#6A6A6A>" + _aString.Substring(0, _aNum) + "</color>" + _aString.Substring(_aNum);

        GameObject gameManager = GameObject.Find("GameDirector");
        gameManager.GetComponent<GameDirector>().CorrectScore();

        audioSource.PlayOneShot(typeSound);
    }

    // 不正解用の関数
    void Miss()
    {
        // 不正解の時の処理
        aText.text = "<color=#6A6A6A>" + _aString.Substring(0, _aNum) + "</color>"
            + "<color=#FF0000>" + _aString.Substring(_aNum, 1) + "</color>"
            + _aString.Substring(_aNum + 1);

        GameObject gameManager = GameObject.Find("GameDirector");
        gameManager.GetComponent<GameDirector>().MissScore();

        audioSource.PlayOneShot(missSound);
    }

    void SetList()
    {
        string[] _fArray = _furigana.text.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        _fList.AddRange(_fArray);

        string[] _qArray = _question.text.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        _qList.AddRange(_qArray);
        
        //string[] _aArray = _answer.text.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        //_aList.AddRange(_aArray);
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

        for(int i = 0; i < moji.Length; i++)
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
            else if (i+ 1 < moji.Length)
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
