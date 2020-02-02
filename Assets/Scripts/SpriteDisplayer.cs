using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteDisplayer : MonoBehaviour
{
    [SerializeField]
    private Image image = null;

    [SerializeField]
    private Sprite initialSprite = null;

    [SerializeField]
    private Sprite initialSprite2 = null;

    private Sprite displaySprite1 = null;
    private Sprite displaySprite2 = null;

    private float timeToHide = 0;

    private void Start()
    {
        StartCoroutine(Run());

        if (initialSprite != null)
        {
            DisplaySprite(initialSprite, initialSprite2);
        }
    }

    private void Update()
    {
        if (Time.time > timeToHide)
        {
            image.enabled = false;
        }
    }

    private IEnumerator Run()
    {
        while(true)
        {
            image.sprite = displaySprite1;
            yield return new WaitForSeconds(0.5f);
            image.sprite = displaySprite2;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void DisplaySprite(Sprite spr1, Sprite spr2)
    {
        image.sprite = spr1;
        image.enabled = true;
        timeToHide = Time.time + 10f;

        displaySprite1 = spr1;
        displaySprite2 = spr2;
    }
}
