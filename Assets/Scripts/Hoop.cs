using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoop : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem confetti = null;

    private void OnTriggerEnter(Collider other)
    {
        confetti.Play();
    }
}
