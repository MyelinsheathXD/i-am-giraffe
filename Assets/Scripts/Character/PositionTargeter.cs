using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTargeter : MonoBehaviour
{
    new private Rigidbody rigidbody;

    public Vector3 targetPos = Vector3.zero;

    [SerializeField]
    [Tooltip("Strength of force towards target position per axis")]
    private Vector3 pullForce = Vector3.zero;

    [SerializeField]
    [Tooltip("Apply target position relative to this object (use world position otherwise)")]
    private Transform inRelationTo = null;

    [SerializeField]
    [Tooltip("Rigidbody to apply counter force to keep system at net 0")]
    private Rigidbody counterForceRigidbody = null;

    [SerializeField]
    [Tooltip("Scales the counter force strength per axis")]
    private Vector3 counterForceScale = Vector3.one;

    [SerializeField]
    [Tooltip("Reduces force as object reaches target - helps prevent overshooting")]
    private Vector3 leadTime = Vector3.zero;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 pos = this.transform.position;
        Vector3 diff = Vector3.zero;
        Vector3 lead = rigidbody.velocity;
        lead.Scale(leadTime);

        if (inRelationTo != null)
        {
            diff = inRelationTo.TransformPoint(targetPos) - (pos + lead);
        }
        diff.Scale(pullForce);

        float dist = Mathf.Abs(diff.magnitude);
        float pullM = Mathf.Clamp01(dist / 0.3f);

        Vector3 impulse = (diff * pullM * Time.fixedDeltaTime);
        rigidbody.AddForceAtPosition(impulse, pos, ForceMode.Impulse);

        if (counterForceRigidbody != null)
        {
            Vector3 counterImpulse = -impulse;
            counterImpulse.Scale(counterForceScale);
            counterForceRigidbody.AddForce(counterImpulse, ForceMode.Impulse);
        }
    }
}
