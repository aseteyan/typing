using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

// ��ʏ�̃e�L�X�g�̕�����ύX
public class TypingManager : MonoBehaviour
{
    // ��ʏ�̃e�L�X�g�������Ă���
    [SerializeField] Text fText; // �ӂ肪�ȗp�̃e�L�X�g
    [SerializeField] Text qText; // ���p�̃e�L�X�g
    [SerializeField] Text aText; // �𓚗p�̃e�L�X�g

    [SerializeField] Text timeText; // �o�ߎ���

    // ����
    public float seconds;
    
    // ����p�ӂ��Ă���
    /*
    private string[] _furigana = { "������", "��������", "����������������", "�肩", "���݂�����傤", "���傤����", "���������ł��", "���Ⴉ��", "�Ȃ����������肾������", "���񂴂�", "�Ղ낮��݂�"};
    private string[] _question = { "����", "���w", "����̈�", "����", "鳖���", "����", "�g�ѓd�b", "�Љ�", "���t������b", "���Z�q", "�v���O���~���O"};
    private string[] _answer = { "kokugo", "suugaku", "okazakitaiiku", "rika", "timimouryou", "kyousitu", "keitaidenwa", "syakai", "naikakusouridaizinn", "enzansi", "puroguramingu"};
    */

    // �e�L�X�g�f�[�^��ǂݍ���
    [SerializeField] TextAsset _furigana;
    [SerializeField] TextAsset _question;
    //[SerializeField] TextAsset _answer;

    //�e�L�X�g�f�[�^���i�[���邽�߂̃��X�g
    private List<string> _fList = new List<string>();
    private List<string> _qList = new List<string>();
    //private List<string> _aList = new List<string>();

    // ���Ԗڂ��w�肷�邽�߂�string
    private string _fString;
    private string _qString;
    private string _aString;

    // ���Ԗڂ̖�肩�I�яo��int
    private int _qNum;

    // ���̉������ڂ�
    private int _aNum;

    // ���̒ǉ�
    public AudioClip typeSound;
    public AudioClip missSound;
    AudioSource audioSource;

    // �����Ă邩�ǂ����̔��f
    bool isCorrect;

    // �����̐錾
    private ChangeDictionary cd;
    private List<string> _romSliceList = new List<string>();
    // �������ڂ̂ӂ肪�Ȃ���͂��Ă��邩
    private List<int> _fCountList = new List<int>();
    private List<int> _romNumList = new List<int>();

