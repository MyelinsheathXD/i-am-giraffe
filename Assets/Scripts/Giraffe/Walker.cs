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

    private void Start()
    {
        StartCoroutine(WalkingQuestionMark());
    }

    private void FixedUpdate()
    {
        body.AddForceAtPosition(Vector3.forward * forwardForce * Time.fixedDeltaTime, body.transform.position + body.transform.up * 3f, ForceMode.Impulse);
    }

    private float delay = 0.15f;

    private IEnumerator WalkingQuestionMark()
    {
        yield return new WaitForSeconds(3.0f);
        while (true)
        {
            yield return new WaitForSeconds(delay);
            SetTargets((int)LegIndex.BackRight, backStepRetracted);
            yield return new WaitForSeconds(delay);
            SetTargets((int)LegIndex.BackRight, backStepForward);
            yield return new WaitForSeconds(delay);
            SetTargets((int)LegIndex.BackRight, backStepExtended);
            yield return new WaitForSeconds(delay);
            SetTargets((int)LegIndex.BackRight, backStepGrounded);

            yield return new WaitForSeconds(delay);
            SetTargets((int)LegIndex.FrontRight, frontStepForward);
            yield return new WaitForSeconds(delay);
            SetTargets((int)LegIndex.FrontRight, frontStepExtended);
            yield return new WaitForSeconds(delay);
            SetTargets((int)LegIndex.FrontRight, frontStepGrounded);

            yield return new WaitForSeconds(delay);
            SetTargets((int)LegIndex.BackLeft, backStepRetracted);
            yield return new WaitForSeconds(delay);
            SetTargets((int)LegIndex.BackLeft, backStepForward);
            yield return new WaitForSeconds(delay);
            SetTargets((int)LegIndex.BackLeft, backStepExtended);
            yield return new WaitForSeconds(delay);
            SetTargets((int)LegIndex.BackLeft, backStepGrounded);

            yield return new WaitForSeconds(delay);
            SetTargets((int)LegIndex.FrontLeft, frontStepForward);
            yield return new WaitForSeconds(delay);
            SetTargets((int)LegIndex.FrontLeft, frontStepExtended);
            yield return new WaitForSeconds(delay);
            SetTargets((int)LegIndex.FrontLeft, frontStepGrounded);
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
