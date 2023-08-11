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

    public static float s_difficulty = 1;

    // 난이도 해금하기 위한 점수
    private readonly int openNormal = 20;
    private readonly int openHard = 45;

    private void Start()
    {
        PlayerPrefs.SetInt("bestScore", 0);

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
        s_difficulty = level;
    }
}