    // �Q�[���J�n���Ɉ�x�������s��������
    void Start()
    {
        // �_��ȓ��͂̂��߂̎�����ǂݍ���
        cd = GetComponent<ChangeDictionary>();

        //�e�L�X�g�f�[�^�����X�g�ɓ����
        SetList();

        // �����o��
        ChangeQ();

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // ���͂��ꂽ�Ƃ��ɔ��f����
        if (Input.anyKeyDown)
        {
            isCorrect = false;
            int fCount = _fCountList[_aNum];

            if (Input.GetKeyDown(_aString[_aNum].ToString()))
            {
                // ����������
                isCorrect = true;
                Correct();

                // �Ō�̕����ɐ����������̏���
                if (_aNum >= _aString.Length)
                {
                    // ����ύX����
                    ChangeQ();
                }
            }
            else if (Input.GetKeyDown("n") && fCount > 0 && _romSliceList[fCount - 1] == "n")
            {
                // nn�̑Ή�
                _romSliceList[fCount - 1] = "nn";
                _aString = string.Join("", GetRomSliceListWithoutSkip());

                ReCreateList(_romSliceList);

                // ����������
                isCorrect = true;
                Correct();

                // �Ō�̕����ɐ����������̏���
                if (_aNum >= _aString.Length)
                {
                    // ����ύX����
                    ChangeQ();
                }
            }
            else
            {
                // ���ɐ��������͂����邩
                string currentF = _fString[fCount].ToString();

                if (fCount < _fString.Length - 1)
                {
                    // 2�������l��������⌟��
                    string addNextMoji = _fString[fCount].ToString() + _fString[fCount + 1].ToString();
                    CheckIrregularType(addNextMoji, fCount, false);
                }

                if (!isCorrect)
                {
                    // �ʏ�̌�⌟��
                    string moji = _fString[fCount].ToString();
                    CheckIrregularType(moji, fCount, true);
                }
            }

            // �s����
            if (!isCorrect)
            {
                Miss();
            }
        }

        // �^�C�}�[
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

                    // ����������
                    isCorrect = true;

                    if (addSmallMoji)
                    {
                        AddSmallMoji();
                    }

                    Correct();

                    // �Ō�̕����ɐ����������̏���
                    if (_aNum >= _aString.Length)
                    {
                        // ����ύX����
                        ChangeQ();
                    }
                    break;
                }
            }
        }
    }

    // �����o�����߂̊֐�
    void ChangeQ()
    {
        // 0�Ԗڂ̕����ɖ߂�
        _aNum = 0;

        // _qNum��0�`��萔�̐��܂ł̃����_���Ȑ�����1�����
        _qNum = Random.Range(0, _qList.Count);

        _fString = _fList[_qNum];
        _qString = _qList[_qNum];

        CreateRomSliceList(_fString);
        _aString = string.Join("", GetRomSliceListWithoutSkip());

        // ������ύX����
        fText.text = _fString;
        qText.text = _qString;
        aText.text = _aString;
    }

    // ����p�̊֐�
    void Correct()
    {
        //�����������̏���
        _aNum++;

        aText.text = "<color=#6A6A6A>" + _aString.Substring(0, _aNum) + "</color>" + _aString.Substring(_aNum);

        GameObject gameManager = GameObject.Find("GameDirector");
        gameManager.GetComponent<GameDirector>().CorrectScore();

        audioSource.PlayOneShot(typeSound);
    }

    // �s����p�̊֐�
    void Miss()
    {
        // �s�����̎��̏���
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

    // �_��ȓ��͂������Ƃ��Ɏ��̕������������Ȃ珬������}��
    void AddSmallMoji()
    {
        int nextMojiNum = _fCountList[_aNum] + 1;
        // ���̕������Ȃ���Ώ������Ȃ�
        if (_fString.Length - 1 < nextMojiNum)
        {
            return;
        }

        string nextMoji = _fString[nextMojiNum].ToString();
        string a = cd.dictionary[nextMoji][0];

        // string a��0�Ԗڂ�"x"�ł�"l"�ł��Ȃ���Ώ������Ȃ�
        if (a[0] != 'x' && a[0] != 'l')
        {
            return;
        }

        // romSliceList�ɑ}���ƕ\���̔��f
        _romSliceList.Insert(nextMojiNum, a);
        // SKIP���폜
        _romSliceList.RemoveAt(nextMojiNum + 1);

        // �ύX�������X�g�̍ĕ\��
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

            if (moji[i].ToString() == "��" || moji[i].ToString() == "��" || moji[i].ToString() == "��" || moji[i].ToString() == "��" || moji[i].ToString() == "��" || moji[i].ToString() == "��" || moji[i].ToString() == "��" || moji[i].ToString() == "��")
            {
                a = "SKIP";
            }
            else if (moji[i].ToString() == "��" && i + 1 < moji.Length)
            {
                a = cd.dictionary[moji[i + 1].ToString()][0][0].ToString();
            }
            // �u��v�̏���
            else if (moji[i].ToString() == "��")
            {
                // �u��v���Ō�̕����Ȃ�"nn"�Ɠ���
                if (i == moji.Length - 1)
                {
                    a = "nn";
                }
                // �u��v�̎��̕��������sor�ȍsor��sor��
                else if (moji[i + 1].ToString() == "��" || moji[i + 1].ToString() == "��" || moji[i + 1].ToString() == "��" || moji[i + 1].ToString() == "��" || moji[i + 1].ToString() == "��" || moji[i + 1].ToString() == "��" || moji[i + 1].ToString() == "��" || moji[i + 1].ToString() == "��" || moji[i + 1].ToString() == "��" || moji[i + 1].ToString() == "��" || moji[i + 1].ToString() == "��" || moji[i + 1].ToString() == "��" || moji[i + 1].ToString() == "��" || moji[i + 1].ToString() == "��")
                {
                    a = "nn";
                }
            }
            else if (i+ 1 < moji.Length)
            {
                // ���̕������܂߂Ď�������T��
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

    // SKIP�����̕\���������邽�߂�List����蒼��
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
