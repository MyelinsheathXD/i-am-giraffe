using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempWalker : MonoBehaviour
{
    new private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        
    }
}
