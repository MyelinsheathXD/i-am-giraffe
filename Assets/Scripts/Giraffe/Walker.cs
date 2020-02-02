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

    [SerializeField]
    private float turnSpeed = 45f;

    [SerializeField]
    private float inPlaceTurnSpeed = 45f;

    [Header("Keyframes")]

    [SerializeField]
    private TargetSet defaultStep = null;

    [Header("Forward")]

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

    [Header("In Place")]

    [SerializeField]
    private TargetSet frontStepUp = null;

    [SerializeField]
    private TargetSet backStepUp = null;

    new private Rigidbody rigidbody = null;
    private RigidbodyConstraints defaultConstraints = 0;

    private bool walking = false;
    private bool walkingInPlace = false;
    private bool walkBackwards = false;
    private bool[] takingStep = new bool[4];

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        defaultConstraints = rigidbody.constraints;
        StartCoroutine(WalkForward());
        StartCoroutine(WalkInPlace());
    }

    private void Update()
    {
        float hIn = Input.GetAxisRaw("Horizontal");
        float vIn = Input.GetAxisRaw("Vertical");
        if (hIn != 0 || vIn != 0)
        {
            if (vIn == 0)
            {
                torsoDirection.facingDirection = Quaternion.Euler(0, hIn * inPlaceTurnSpeed * Time.deltaTime, 0) * torsoDirection.facingDirection;
                walkingInPlace = true;
                walking = false;
            }
            else
            {
                torsoDirection.facingDirection = Quaternion.Euler(0, hIn * turnSpeed * Time.deltaTime, 0) * torsoDirection.facingDirection;
                walkBackwards = (vIn < 0);
                walking = true;
                walkingInPlace = false;
            }
        }
        else
        {
            walkingInPlace = false;
            walking = false;
        }

        foreach (Leg singleLeg in legs)
        {
            singleLeg.constantForce.force = Vector2.up * (walkingInPlace ? -50 : -1000);
        }

        this.rigidbody.constraints = defaultConstraints;
        if (walkingInPlace)
        {
            this.rigidbody.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }
    }

    private void FixedUpdate()
    {
        Vector3 forwardDir = body.transform.up;
        forwardDir.y = 0;
        forwardDir.Normalize();

        body.AddForceAtPosition(forwardDir * forwardForce * Time.fixedDeltaTime, body.transform.position + body.transform.up * 3f, ForceMode.Impulse);
    }

    private float stepOffset = 0.25f;
    private float delay = 0.10f;

    private IEnumerator WalkForward()
    {
        while (true)
        {
            yield return StartCoroutine(WaitForWalking());
            StartCoroutine(TakeStep(LegIndex.BackRight, walkBackwards));
            yield return new WaitForSeconds(stepOffset);

            yield return StartCoroutine(WaitForWalking());
            StartCoroutine(TakeStep(LegIndex.FrontRight, walkBackwards));
            yield return new WaitForSeconds(stepOffset);

            yield return StartCoroutine(WaitForWalking());
            StartCoroutine(TakeStep(LegIndex.BackLeft, walkBackwards));
            yield return new WaitForSeconds(stepOffset);

            yield return StartCoroutine(WaitForWalking());
            StartCoroutine(TakeStep(LegIndex.FrontLeft, walkBackwards));
            yield return new WaitForSeconds(stepOffset);
        }
    }

    private IEnumerator WalkInPlace()
    {
        while (true)
        {
            yield return StartCoroutine(WaitForWalkInPlace());
            StartCoroutine(TakeStepInPlace(LegIndex.BackRight));
            yield return new WaitForSeconds(stepOffset);

            yield return StartCoroutine(WaitForWalkInPlace());
            StartCoroutine(TakeStepInPlace(LegIndex.FrontRight));
            yield return new WaitForSeconds(stepOffset);

            yield return StartCoroutine(WaitForWalkInPlace());
            StartCoroutine(TakeStepInPlace(LegIndex.BackLeft));
            yield return new WaitForSeconds(stepOffset);

            yield return StartCoroutine(WaitForWalkInPlace());
            StartCoroutine(TakeStepInPlace(LegIndex.FrontLeft));
            yield return new WaitForSeconds(stepOffset);
        }
    }

    private IEnumerator TakeStep(LegIndex legIndex, bool backwards)
    {
        int legIndexValue = (int)legIndex;
        takingStep[legIndexValue] = true;
        if (LegIndex.FrontLeft == legIndex || LegIndex.FrontRight == legIndex)
        {
            SetTargets(legIndexValue, frontStepForward, backwards);
            yield return new WaitForSeconds(delay);
            SetTargets(legIndexValue, frontStepExtended, backwards);

            Leg leg = legs[legIndexValue];
            Vector3 point = body.transform.InverseTransformPoint(leg.ankle.transform.position);
            while (point.y < 3f && !backwards)
            {
                if (walkingInPlace)
                {
                    yield break;
                }
                yield return null;
                point = body.transform.InverseTransformPoint(leg.ankle.transform.position);
            }
            while (point.y > 0 && backwards)
            {
                if (walkingInPlace)
                {
                    yield break;
                }
                yield return null;
                point = body.transform.InverseTransformPoint(leg.ankle.transform.position);
            }

            SetTargets(legIndexValue, frontStepGrounded, backwards);
        }
        else
        {
            SetTargets(legIndexValue, backStepRetracted, backwards);
            yield return new WaitForSeconds(delay * 2f);
            SetTargets(legIndexValue, backStepForward, backwards);
            yield return new WaitForSeconds(delay);
            SetTargets(legIndexValue, backStepExtended, backwards);

            Leg leg = legs[legIndexValue];
            Vector3 point = body.transform.InverseTransformPoint(leg.ankle.transform.position);
            while (point.y < 0.6f && !backwards)
            {
                if (walkingInPlace)
                {
                    yield break;
                }
                yield return null;
                point = body.transform.InverseTransformPoint(leg.ankle.transform.position);
            }
            while (point.y > -1.5f && backwards)
            {
                if (walkingInPlace)
                {
                    yield break;
                }
                yield return null;
                point = body.transform.InverseTransformPoint(leg.ankle.transform.position);
            }

            SetTargets(legIndexValue, backStepGrounded, backwards);
            yield return new WaitForSeconds(delay);
            SetTargets(legIndexValue, backStepGroundedStraight, backwards);
        }
        takingStep[legIndexValue] = false;
    }

    private IEnumerator TakeStepInPlace(LegIndex legIndex)
    {
        int legIndexValue = (int)legIndex;
        takingStep[legIndexValue] = true;
        if (LegIndex.FrontLeft == legIndex || LegIndex.FrontRight == legIndex)
        {
            SetTargets(legIndexValue, frontStepUp);
            yield return new WaitForSeconds(delay * 5);
            SetTargets(legIndexValue, defaultStep);
        }
        else
        {
            SetTargets(legIndexValue, backStepUp);
            yield return new WaitForSeconds(delay * 5);
            SetTargets(legIndexValue, defaultStep);
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

    private IEnumerator WaitForWalkInPlace()
    {
        while (!walkingInPlace)
        {
            yield return null;
        }
    }

    private void SetTargets(int legIndex, TargetSet set, bool reverseHip = false)
    {
        Quaternion hipQuat = set.hip;
        if (reverseHip) hipQuat.x *= -1;

        Leg leg = legs[legIndex];
        leg.hip.targetRotation = hipQuat;
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
