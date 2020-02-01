using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giraffe : MonoBehaviour
{
    private static Giraffe s_instance;
    public static Giraffe Instance
    {
        get { return s_instance; }
    }

    [System.NonSerialized]
    public int numHats;

    private void Awake()
    {
        s_instance = this;
    }
}
