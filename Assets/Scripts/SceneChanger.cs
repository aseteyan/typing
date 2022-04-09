using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    void Start()
    {
        GameObject gameManager = GameObject.Find("GameDirector");
        gameManager.GetComponent<GameDirector>().StartSceneManager();
    }

    void Update()
    {
        GameObject gameManager = GameObject.Find("GameDirector");
        gameManager.GetComponent<GameDirector>().StartTypingScene();
    }
}
