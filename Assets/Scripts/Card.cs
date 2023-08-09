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
        FlipCard(true);

        if (GameManager.I.firstCard == null)
            GameManager.I.firstCard = this;
        else
        {
            GameManager.I.secondCard = this;
            GameManager.I.CheckMatch();
        }
    }

    public void Matched()
    {
        Invoke("DestroyCard", 1.0f);
    }

    private void DestroyCard()
    {
        anim.SetTrigger("isDestroy");
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

    // true : �ո� / false : �޸�
    public void FlipCard(bool fb)
    {
        anim.SetBool("isOpen", fb);
        audioScource.PlayOneShot(flip);
    }
}
