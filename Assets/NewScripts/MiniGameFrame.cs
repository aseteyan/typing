using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameFrame : MonoBehaviour
{
    // àÍéûìIÇ…å∏è≠Çí‚é~Ç∑ÇÈbool
    public bool isStop;

    public int frame;

    float time;

    public float fillSpeed;

    private void Start()
    {
        isStop = false;
        fillSpeed = 0.005f;
        time = 1;
    }

    void Update()
    {
        if (isStop)
        {
            // 0.0035ìÔà’ìx:çÇ 0.003íÜ 0.002í·
            GetComponent<Image>().fillAmount -= fillSpeed;
            time -= fillSpeed;
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
            director.GetComponent<GameDirector>().StartCoroutine("MiniGameLife");
            if (frame == 1)
            {
                GameObject qChage = GameObject.Find("MiniGameManager");
                qChage.GetComponent<MiniGameManager>().DamageChangeQ1();
            }
            if (frame == 2)
            {
                GameObject qChage = GameObject.Find("MiniGameManager");
                qChage.GetComponent<MiniGameManager>().DamageChangeQ2();
            }
        }
        if (time < 0 && director.GetComponent<GameDirector>().lifeNumber == 1)
        {
            director.GetComponent<GameDirector>().StartCoroutine("MiniGameLife");
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
        GetComponent<Image>().fillAmount -= 1f;
        time -= 1f;
    }
}
