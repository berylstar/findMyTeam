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

    public bool isMatched = false;
    public float plusTime;
    public float minusTime;
    public float comboPoint;

    private float time;
    private bool isGameStart = false;
    private int count = 16;
    private int bestScore;

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        Time.timeScale = 1.0f;

        int[] rtans = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        rtans = rtans.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        for (int i = 0; i < 16; i++)
        {
            GameObject newCard = Instantiate(card, new Vector3((i / 4) * 1.4f - 2.1f, (i % 4) * 1.4f - 3.0f, 0), Quaternion.identity, GameObject.Find("cards").transform);

            // 이미지 설정
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("rtan" + rtans[i].ToString());
            newCard.GetComponent<Card>().num = rtans[i];
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
                GameEnd();
            }
        }
    }

    private void ShowAllCards()
    {
        foreach (Transform card in GameObject.Find("cards").transform)
        {
            card.GetComponent<Card>().FlipCard(true);
        }

        Invoke("ShowAllCardsInvoke", 5f);       // 난이도에 따라 시간 다르게 ?
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
            isMatched = true;

            firstCard.destroyCard();
            secondCard.destroyCard();
            Invoke("nameTextReset", 1.0f);

            count -= 2;
            if (count == 0)
                Invoke("GameEnd", 1.0f);
        }
        else
        {
            firstCard.closeCard();
            secondCard.closeCard();
            time -= minusTime;
        }

        firstCard = null;
        secondCard = null;
    }

    private void nameTextReset()
    {
        nameText.text = "???";
    }

    private void GameEnd()
    {
        endPanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void ReGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
