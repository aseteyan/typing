using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip sound;
    public AudioClip select;
    bool isStart = true;
    int miniGame;
    int stageChange;

    static bool isChange = true;
    static int difficultyNum;
    [SerializeField] Text difficultyText;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();


        if (difficultyNum == 1)
        {
            difficultyText.text = "難易度:EASY";
        }
        if (difficultyNum == 3)
        {
            difficultyText.text = "難易度:HARD";
        }

        StartCoroutine(DelayStart());
    }

    private void Update()
    {
        if (isChange)
        {
            isChange = false;
            GameObject gd = GameObject.Find("GameDirector");
            gd.GetComponent<GameDirector>().DifficultySettingNormal();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isStart)
        {
            isStart = false;
            StartCoroutine(DelayTime());
        }

        if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
        {
            miniGame = 1;
        }
        else if (miniGame == 1 && (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)))
        {
            miniGame = 2;
        }
        else if (miniGame == 2 && (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)))
        {
            SceneManager.LoadScene("MiniGame");
        }
        else if (Input.anyKeyDown)
        {
            miniGame = 0;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            stageChange = 1;
        }
        else if (stageChange == 1 && Input.GetKeyDown(KeyCode.T))
        {
            stageChange = 2;
        }
        else if (stageChange == 2 && Input.GetKeyDown(KeyCode.A))
        {
            stageChange = 3;
        }
        else if (stageChange == 3 && Input.GetKeyDown(KeyCode.G))
        {
            stageChange = 4;
        }
        else if (stageChange == 4 && Input.GetKeyDown(KeyCode.E))
        {
            stageChange = 5;
        }
        else if (stageChange == 5 && (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)))
        {
            SceneManager.LoadScene("Stage2");
        }
        else if (stageChange == 5 && (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)))
        {
            SceneManager.LoadScene("Stage4");
        }
        else if (stageChange == 5 && (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)))
        {
            SceneManager.LoadScene("Stage5");
        }
        else if (Input.anyKeyDown)
        {
            stageChange = 0;
        }

        GameObject director = GameObject.Find("GameDirector");
        if (miniGame != 2 && (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)))
        {
            director.GetComponent<GameDirector>().DifficultySettingEasy();
            difficultyText.text = "難易度:EASY";

            difficultyNum = 1;
            audioSource.PlayOneShot(select);
        }
        if (stageChange != 5 && (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)))
        {
            director.GetComponent<GameDirector>().DifficultySettingNormal();
            difficultyText.text = "難易度:NORMAL";

            difficultyNum = 2;
            audioSource.PlayOneShot(select);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            director.GetComponent<GameDirector>().DifficultySettingHard();
            difficultyText.text = "難易度:HARD";

            difficultyNum = 3;
            audioSource.PlayOneShot(select);
        }
    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(0.5f);
        audioSource.Play();
    }
    IEnumerator DelayTime()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(sound);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Stage1");
    }
}
