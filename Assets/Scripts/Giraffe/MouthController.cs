using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthController : MonoBehaviour
{
    private bool open = false;

    private FixedJoint joint = null;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            open = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            open = false;
            Release();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (open)
        {
            Grab(collision.rigidbody);
        }
    }

    private void Grab(Rigidbody other)
    {
        Debug.Log(other.name);
        if (joint == null)
        {
            joint = this.gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = other;
        }
    }

    private void Release()
    {
        if (joint != null)
        {
            Destroy(joint);
            joint = null;
        }
    }
}
