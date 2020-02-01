using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type
    {
        PottedPlant,
        Teapot,
        Clock,
        Vase
    }

    [SerializeField]
    private float breakImpulse = 2f;

    private void Break()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        float impulseMagnitude = collision.impulse.magnitude;
        if (impulseMagnitude >= breakImpulse)
        {
            Break();
        }
    }
}
