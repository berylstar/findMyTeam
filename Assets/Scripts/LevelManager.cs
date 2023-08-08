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

    private int openNormal = 10;
    private int openHard = 20;

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

    public void ButtonLevel(float timeLimit)
    {
        SceneManager.LoadScene("MainScene");
        PlayerPrefs.SetFloat("timeLimit", timeLimit);
    }
}
