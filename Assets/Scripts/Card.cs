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

        if (GameManager.I.firstCard == null)
            GameManager.I.firstCard = this;
        else
        {
            GameManager.I.secondCard = this;
            GameManager.I.CheckMatch();
        }
    }

    public void DestroyCard()
    {
        Invoke("DestroyCardInvoke", 1.0f);
    }

    private void DestroyCardInvoke()
    {
        Destroy(this.gameObject);
    }

    public void CloseCard()
    {
        Invoke("CloseCardInvoke", 1.0f);
    }

    private void CloseCardInvoke()
    {
        anim.SetBool("isOpen", false);
    }

    // true : ¾Õ¸é / false : µÞ¸é
    public void FlipCard(bool fb)
    {
        anim.SetBool("isOpen", fb);
    }
}
