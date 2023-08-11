using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    public Text timeText;
    public Text scoreText;
    public Text nameText;
    public GameObject endPanel;
    public Text resultText;
    public Text bestScoreText;
    public Text nowScoreText;
    public Transform field;
    public GameObject card;
    [HideInInspector] public Card firstCard;
    [HideInInspector] public Card secondCard;
    
    private AudioSource audioSource;
    public AudioClip audioMatchSuccess;
    public AudioClip audioMatchFail;
    public AudioClip audioGameClear;
    public AudioClip audioGameOver;

    private float time;
    private bool isGameStart = false;
    private bool isClear = false;
    private int cardCount = 12;
    private float showTime = 5f;

    public int totalScore = 0;
    public int plusScore = 1;
    private int comboCount = 0;

    public float plusTime = 5f;
    public float minusTime = 5f;

    private void Awake()
    {
        I = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Time.timeScale = 1.0f;

        List<int> nums = new List<int>() { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 7, 8, 9 };

        for (int i = 0; i < 16; i++)
        {
            GameObject newCard = Instantiate(card, new Vector3((i / 4) * 1.4f - 2.1f, (i % 4) * 1.4f - 3.0f, 0), Quaternion.identity, field);

            int cardIndex = Random.Range(0, nums.Count);

            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("image" + nums[cardIndex].ToString());
            newCard.GetComponent<Card>().num = nums[cardIndex];

            nums.RemoveAt(cardIndex);
        }

        if      (LevelManager.s_difficulty == 2) { time = 25f; showTime = 4f; plusScore = 2; }
        else if (LevelManager.s_difficulty == 3) { time = 20f; showTime = 3f; plusScore = 3; }
        else                                     { time = 30f; showTime = 5f; plusScore = 1; }      // else를 기본으로 한 이유는 혹시 모를 오류 방지

        ShowAllCards();
    }

    private void Update()
    {
        if (isGameStart)
        {
            time -= Time.deltaTime;
            timeText.text = time.ToString("N2");

            if (time < 0)
            {
                timeText.text = "0";
                isClear = false;
                GameEnd();
            }
            else if (time <= 10)
            {
                timeText.color = Color.red;
            }
            else
            {
                timeText.color = Color.white;
            }
        }
    }

    // 처음 시작시 카드 앞면 전체 공개
    private void ShowAllCards()
    {
        foreach (Transform card in field)
        {
            card.GetComponent<Card>().FlipCard(true);
        }

        Invoke(nameof(ShowAllCardsInvoke), showTime);
    }

    private void ShowAllCardsInvoke()
    {
        foreach (Transform card in field)
        {
            card.GetComponent<Card>().FlipCard(false);
        }

        isGameStart = true;
    }

    public void CheckMatch()
    {
        if (firstCard.num == secondCard.num)
        {
            firstCard.Matched();
            secondCard.Matched();

            audioSource.PlayOneShot(audioMatchSuccess);

            switch (firstCard.num)
            {
                case 0:
                case 1:
                    nameText.text = "김형중";
                    break;
                case 2:
                case 3:
                    nameText.text = "김민상";
                    break;
                case 4:
                case 5:
                    nameText.text = "김하늘";
                    break;
            }

            Invoke(nameof(NameTextReset), 1.0f);
            
            time += plusTime;

            AddScore(plusScore + comboCount);
            if (comboCount < 3) comboCount++;

            cardCount -= 2;
            if (cardCount == 0)
            {
                isClear = true;
                Invoke(nameof(GameEnd), 1.0f);
            }
        }
        else
        {
            firstCard.CloseCard();
            secondCard.CloseCard();

            audioSource.PlayOneShot(audioMatchFail);

            nameText.text = "매칭 실패";
            nameText.color = Color.red;
            Invoke(nameof(NameTextReset), 1.0f);
            
            time -= minusTime;

            comboCount = 0;
        }

        firstCard = null;
        secondCard = null;
    }

    private void NameTextReset()
    {
        nameText.text = "???";
        nameText.color = Color.white;
    }

    private void GameEnd()
    {
        endPanel.SetActive(true);
        Time.timeScale = 0f;
        isGameStart = false;

        if (isClear)
        {
            resultText.text = "성공 !";
            audioSource.PlayOneShot(audioGameClear);

            totalScore += (int)System.Math.Floor(time);
        }
        else
        {
            resultText.text = "실패 ...";
            audioSource.PlayOneShot(audioGameOver);
        }

        nowScoreText.text = "이번기록 : " + totalScore;

        if (PlayerPrefs.GetInt("bestScore", 0) < totalScore)
            PlayerPrefs.SetInt("bestScore", totalScore);

        bestScoreText.text = "최고기록 : " + PlayerPrefs.GetInt("bestScore");
    }

    public void ReTry()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void AddScore(int score)
    {
        totalScore += score;
        scoreText.text = totalScore.ToString();
    }
}
