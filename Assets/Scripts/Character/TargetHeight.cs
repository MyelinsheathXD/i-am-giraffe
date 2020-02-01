using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHeight : MonoBehaviour
{
    [SerializeField]
    private float targetHeight = 1f;

    [SerializeField]
    private float strength = 0f;

    [SerializeField]
    private float leadTime = 0.3f;

    [SerializeField]
    private Transform inRelationTo = null;

    [SerializeField]
    private LayerMask mask = 0;

    new private Rigidbody rigidbody = null;
    private float groundHeight = 0;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 100f, mask))
        {
            groundHeight = hit.point.y;
        }

        float diff = (groundHeight + targetHeight) - (this.transform.position.y + rigidbody.velocity.y * leadTime);
        if (inRelationTo != null) {
            diff = inRelationTo.TransformPoint(Vector3.up * targetHeight).y - (transform.position.y + rigidbody.velocity.y * leadTime);
        }

        float distance = Mathf.Abs(diff);
        float pullM = Mathf.Clamp01(distance / 0.3f);
        rigidbody.AddForce(new Vector3(0, Mathf.Sign(diff) * strength * pullM * Time.fixedDeltaTime, 0), ForceMode.Impulse);
    }
}
