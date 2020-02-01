using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForceApplier : MonoBehaviour
{
    [SerializeField]
    private Vector3 force = Vector3.zero;

    new private Rigidbody rigidbody = null;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddForce(force * Time.fixedDeltaTime);
        }
    }
}
