using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    // scoreに関するint
    static private double _aInt;
    static private double _cInt;
    static private int _mInt;
    static private double _rInt;

    [SerializeField] Text aScore; // 総タイピング数
    [SerializeField] Text mScore; // ミスタイピング数
    [SerializeField] Text rScore; // 正確率

    [SerializeField] Text perScore;

    SpriteRenderer panel;
    SpriteRenderer panel1;
    SpriteRenderer panel2;
    Image flame;
    Image backFlame;
    Image flame1;
    Image flame2;
    Image backFlame1;
    Image backFlame2;

    Text fText;
    Text qText;
    Text aText;

    GameObject life1;
    GameObject life2;
    GameObject life3;
    GameObject life4;
    GameObject life5;
    public int lifeNumber = 3;
    SpriteRenderer damageFlash;
    SpriteRenderer deathBlind;

    AudioSource audioSource;
    public AudioClip deathSound;
    public AudioClip damageSound;

    static float difficulty1;
    static float difficulty2;
    static float difficulty3;
    public float fDifficulty1;
    public float fDifficulty2;
    public float fDifficulty3;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void DifficultySet()
    {
        fDifficulty1 = difficulty1;
        fDifficulty2 = difficulty2;
        fDifficulty3 = difficulty3;
    }

    public void DifficultySettingEasy()
    {
        difficulty1 = 0.001f;
        difficulty2 = 0.002f;
        difficulty3 = 0.0008f;
    }

    public void DifficultySettingNormal()
    {
        difficulty1 = 0.004f;
        difficulty2 = 0.008f;
        difficulty3 = 0.0027f;
    }

    public void DifficultySettingHard()
    {
        difficulty1 = 0.0055f;
        difficulty2 = 0.0095f;
        difficulty3 = 0.0042f;
    }

    public void CorrectScore()
    {
        _aInt++;
        _cInt++;
        _rInt = _cInt / _aInt * 100;

        aScore.text = "キー入力:" + _aInt.ToString() + "回";
        rScore.text = "正確率:" + _rInt.ToString("F1") + "%";
    }

    public void MissScore()
    {
        _aInt++;
        _mInt++;
        _rInt = _cInt / _aInt * 100;

        aScore.text = "キー入力:" + _aInt.ToString() + "回";
        mScore.text = "ミス:" + _mInt.ToString() + "回";
        rScore.text = "正確率:" + _rInt.ToString("F1") + "%";
    }

    public void StartSceneManager()
    {
        aScore.text = "キー入力:" + _aInt.ToString() + "回";
        mScore.text = "ミス:" + _mInt.ToString() + "回";
        rScore.text = "正確率:" + _rInt.ToString("F1") + "%";

        perScore.text = (_cInt / 60f).ToString("0.00") + "回/秒";
    }

    public void StartTypingScene()
    {
        _aInt = 0;
        _cInt = 0;
        _mInt = 0;
        _rInt = 0;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("TypingScene");
        }
    }

    public void QuestionAppear()
    {
        panel1 = GameObject.Find("panel1").GetComponent<SpriteRenderer>();
        panel2 = GameObject.Find("panel2").GetComponent<SpriteRenderer>();
        flame1 = GameObject.Find("flame1").GetComponent<Image>();
        flame2 = GameObject.Find("flame2").GetComponent<Image>();
        panel1.color = new Color32(0, 0, 0, 230);
        panel2.color = new Color32(0, 0, 0, 230);
        flame1.color = new Color(255, 255, 255, 255);
        flame2.color = new Color(255, 255, 255, 255);
    }
    public void QuestionDisappear()
    {
        panel1 = GameObject.Find("panel1").GetComponent<SpriteRenderer>();
        panel2 = GameObject.Find("panel2").GetComponent<SpriteRenderer>();
        flame1 = GameObject.Find("flame1").GetComponent<Image>();
        flame2 = GameObject.Find("flame2").GetComponent<Image>();
        panel1.color = new Color32(0, 0, 0, 0);
        panel2.color = new Color32(0, 0, 0, 0);
        flame1.color = new Color(255, 255, 255, 0);
        flame2.color = new Color(255, 255, 255, 0);
    }

    public void QuestionDisappear1()
    {
        panel = GameObject.Find("OutputLocation").GetComponent<SpriteRenderer>();
        flame = GameObject.Find("LimitLine").GetComponent<Image>();
        backFlame = GameObject.Find("LimitLineBackGround").GetComponent<Image>();
        panel.color = new Color32(0, 0, 0, 0);
        flame.color = new Color(255, 255, 255, 0);
        backFlame.color = new Color(90, 90, 90, 0);
    }
    public void QuestionAppear1()
    {
        panel = GameObject.Find("OutputLocation").GetComponent<SpriteRenderer>();
        flame = GameObject.Find("LimitLine").GetComponent<Image>();
        backFlame = GameObject.Find("LimitLineBackGround").GetComponent<Image>();
        panel.color = new Color32(0, 0, 0, 230);
        flame.color = new Color(255, 255, 255, 255);
        backFlame.color = new Color32(90, 90, 90, 255);
    }

    public IEnumerator DecreaseLife()
    {
        /*if (lifeNumber <= 5)
        {
            life5 = GameObject.Find("Life5");
            Destroy(life5);
        }
        if (lifeNumber <= 4)
        {
            life4 = GameObject.Find("Life4");
            Destroy(life4);
        }*/
        if (lifeNumber <= 3)
        {
            life3 = GameObject.Find("Life3");
            Destroy(life3);
        }
        if (lifeNumber <= 2)
        {
            life2 = GameObject.Find("Life2");
            Destroy(life2);
        }
        if (lifeNumber <= 1)
        {
            GameObject qChage = GameObject.Find("Story2Manager");
            qChage.GetComponent<SelectQ>().stopInput = 0;

            life1 = GameObject.Find("Life1");
            Destroy(life1);
            audioSource.PlayOneShot(damageSound);
            audioSource.PlayOneShot(deathSound);

            QuestionDisappear1();
            fText = GameObject.Find("fText").GetComponent<Text>();
            qText = GameObject.Find("qText").GetComponent<Text>();
            aText = GameObject.Find("aText").GetComponent<Text>();
            fText.text = "";
            qText.text = "";
            aText.text = "";

            damageFlash = GameObject.Find("DamageFlash").GetComponent<SpriteRenderer>();
            damageFlash.color = new Color32(255, 0, 0, 200);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 0);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 200);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 0);

            for (int i = 0; i < 255; i++)
            {
                deathBlind = GameObject.Find("DeathBlind").GetComponent<SpriteRenderer>();
                deathBlind.color += new Color32(0, 0, 0, 1);
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("GameOver");
        }

        if (lifeNumber >= 2)
        {
            damageFlash = GameObject.Find("DamageFlash").GetComponent<SpriteRenderer>();
            damageFlash.color = new Color32(255, 0, 0, 200);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 0);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 200);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 0);
        }

        lifeNumber--;
    }

    public IEnumerator DecreaseLife4()
    {
        /*if (lifeNumber <= 5)
        {
            life5 = GameObject.Find("Life5");
            Destroy(life5);
        }
        if (lifeNumber <= 4)
        {
            life4 = GameObject.Find("Life4");
            Destroy(life4);
        }*/
        if (lifeNumber <= 3)
        {
            life3 = GameObject.Find("Life3");
            Destroy(life3);
        }
        if (lifeNumber <= 2)
        {
            life2 = GameObject.Find("Life2");
            Destroy(life2);
        }
        if (lifeNumber <= 1)
        {
            GameObject qChage = GameObject.Find("Story4Manager");
            qChage.GetComponent<NewSelectQ4>().stopInput = false;

            life1 = GameObject.Find("Life1");
            Destroy(life1);
            audioSource.PlayOneShot(damageSound);
            audioSource.PlayOneShot(deathSound);

            QuestionDisappear4();
            QuestionDisappear41();
            Destroy(GameObject.Find("qText1"));
            Destroy(GameObject.Find("aText1"));
            Destroy(GameObject.Find("qText2"));
            Destroy(GameObject.Find("aText2"));
            Destroy(GameObject.Find("flame1"));
            Destroy(GameObject.Find("flame2"));

            damageFlash = GameObject.Find("DamageFlash").GetComponent<SpriteRenderer>();
            damageFlash.color = new Color32(255, 0, 0, 200);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 0);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 200);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 0);

            Destroy(GameObject.Find("Slider"));

            for (int i = 0; i < 255; i++)
            {
                deathBlind = GameObject.Find("DeathBlind").GetComponent<SpriteRenderer>();
                deathBlind.color += new Color32(0, 0, 0, 1);
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("GameOver1");
        }

        if (lifeNumber >= 2)
        {
            damageFlash = GameObject.Find("DamageFlash").GetComponent<SpriteRenderer>();
            damageFlash.color = new Color32(255, 0, 0, 200);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 0);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 200);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 0);
        }

        lifeNumber--;
    }

    public IEnumerator DecreaseLife5()
    {
        /*if (lifeNumber <= 5)
        {
            life5 = GameObject.Find("Life5");
            Destroy(life5);
        }
        if (lifeNumber <= 4)
        {
            life4 = GameObject.Find("Life4");
            Destroy(life4);
        }*/
        if (lifeNumber <= 3)
        {
            life3 = GameObject.Find("Life3");
            Destroy(life3);
        }
        if (lifeNumber <= 2)
        {
            life2 = GameObject.Find("Life2");
            Destroy(life2);
        }
        if (lifeNumber <= 1)
        {
            GameObject qChage = GameObject.Find("Story5Manager");
            qChage.GetComponent<Stage5>().isInput = false;

            life1 = GameObject.Find("Life1");
            Destroy(life1);
            audioSource.PlayOneShot(damageSound);
            audioSource.PlayOneShot(deathSound);

            Destroy(GameObject.Find("backframe"));
            Destroy(GameObject.Find("frame"));
            Destroy(GameObject.Find("OutputLocation"));
            Destroy(GameObject.Find("Slider"));

            fText = GameObject.Find("fText").GetComponent<Text>();
            qText = GameObject.Find("qText").GetComponent<Text>();
            aText = GameObject.Find("aText").GetComponent<Text>();
            fText.text = "";
            qText.text = "";
            aText.text = "";

            damageFlash = GameObject.Find("DamageFlash").GetComponent<SpriteRenderer>();
            damageFlash.color = new Color32(255, 0, 0, 200);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 0);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 200);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 0);

            for (int i = 0; i < 255; i++)
            {
                deathBlind = GameObject.Find("DeathBlind").GetComponent<SpriteRenderer>();
                deathBlind.color += new Color32(0, 0, 0, 1);
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("GameOver2");
        }

        if (lifeNumber >= 2)
        {
            damageFlash = GameObject.Find("DamageFlash").GetComponent<SpriteRenderer>();
            damageFlash.color = new Color32(255, 0, 0, 200);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 0);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 200);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 0);
        }

        lifeNumber--;
    }

    public IEnumerator MiniGameLife()
    {
        if (lifeNumber <= 5)
        {
            life5 = GameObject.Find("Life5");
            Destroy(life5);
        }
        if (lifeNumber <= 4)
        {
            life4 = GameObject.Find("Life4");
            Destroy(life4);
        }
        if (lifeNumber <= 3)
        {
            life3 = GameObject.Find("Life3");
            Destroy(life3);
        }
        if (lifeNumber <= 2)
        {
            life2 = GameObject.Find("Life2");
            Destroy(life2);
        }
        if (lifeNumber <= 1)
        {
            GameObject qChage = GameObject.Find("MiniGameManager");
            qChage.GetComponent<MiniGameManager>().stopInput = false;
            GameObject scoreManager = GameObject.Find("ScoreManager");
            scoreManager.GetComponent<ScoreManager>().AverageInput();

            life1 = GameObject.Find("Life1");
            Destroy(life1);
            audioSource.PlayOneShot(damageSound);
            audioSource.PlayOneShot(deathSound);

            Destroy(GameObject.Find("Canvas"));
            Destroy(GameObject.Find("OutputLocations"));

            damageFlash = GameObject.Find("DamageFlash").GetComponent<SpriteRenderer>();
            damageFlash.color = new Color32(255, 0, 0, 200);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 0);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 200);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 0);

            Destroy(GameObject.Find("Slider"));

            for (int i = 0; i < 255; i++)
            {
                deathBlind = GameObject.Find("DeathBlind").GetComponent<SpriteRenderer>();
                deathBlind.color += new Color32(0, 0, 0, 1);
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("RankingScene");
        }

        if (lifeNumber >= 2)
        {
            damageFlash = GameObject.Find("DamageFlash").GetComponent<SpriteRenderer>();
            damageFlash.color = new Color32(255, 0, 0, 200);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 0);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 200);
            yield return new WaitForSeconds(0.1f);
            damageFlash.color = new Color32(255, 0, 0, 0);
        }

        lifeNumber--;
    }

    public void QuestionAppear4()
    {
        panel1 = GameObject.Find("panel1").GetComponent<SpriteRenderer>();
        flame1 = GameObject.Find("flame1").GetComponent<Image>();
        backFlame1 = GameObject.Find("LimitLineBackGround1").GetComponent<Image>();
        panel1.color = new Color32(0, 0, 0, 230);
        flame1.color = new Color(255, 255, 255, 255);
        backFlame1.color = new Color32(90, 90, 90, 255);
    }

    public void QuestionDisappear4()
    {
        panel1 = GameObject.Find("panel1").GetComponent<SpriteRenderer>();
        flame1 = GameObject.Find("flame1").GetComponent<Image>();
        backFlame1 = GameObject.Find("LimitLineBackGround1").GetComponent<Image>();
        panel1.color = new Color32(0, 0, 0, 0);
        flame1.color = new Color(255, 255, 255, 0);
        backFlame1.color = new Color32(90, 90, 90, 0);
    }

    public void QuestionAppear41()
    {
        panel2 = GameObject.Find("panel2").GetComponent<SpriteRenderer>();
        flame2 = GameObject.Find("flame2").GetComponent<Image>();
        backFlame2 = GameObject.Find("LimitLineBackGround2").GetComponent<Image>();
        panel2.color = new Color32(0, 0, 0, 230);
        flame2.color = new Color(255, 255, 255, 255);
        backFlame2.color = new Color32(90, 90, 90, 255);
    }

    public void QuestionDisappear41()
    {
        panel2 = GameObject.Find("panel2").GetComponent<SpriteRenderer>();
        flame2 = GameObject.Find("flame2").GetComponent<Image>();
        backFlame2 = GameObject.Find("LimitLineBackGround2").GetComponent<Image>();
        panel2.color = new Color32(0, 0, 0, 0);
        flame2.color = new Color(255, 255, 255, 0);
        backFlame2.color = new Color32(90, 90, 90, 0);
    }

    public void Audio()
    {
        audioSource.Play();
    }
    public void AudioStop()
    {
        audioSource.Stop();
    }
}
