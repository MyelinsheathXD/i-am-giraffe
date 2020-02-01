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
    private Leg[] legs;

    [Header("Keyframes")]

    [SerializeField]
    private TargetSet idle = null;

    [SerializeField]
    private TargetSet raised = null;

    [SerializeField]
    private TargetSet raisedExtended = null;

    private void Start()
    {
        StartCoroutine(WalkingQuestionMark());
    }

    private IEnumerator WalkingQuestionMark()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            SetTargets((int)LegIndex.FrontLeft, raised);
            SetTargets((int)LegIndex.BackRight, raised);
            SetTargets((int)LegIndex.FrontRight, idle);
            SetTargets((int)LegIndex.BackLeft, idle);

            yield return new WaitForSeconds(0.5f);
            SetTargets((int)LegIndex.FrontLeft, idle);
            SetTargets((int)LegIndex.BackRight, idle);
            SetTargets((int)LegIndex.FrontRight, raised);
            SetTargets((int)LegIndex.BackLeft, raised);
        }
    }

    private void SetTargets(int legIndex, TargetSet set)
    {
        Leg leg = legs[legIndex];
        leg.hip.targetRotation = set.hip;
        leg.knee.targetRotation = set.knee;
        leg.ankle.targetRotation = set.ankle;
    }

    [System.Serializable]
    private class TargetSet
    {
        public Quaternion hip = Quaternion.identity;
        public Quaternion knee = Quaternion.identity;
        public Quaternion ankle = Quaternion.identity;
    }

    [System.Serializable]
    private class Leg
    {
        public ConfigurableJoint hip;
        public ConfigurableJoint knee;
        public ConfigurableJoint ankle;
    }
}
