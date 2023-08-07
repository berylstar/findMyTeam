using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int num = 0;
    public AudioClip flip;

    public GameObject front;
    public GameObject back;

    private Animator anim;
    private AudioSource audioScource;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioScource = GetComponent<AudioSource>();
    }

    public void OpenCard()
    {
        anim.SetBool("isOpen", true);
        audioScource.PlayOneShot(flip);

        front.SetActive(true);
        back.SetActive(false);

        if (GameManager.I.firstCard == null)
            GameManager.I.firstCard = this;
        else
        {
            GameManager.I.secondCard = this;
            GameManager.I.CheckMatch();
        }
    }

    public void destroyCard()
    {
        Invoke("destroyCardInvoke", 1.0f);
    }

    private void destroyCardInvoke()
    {
        Destroy(this.gameObject);
    }

    public void closeCard()
    {
        Invoke("closeCardInvoke", 1.0f);
    }

    private void closeCardInvoke()
    {
        anim.SetBool("isOpen", false);

        back.SetActive(true);
        front.SetActive(false);
    }

    public void FlipCard(bool fb)
    {
        anim.SetBool("isOpen", fb);
        front.SetActive(fb);
        back.SetActive(!fb);
    }
}
