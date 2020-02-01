using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempWalker : MonoBehaviour
{
    [SerializeField]
    private Rigidbody[] rigidbodies = null;

    [SerializeField]
    private FaceDirection facer = null;

    private Vector3 moveDir;

    private void Update()
    {
        float hIn = Input.GetAxisRaw("Horizontal");
        float vIn = Input.GetAxisRaw("Vertical");
        moveDir = new Vector3(hIn, 0, vIn).normalized;
        //Vector3 facingDir = new Vector3(hIn, 0, vIn);
    }

    private void FixedUpdate()
    {
        Vector3 dir = this.transform.up;
        dir.y = 0;
        dir.Normalize();

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.MovePosition(rb.position + moveDir * 3f * Time.fixedDeltaTime);
        }
    }
}
