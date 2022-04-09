using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectingTime4 : MonoBehaviour
{
    // àÍéûìIÇ…å∏è≠Çí‚é~Ç∑ÇÈbool
    public bool isStop;

    public int frame;

    float speed;
    float time;

    private void Start()
    {
        isStop = true;
        time = 1;

        GameObject director = GameObject.Find("GameDirector");
        director.GetComponent<GameDirector>().DifficultySet();

        speed = director.GetComponent<GameDirector>().fDifficulty2;
    }

    void Update()
    {
        if (isStop)
        {
            // webgl0.0085
            // 0.0035ìÔà’ìx:çÇ 0.003íÜ 0.002í·
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
            director.GetComponent<GameDirector>().StartCoroutine("DecreaseLife4");
            if (frame == 1)
            {
                GameObject qChage = GameObject.Find("Story4Manager");
                qChage.GetComponent<NewSelectQ4>().DamageChangeQ1();
            }
            if (frame == 2)
            {
                GameObject qChage = GameObject.Find("Story4Manager");
                qChage.GetComponent<NewSelectQ4>().DamageChangeQ2();
            }
        }
        if (time < 0 && director.GetComponent<GameDirector>().lifeNumber == 1)
        {
            director.GetComponent<GameDirector>().StartCoroutine("DecreaseLife4");
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
        GetComponent<Image>().fillAmount -= 0.2f;
        time -= 0.2f;
    }
}
