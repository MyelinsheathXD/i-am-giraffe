using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform[] targets = null;

    private Vector3 offset = Vector3.zero;
    private Vector3 smoothDampVelocity = Vector3.zero;

    private void Start()
    {
        offset = this.transform.position - GetAverageTargetPosition();
    }

    private void LateUpdate()
    {
        Vector3 targetPos = GetAverageTargetPosition() + offset;
        this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPos, ref smoothDampVelocity, 0.3f);
    }

    private Vector3 GetAverageTargetPosition()
    {
        Vector3 avgTargetPos = Vector3.zero;
        foreach (Transform t in targets)
        {
            avgTargetPos += t.position;
        }
        avgTargetPos /= (float)targets.Length;
        avgTargetPos.y = 0;
        return avgTargetPos;
    }
}
