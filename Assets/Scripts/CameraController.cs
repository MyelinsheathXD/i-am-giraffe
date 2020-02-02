using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform followBack = null;

    [SerializeField]
    private FaceDirection faceDirection = null;

    [SerializeField]
    private float followDistance = 5f;

    [SerializeField]
    private float followHeight = 5f;

    [SerializeField]
    private float lookAngle = 45f;
    private Vector3 offset = Vector3.zero;
    private Vector3 smoothDampVelocity = Vector3.zero;
    private Vector3 smoothDampForward = Vector3.zero;

    private void Start()
    {
        
    }

    private void LateUpdate()
    {
        Vector3 targetForward = faceDirection.facingDirection;

        Vector3 targetPos = followBack.position - (targetForward * followDistance) + (Vector3.up * followHeight);
        this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPos, ref smoothDampVelocity, 0.3f);

        this.transform.forward = Vector3.SmoothDamp(this.transform.forward, targetForward, ref smoothDampForward, 0.3f);

        Vector3 eulerAngles = this.transform.eulerAngles;
        eulerAngles.x = lookAngle;
        this.transform.eulerAngles = eulerAngles;
    }
}
