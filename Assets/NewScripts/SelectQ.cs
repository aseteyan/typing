using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

// ��ʏ�̃e�L�X�g�̕�����ύX
public class SelectQ : MonoBehaviour
{
    // ��ʏ�̃e�L�X�g�������Ă���
    [SerializeField] Text fText;
    [SerializeField] Text qText;
    [SerializeField] Text aText;

    // ����p�ӂ��Ă���
    private string[] _furigana = { "�߂���", "���ꂢ", "���傤����", "�������Ⴍ", "�����", "���񂻂�", "�ق�����", "����", "������", "���Ƃ���", "��������", "����", "������", "��悢", "�����܂������Ă��肩������", "���Ԃ͂��Ƃ炸", "�����̂����ɂ�����˂�", "�������΂܂��", "�������������ɂ�����", "����������炨����", "�Ȃ�����ɂ͂�", "�͂₨���͂������̂Ƃ�", "�ł邭���͂������", "�Ȃ����͂ЂƂ̂��߂Ȃ炸", "�₯�����ɂ݂�", "����߂ɂ������", "�炭����΂�����", "��傤�₭�͂����ɂɂ���" };
    private string[] _question = { "����", "�ؗ�", "�ꕶ", "�Î�", "�O�k", "���z", "����", "���", "�͓�", "����", "����", "����", "���J", "�퐶", "���B���ĐK�B����", "���I�Ƃ炸", "�΂̏�ɂ��O�N", "�}���Ή��", "�����ɗ�����", "�����؂��痎����", "�������ʂɖI", "���N���͎O���̓�", "�o��Y�͑ł����", "��͐l�ׂ̈Ȃ炸", "�Ă��΂ɐ�", "���ڂ��M���", "�y����΋ꂠ��", "�ǖ�͌��ɋꂵ" };

    // ���Ԗڂ��w�肷�邽�߂�string
    private string _fString;
    private string _qString;
    private string _aString;

    // ���Ԗڂ̖�肩�I�яo��int
    private int _qNum;

    // 2�񑱂��ē�����肪�o�Ȃ��悤�ɂ���ۑ��p
    private int beforeRand;

    // ���̉������ڂ�
    private int _aNum;

    // ���̒ǉ�
    public AudioClip typeSound;
    public AudioClip missSound;
    public AudioClip killSound;
    public AudioClip damageSound;
    AudioSource audioSource;

    // �����Ă邩�ǂ����̔��f
    bool isCorrect;

    // �����̐錾
    private ChangeDictionary cd;
    private List<string> _romSliceList = new List<string>();
    // �������ڂ̂ӂ肪�Ȃ���͂��Ă��邩
    private List<int> _fCountList = new List<int>();
    private List<int> _romNumList = new List<int>();

    // �����������o�Ă��Ȃ��Ƃ����͂�W����p��int
    public int stopInput;

    // story�i�s�p
    int storyProgress;

    // �G�̌̎��ʗp
    public int Indvidual;
    // �G�̌��j�p
    public bool isKill;
    public bool isDamage;

    public SpriteRenderer background;

    // �Q�[���J�n���Ɉ�x�������s��������
    void Start()
    {
        // �_��ȓ��͂̂��߂̎�����ǂݍ���
        cd = GetComponent<ChangeDictionary>();

        // �����o��
        ChangeQ();

        audioSource = GetComponent<AudioSource>();
        isKill = false;
        isDamage = false;
    }

    // Update is called once per frame
    void Update()
    {
        // ���͂��ꂽ�Ƃ��ɔ��f����
        if (Input.anyKeyDown && stopInput == 1)
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
                    StartCoroutine(WaitChangeQ());
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
                    StartCoroutine(WaitChangeQ());
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
                        StartCoroutine(WaitChangeQ());
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

        while (beforeRand == _qNum)
        {
            // _qNum��0�`��萔�̐��܂ł̃����_���Ȑ�����1�����
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

        // ������ύX����
        fText.text = _fString;
        qText.text = _qString;
        aText.text = _aString;

        stopInput = 1;
    }

    // ����p�̊֐�
    void Correct()
    {
        //�����������̏���
        _aNum++;

        aText.text = "<color=#6A6A6A>" + _aString.Substring(0, _aNum) + "</color>" + _aString.Substring(_aNum);

        audioSource.PlayOneShot(typeSound);
    }

    // �s����p�̊֐�
    void Miss()
    {
        // �s�����̎��̏���
        aText.text = "<color=#6A6A6A>" + _aString.Substring(0, _aNum) + "</color>"
            + "<color=#FF0000>" + _aString.Substring(_aNum, 1) + "</color>"
            + _aString.Substring(_aNum + 1);

        audioSource.PlayOneShot(missSound);
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

        for (int i = 0; i < moji.Length; i++)
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
            else if (i + 1 < moji.Length)
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
