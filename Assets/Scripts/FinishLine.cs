using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [Header("Fanfare")]

    [SerializeField]
    private ParticleSystem[] confettiSystems = null;

    [SerializeField]
    private float timeToPauseConfetti = 4.5f;

    private bool triggered = false;
    private HashSet<GameObject> crossedLine = new HashSet<GameObject>();
    private bool allTargetItemsCrossed = false;
    private AudioSource applause = null;

    private void Start()
    {
        if (LevelManager.Instance.MainItems.Length == 0) allTargetItemsCrossed = true;

        applause = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Item i = other.attachedRigidbody.GetComponent<Item>();
        if (i != null)
        {
            crossedLine.Add(i.gameObject);
            Debug.Log(i.gameObject.name + "Crossed the line");

            bool finished = true;
            foreach(Item targetItem in LevelManager.Instance.MainItems)
            {
                if (!crossedLine.Contains(targetItem.gameObject))
                {
                    finished = false;
                    break;
                }
            }
            allTargetItemsCrossed = finished;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (triggered) return;

        Giraffe n = other.transform.root.GetComponent<Giraffe>();
        if (n != null && allTargetItemsCrossed)
        {
            triggered = true;
            StartCoroutine(Confetti());
        }
    }

    private IEnumerator Confetti()
    {
        applause.Play();

        foreach(ParticleSystem ps in confettiSystems)
        {
            ps.Play();
        }
        yield return new WaitForSeconds(4.5f);
        foreach (ParticleSystem ps in confettiSystems)
        {
            ps.Pause();
        }

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(LevelManager.Instance.nextLevel);
    }
}
