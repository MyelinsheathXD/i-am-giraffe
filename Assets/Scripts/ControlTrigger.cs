using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTrigger : MonoBehaviour
{
    [SerializeField]
    private SpriteDisplayer spriteDisplayer = null;

    [SerializeField]
    private Sprite spr1 = null;

    [SerializeField]
    private Sprite spr2 = null;

    private void OnTriggerEnter(Collider other)
    {
        spriteDisplayer.DisplaySprite(spr1, spr2);
        Destroy(this.gameObject);
    }
}
