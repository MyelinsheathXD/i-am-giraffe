using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDirection : MonoBehaviour
{
    new private Rigidbody rigidbody;

    [SerializeField]
    private Vector3 bodyForward = new Vector3(0, 0, 2);

    public Vector3 facingDirection = Vector3.zero;

    [SerializeField]
    private float facingForce = 800;

    [SerializeField]
    private float leadTime = 0;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        facingDirection = facingDirection.normalized;
        bodyForward = bodyForward.normalized;

        if (leadTime == 0)
        {
            if (facingDirection != Vector3.zero)
            {
                Vector3 force = facingForce * facingDirection * Time.fixedDeltaTime;
                Vector3 forcePosition = rigidbody.transform.TransformDirection(bodyForward);
                rigidbody.AddForceAtPosition(force, forcePosition, ForceMode.Impulse);
                rigidbody.AddForceAtPosition(-force, -forcePosition, ForceMode.Impulse);
            }
        }
        else
        {
            Vector3 targetPoint = transform.position + facingDirection * bodyForward.magnitude;
            Vector3 currentPoint = transform.TransformPoint(bodyForward);
            Vector3 reversePoint = transform.TransformPoint(-bodyForward);
            Vector3 velocity = rigidbody.GetPointVelocity(currentPoint);

            Vector3 diff = targetPoint - (currentPoint + velocity * leadTime);

            rigidbody.AddForceAtPosition(facingForce * diff * Time.fixedDeltaTime, currentPoint, ForceMode.Impulse);
            rigidbody.AddForceAtPosition(-facingForce * diff * Time.fixedDeltaTime, reversePoint, ForceMode.Impulse);
        }
    }
}
