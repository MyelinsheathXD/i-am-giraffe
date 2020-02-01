using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestImpulse : MonoBehaviour
{
    [SerializeField]
    private Vector3 impulse = Vector3.zero;

    new private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody.AddForce(impulse, ForceMode.Impulse);
            Debug.Log("Applied " + impulse + " to " + rigidbody.gameObject.name);
        }
    }
}
