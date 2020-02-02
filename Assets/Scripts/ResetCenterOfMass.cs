using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCenterOfMass : MonoBehaviour
{
    private void Awake()
    {
        Rigidbody rbody = GetComponent<Rigidbody>();
        if (rbody != null)
        {
            rbody.ResetCenterOfMass();
        }
    }
}
