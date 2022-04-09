using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LimitFrame5 : MonoBehaviour
{
    // àÍéûìIÇ…å∏è≠Çí‚é~Ç∑ÇÈbool
    public bool isStop;

    float speed;
    float time;

    void Start()
    {
        isStop = false;
        time = 1;

        GameObject director = GameObject.Find("GameDirector");
        director.GetComponent<GameDirector>().DifficultySet();

        speed = director.GetComponent<GameDirector>().fDifficulty3;
    }

    void Update()
    {
        if (isStop)
        {
            // 0.001f
            GetComponent<Image>().fillAmount -= speed;
            time -= speed;
        }

        if (GetComponent<Image>().fillAmount < 0.13)
        {
            GetComponent<Image>().color = Color.red;
        }
        else if (GetComponent<Image>().fillAmount < 0.36)
        {
            GetComponent<Image>().color = Color.yellow;
        }

        GameObject director = GameObject.Find("GameDirector");
        if (time < 0 && director.GetComponent<GameDirector>().lifeNumber != 1)
        {
            director.GetComponent<GameDirector>().StartCoroutine("DecreaseLife5");
            GameObject qChage = GameObject.Find("Story5Manager");
            qChage.GetComponent<Stage5>().StartCoroutine("DamageChangeQ");
        }
        if (time < 0 && director.GetComponent<GameDirector>().lifeNumber == 1)
        {
            director.GetComponent<GameDirector>().StartCoroutine("DecreaseLife5");
            time = 100;
        }
    }

    public void UndoFrame()
    {
        GetComponent<Image>().fillAmount = 1;
        isStop = false;
        time = 1;
    }

    public void MissDamage()
    {
        GetComponent<Image>().fillAmount -= 0.1f;
        time -= 0.1f;
    }
}
