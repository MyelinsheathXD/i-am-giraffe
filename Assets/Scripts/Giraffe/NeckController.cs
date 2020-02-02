using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeckController : MonoBehaviour
{
    [SerializeField]
    private PositionTargeter headPosTargeter = null;

    [SerializeField]
    private Vector3 upPos = Vector3.zero;

    [SerializeField]
    private Vector3 downPos = Vector3.zero;

    [SerializeField]
    private Vector3 leftPos = Vector3.zero;

    [SerializeField]
    private Vector3 rightPos = Vector3.zero;

    [SerializeField]
    private float speed = 0.01f;

    [SerializeField]
    private Vector3 pos = new Vector2(0, 1);
    private Vector3 lastMousePos = Vector2.zero;

    private void Start()
    {
        lastMousePos = Input.mousePosition;
    }

    private void Update()
    {
        float hIn = Input.GetAxisRaw("Mouse X");
        float vIn = Input.GetAxisRaw("Mouse Y");

        pos += new Vector3(hIn, vIn, 0) * speed;
        pos.x = Mathf.Clamp(pos.x, -1, 1);
        pos.y = Mathf.Clamp(pos.y, -1, 1);
        pos.z = Mathf.Clamp(pos.z, -1, 1);

        Vector3 vertical = Vector3.Lerp(downPos, upPos, (pos.y + 1f) / 2f);
        Vector3 horizontal = Vector3.Lerp(leftPos, rightPos, (pos.x + 1f) / 2f);
        Vector3 targetPos = vertical + horizontal;
        headPosTargeter.targetPos = targetPos;
    }
}
