using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreRankingScene : MonoBehaviour
{
    void Start()
    {
        GameObject count = GameObject.Find("ScoreManager");
        count.GetComponent<ScoreManager>().RankingSet();
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(count.GetComponent<ScoreManager>().rankingScore);
    }
}
