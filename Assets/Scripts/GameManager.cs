using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
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
    public AudioClip match;
    public AudioSource audioSource;

    private float time;
    private bool isGameStart = false;
    private bool isClear = false;
    private int cardCount = 12;

    public int totalScore = 0;
    public int plusScore = 1;
    private int comboCount = 0;

    public float plusTime;
    public float minusTime;

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        Time.timeScale = 1.0f;

        int[] nums = { 0, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 9};
        nums = nums.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        for (int i = 0; i < 16; i++)
        {
            GameObject newCard = Instantiate(card, new Vector3((i / 4) * 1.4f - 2.1f, (i % 4) * 1.4f - 3.0f, 0), Quaternion.identity, field);

            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("image" + nums[i].ToString());
            newCard.GetComponent<Card>().num = nums[i];
        }

        if (PlayerPrefs.HasKey("timeLimit"))
            time = PlayerPrefs.GetFloat("timeLimit");
        else
            time = 30f;

        ShowAllCards();
    }

    private void Update()
    {
        if (isGameStart)
        {
            time -= Time.deltaTime;
            timeText.text = time.ToString("N2");

            if (time <= 0)
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

        Invoke("ShowAllCardsInvoke", 5f);       // 난이도에 따라 시간 다르게
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
        if (firstCard.num == secondCard.num && firstCard.num >= 2)
        {
            firstCard.DestroyCard();
            secondCard.DestroyCard();

            audioSource.PlayOneShot(match);

            nameText.text = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
            Invoke("nameTextReset", 1.0f);
            
            time += plusTime;

            if (comboCount < 3) comboCount++;
            addScore(plusScore + comboCount - 1);

            cardCount -= 2;
            if (cardCount == 0)
            {
                isClear = true;
                Invoke("GameEnd", 1.0f);
            }
        }
        else
        {
            firstCard.CloseCard();
            secondCard.CloseCard();

            nameText.text = "매칭 실패";
            nameText.color = Color.red;
            Invoke("nameTextReset", 1.0f);
            
            time -= minusTime;

            comboCount = 0;
        }

        firstCard = null;
        secondCard = null;
    }

    private void nameTextReset()
    {
        nameText.text = "???";
        nameText.color = Color.white;
    }

    private void GameEnd()
    {
        endPanel.SetActive(true);
        Time.timeScale = 0f;

        if (isClear)
            resultText.text = "성공 !";
        else
            resultText.text = "실패 ...";

        nowScoreText.text = "이번기록 : " + totalScore;

        if (PlayerPrefs.HasKey("bestScore"))
        {
            if (PlayerPrefs.GetInt("bestScore") < totalScore)
            {
                PlayerPrefs.SetInt("bestScore", totalScore);
            }
        }
        else
        {
            PlayerPrefs.SetInt("bestScore", totalScore);
        }

        bestScoreText.text = "최고기록 : " + PlayerPrefs.GetInt("bestScore");
    }

    public void ReTry()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void addScore(int score)
    {
        totalScore += score;
        scoreText.text = totalScore.ToString();
    }
}
