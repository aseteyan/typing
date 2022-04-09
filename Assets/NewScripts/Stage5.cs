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

    // ����p�ӂ��Ă���
    private string[] _furigana = { "�������܂ł�����ł���Ă��肪�Ƃ�", "�������ɂ�邳��Ȃ����Ƃ�����", "����Ȃ��ƂɂȂ�Ƃ͂�����Ȃ�����", "�ǂ�Ȃɂ������������낤��", "����܂��Ă�����܂肫��Ȃ���", "�Ȃ��������Ȃ���Ȃ�Ă��Ƃ�������", "���݂͂킽���������ł���", "���̂���̂킽���͂܂����ǂ�������", "�킽���̂��������Ђ���������", "���������ɂɂǂƂ��Ȃ��Ƃ�����", "�����납�炷�܂Ȃ������Ƃ������Ă���", "�Ȃ�����Ȃ��Ƃ����Ă��܂����̂�", "���̂Ƃ��͂ǂ������Ă���", "�ق�Ƃ��ɂ������킯�Ȃ�" };
    private string[] _question = { "�Ō�܂ŗV��ł���Ă��肪�Ƃ�", "�m���ɋ�����Ȃ����Ƃ�����", "����Ȏ��ɂȂ�Ƃ͎v��Ȃ�����", "�ǂ�Ȃɒɂ��������낤��", "�ӂ��Ă��ӂ肫��Ȃ���", "�Ȃ��΂𓊂���Ȃ�Ă��Ƃ�������", "�N�͎�������ł���", "���̍��̎��͂܂��q��������", "���̈ӎ����Ⴉ������", "��΂ɓ�x�Ƃ��Ȃ��Ɛ���", "�S���炷�܂Ȃ������Ǝv���Ă���", "���̂���Ȃ��Ƃ����Ă��܂����̂�", "���̎��͂ǂ������Ă���", "�{���ɐ\����Ȃ�" };

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

    // �����Ă邩�ǂ����̔��f
    bool isCorrect;

    // ���͉�
    public bool isInput;

    // �����̐錾
    private ChangeDictionary cd;
    private List<string> _romSliceList = new List<string>();
    // �������ڂ̂ӂ肪�Ȃ���͂��Ă��邩
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

        // �_��ȓ��͂̂��߂̎�����ǂݍ���
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
                talkText.text = "����܂��Ă��񂾂�˂��I";
            }
            if (storyProgress == 1)
            {
                storyProgress = 2;
                talkText.text = "(��...���܂Ȃ�����...)";
            }
            if (storyProgress == 0)
            {
                storyProgress = 1;
                talkText.text = "�������A�N�͂��̂Ƃ��΂𓊂��Ă����I";
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
                talkText.text = "(�{���ɐ\����Ȃ�����...)";
            }
            if (storyProgress == 31)
            {
                storyProgress = 32;
                talkText.text = "���݂���܂��ē����Ȃ��Ȃ肻����";
            }
            if (storyProgress == 30)
            {
                storyProgress = 31;
                talkText.text = "�l�͐΂𓊂���ꂽ���݂œ�����悤�ɂȂ���";
            }
            if (storyProgress == 35)
            {
                storyProgress = 101;
                StartCoroutine(ChangeGame());
            }
        }

        // ���͂��ꂽ�Ƃ��ɔ��f����
        if (Input.anyKeyDown && isInput)
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
                    StartCoroutine(DamageZizou());
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
                    StartCoroutine(DamageZizou());
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
        explanationText.text = "���ʉ��EBGM�f�ށF�|�P�b�g�T�E���h";
        yield return new WaitForSeconds(3f);
        explanationText.text = "�C���X�g�f�ށF���炷�Ƃ�";
        yield return new WaitForSeconds(3f);
        explanationText.text = "����ҁF�Έ�(����)";
        yield return new WaitForSeconds(3f);
        explanationText.text = "�^�C�g����ʂŁw�W�P�T�x�Ɠ��͂����\r\n�~�j�Q�[�����V�ׂ܂��I";
        enterText.text = "ENTER��";
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
        talkText.text = "������x�Ƃ���Ȃ��Ƃ���񂶂��..��...��...";
        zizou.color = new Color32(0, 0, 0, 0);
        yield return new WaitForSeconds(3f);
        GameObject zizouScript = GameObject.Find("zizou1");
        zizouScript.GetComponent<ZizouAppear>().StartCoroutine("ZizouGoodbye");
        yield return new WaitForSeconds(3f);
        talkText.text = "(���n���l...)";
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
        talkText.text = "�����I����ȂɎӂ���Ȃ��...";
        enterText.text = "ENTER��";

        storyProgress = 30;
    }

    IEnumerator ConcentrationLine()
    {
        audioSource.Stop();
        GameObject director = GameObject.Find("GameDirector");
        director.GetComponent<GameDirector>().Audio();
        audioSource.PlayOneShot(amazingSound);

        talkText.text = "�x�@�͂���Ȃ��񂾂�I�I�I";
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
                        StartCoroutine(DamageZizou());
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

        // ������ύX����
        fText.text = _fString;
        qText.text = _qString;
        aText.text = _aString;

        isInput = true;
    }

    // ����p�̊֐�
    void Correct()
    {
        //�����������̏���
        _aNum++;

        aText.text = "<color=#6A6A6A>" + _aString.Substring(0, _aNum) + "</color>" + _aString.Substring(_aNum);

        audioSource.PlayOneShot(typingSound);
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
