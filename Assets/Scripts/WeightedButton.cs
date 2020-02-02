using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedButton : MonoBehaviour
{
    [SerializeField]
    private Color defaultColor = Color.black;

    [SerializeField]
    private GameObject[] lights;

    [SerializeField]
    private GameObject cage;

    private Color[] originalColors;

    private int count = 0;

    private void Awake()
    {
        originalColors = new Color[lights.Length];
        for (int i = 0; i < lights.Length; i++)
        {
            originalColors[i] = lights[i].GetComponent<Renderer>().material.color;
            lights[i].GetComponent<Renderer>().material.color = defaultColor;
        }
        StartCoroutine(Activate());
    }

    private IEnumerator Activate()
    {
        float targetHeight = 13f;
        while (cage.transform.position.y < targetHeight)
        {
            cage.transform.position += Vector3.up * 4f * Time.deltaTime;
            yield return null;
        }
    }

    private void Add()
    {
        if (count >= lights.Length) return;
        lights[count].GetComponent<Renderer>().material.color = originalColors[count];
        count++;
        if (count == lights.Length) StartCoroutine(Activate());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            Add();
        }
    }
}
