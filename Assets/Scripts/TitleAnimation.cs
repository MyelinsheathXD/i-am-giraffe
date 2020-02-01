using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleAnimation : MonoBehaviour
{
    [SerializeField]
    private Sprite frame2 = null;

    [SerializeField]
    private float frameTime = 1f;

    private Image image = null;
    private Sprite frame1 = null;
    private WaitForSeconds wait = null;

    private void Start()
    {
        image = this.GetComponent<Image>();
        frame1 = image.sprite;
        wait = new WaitForSeconds(frameTime);
    }

    private void OnEnable()
    {
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        while (true)
        {
            yield return wait;
            image.sprite = (image.sprite == frame1) ? frame2 : frame1;
        }
    }
}
