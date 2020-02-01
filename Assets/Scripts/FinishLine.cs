using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [Header("Fanfare")]

    [SerializeField]
    private ParticleSystem[] confettiSystems = null;

    [SerializeField]
    private float timeToPauseConfetti = 4.5f;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        triggered = true;
        StartCoroutine(Confetti());
    }

    private IEnumerator Confetti()
    {
        foreach(ParticleSystem ps in confettiSystems)
        {
            ps.Play();
        }
        yield return new WaitForSeconds(4.5f);
        foreach (ParticleSystem ps in confettiSystems)
        {
            ps.Pause();
        }
    }
}
