using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Button buttonEasy;
    public Button buttonNormal;
    public Button buttonHard;

    public static float s_TimeLimit = 30f;
    public static float s_showTime = 5f;

    // 난이도 해금하기 위한 점수
    private int openNormal = 20;
    private int openHard = 45;

    private void Start()
    {
        if (PlayerPrefs.HasKey("bestScore"))
        {
            int bestScore = PlayerPrefs.GetInt("bestScore");

            if (bestScore >= openNormal)
                buttonNormal.interactable = true;

            if (bestScore >= openHard)
                buttonHard.interactable = true;
        }
    }

    public void ButtonLevel(int level)
    {
        SceneManager.LoadScene("MainScene");
        
        if (level == 1) { s_showTime = 30f; s_showTime = 5f; }
        else if (level == 2) { s_showTime = 25f; s_showTime = 4f; }
        else if (level == 3) { s_showTime = 20f; s_showTime = 3f; }
    }
}
