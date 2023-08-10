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
        // Alt + Enter : 개선사항 보기
        Invoke(nameof(DestroyCard), 1.0f);          // 유지보수 위해 함수 이름 노출
    }

    private void DestroyCard()
    {
        anim.SetTrigger("isDestroy");
        Invoke(nameof(DestroyCardInvoke), 1.0f);
    }

    private void DestroyCardInvoke()
    {
        Destroy(this.gameObject);
    }

    public void CloseCard()
    {
        Invoke(nameof(CloseCardInvoke), 1.0f);
    }

    private void CloseCardInvoke()
    {
        anim.SetBool("isOpen", false);
    }

    // true : 앞면 / false : 뒷면
    public void FlipCard(bool fb)
    {
        anim.SetBool("isOpen", fb);
        audioScource.PlayOneShot(flip);
    }
}
