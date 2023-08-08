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
    public GameObject card;
    [HideInInspector] public Card firstCard;
    [HideInInspector] public Card secondCard;

    public AudioClip match;
    public AudioSource audioSource;

    public float plusTime;
    public float minusTime;
    int comboCount = 0;

    private float time;
    private bool isGameStart = false;
    private int count = 16;

    public int totalScore = 0;
    public int plusScore = 1;

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        Time.timeScale = 1.0f;

        int[] nums = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        nums = nums.OrderBy(item => UnityEngine.Random.Range(-1.0f, 1.0f)).ToArray();

        for (int i = 0; i < 16; i++)
        {
            GameObject newCard = Instantiate(card, new Vector3((i / 4) * 1.4f - 2.1f, (i % 4) * 1.4f - 3.0f, 0), Quaternion.identity, GameObject.Find("cards").transform);

            // 이미지 설정
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
                GameOver();
            }
            else if (time <= 10)
            {
                timeText.color = Color.red;
            }
        }
    }

    // 처음 시작시 카드 앞면 전체 공개
    private void ShowAllCards()
    {
        foreach (Transform card in GameObject.Find("cards").transform)
        {
            card.GetComponent<Card>().FlipCard(true);
        }

        Invoke("ShowAllCardsInvoke", 5f);       // 난이도에 따라 시간 다르게
    }

    private void ShowAllCardsInvoke()
    {
        foreach (Transform card in GameObject.Find("cards").transform)
        {
            card.GetComponent<Card>().FlipCard(false);
        }

        isGameStart = true;
    }

    public void CheckMatch()
    {
        if (firstCard.num == secondCard.num)
        {
            audioSource.PlayOneShot(match);

            nameText.text = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
            time += plusTime;
            if (comboCount < 3) comboCount++;

            firstCard.destroyCard();
            secondCard.destroyCard();
            Invoke("nameTextReset", 1.0f);

            addScore(plusScore + comboCount - 1);

            count -= 2;
            if (count == 0)
                Invoke("GameClear", 1.0f);
        }
        else
        {
            nameText.text = "매칭 실패";
            nameText.color = Color.red;
            comboCount = 0;
            firstCard.closeCard();
            secondCard.closeCard();
            Invoke("nameTextReset", 1.0f);
            time -= minusTime;
        }

        firstCard = null;
        secondCard = null;
    }

    private void nameTextReset()
    {
        nameText.text = "???";
        nameText.color = Color.white;
    }

    private void GameOver()
    {
        endPanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    private void GameClear()
    {
        endPanel.SetActive(true);
        Time.timeScale = 0f;

        resultText.text = "성공 !";
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
