using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    private enum LegIndex : int
    {
        FrontLeft = 0,
        FrontRight = 1,
        BackLeft = 2,
        BackRight = 3
    }

    [SerializeField]
    private Leg[] legs = null;

    [SerializeField]
    private Rigidbody body = null;

    [SerializeField]
    private float forwardForce = 0f;

    [SerializeField]
    private FaceDirection torsoDirection = null;

    [Header("Keyframes")]

    [SerializeField]
    private TargetSet frontStepForward = null;

    [SerializeField]
    private TargetSet frontStepExtended = null;

    [SerializeField]
    private TargetSet frontStepGrounded = null;

    [SerializeField]
    private TargetSet backStepRetracted = null;

    [SerializeField]
    private TargetSet backStepForward = null;

    [SerializeField]
    private TargetSet backStepExtended = null;

    [SerializeField]
    private TargetSet backStepGrounded = null;

    [SerializeField]
    private TargetSet backStepGroundedStraight = null;

    private bool walking = false;
    private bool[] takingStep = new bool[4];

    private void Start()
    {
        StartCoroutine(WalkingQuestionMark());
    }

    private void Update()
    {
        float hIn = Input.GetAxis("Horizontal");
        float vIn = Input.GetAxis("Vertical");
        if (hIn != 0 || vIn != 0)
        {
            torsoDirection.facingDirection = new Vector3(hIn, 0, vIn);
            walking = true;
        }
        else
        {
            walking = false;
        }
    }

    private void FixedUpdate()
    {
        Vector3 forwardDir = body.transform.up;
        forwardDir.y = 0;
        forwardDir.Normalize();

        body.AddForceAtPosition(forwardDir * forwardForce * Time.fixedDeltaTime, body.transform.position + body.transform.up * 3f, ForceMode.Impulse);
    }

    private float delay = 0.10f;

    private IEnumerator WalkingQuestionMark()
    {
        while (true)
        {
            yield return StartCoroutine(WaitForWalking());
            yield return TakeStep(LegIndex.BackRight);
            yield return new WaitForSeconds(delay * 0.05f);

            yield return StartCoroutine(WaitForWalking());
            yield return TakeStep(LegIndex.FrontRight);
            yield return new WaitForSeconds(delay * 0.05f);

            yield return StartCoroutine(WaitForWalking());
            yield return TakeStep(LegIndex.BackLeft);
            yield return new WaitForSeconds(delay * 0.05f);

            yield return StartCoroutine(WaitForWalking());
            yield return TakeStep(LegIndex.FrontLeft);
            yield return new WaitForSeconds(delay * 0.05f);
        }
    }

    private IEnumerator TakeStep(LegIndex legIndex)
    {
        int legIndexValue = (int)legIndex;
        takingStep[legIndexValue] = true;
        if (LegIndex.FrontLeft == legIndex || LegIndex.FrontRight == legIndex)
        {
            SetTargets(legIndexValue, frontStepForward);
            yield return new WaitForSeconds(delay);
            SetTargets(legIndexValue, frontStepExtended);

            Leg leg = legs[legIndexValue];
            Vector3 point = body.transform.InverseTransformPoint(leg.ankle.transform.position);
            while (point.y < 3f)
            {
                yield return null;
                point = body.transform.InverseTransformPoint(leg.ankle.transform.position);
            }

            SetTargets(legIndexValue, frontStepGrounded);
        }
        else
        {
            SetTargets(legIndexValue, backStepRetracted);
            yield return new WaitForSeconds(delay * 2f);
            SetTargets(legIndexValue, backStepForward);
            yield return new WaitForSeconds(delay);
            SetTargets(legIndexValue, backStepExtended);

            Leg leg = legs[legIndexValue];
            Vector3 point = body.transform.InverseTransformPoint(leg.ankle.transform.position);
            while (point.y < 0.6f)
            {
                yield return null;
                point = body.transform.InverseTransformPoint(leg.ankle.transform.position);
            }

            SetTargets(legIndexValue, backStepGrounded);
            yield return new WaitForSeconds(delay);
            SetTargets(legIndexValue, backStepGroundedStraight);
        }
        takingStep[legIndexValue] = false;
    }

    private IEnumerator WaitForNotTakingStep(LegIndex legIndex)
    {
        int legIndexValue = (int)legIndex;
        while (takingStep[legIndexValue])
        {
            yield return null;
        }
    }

    private IEnumerator WaitForWalking()
    {
        while (!walking)
        {
            yield return null;
        }
    }

    private void SetTargets(int legIndex, TargetSet set)
    {
        Leg leg = legs[legIndex];
        leg.hip.targetRotation = set.hip;
        leg.knee.targetRotation = set.knee;
        leg.ankle.targetRotation = set.ankle;
        leg.constantForce.enabled = set.useConstantForce;
    }

    [System.Serializable]
    private class TargetSet
    {
        public Quaternion hip = Quaternion.identity;
        public Quaternion knee = Quaternion.identity;
        public Quaternion ankle = Quaternion.identity;
        public bool useConstantForce = false;
    }

    [System.Serializable]
    private class Leg
    {
        public ConfigurableJoint hip = null;
        public ConfigurableJoint knee = null;
        public ConfigurableJoint ankle = null;
        public ConstantForce constantForce = null;
    }
}
