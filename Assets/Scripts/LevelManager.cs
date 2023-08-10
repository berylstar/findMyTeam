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

    public static float s_difficulty = 1;       // static 변수

    // 난이도 해금하기 위한 점수
    private readonly int openNormal = 20;       // readonly
    private readonly int openHard = 45;

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

    public void ButtonLevel(int level)          // 버튼에서 인풋 받기
    {
        SceneManager.LoadScene("MainScene");
        s_difficulty = level;
    }
}
