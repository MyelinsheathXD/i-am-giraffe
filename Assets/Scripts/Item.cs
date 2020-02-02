using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type
    {
        PottedPlant,
        Teapot,
        Clock,
        Vase
    }

    [SerializeField]
    private float breakImpulse = 2f;

    [SerializeField]
    private GameObject brokenPrefab = null;

    private bool broken = false;

    private Rigidbody rigidBody = null;

    private Vector3 preVelocity;
    private Vector3 preAngularVelocity;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        preVelocity = rigidBody.velocity;
        preAngularVelocity = rigidBody.angularVelocity;
    }

    private void FixedUpdate()
    {
        preVelocity = rigidBody.velocity;
        preAngularVelocity = rigidBody.angularVelocity;
    }

    private void Break()
    {
        if (broken) return;
        broken = true;
        GameObject newObject = Instantiate(brokenPrefab, this.transform.position, this.transform.rotation);
        newObject.transform.localScale = this.transform.localScale;

        Rigidbody[] pieces = newObject.GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody p in pieces)
        {
            p.velocity = preVelocity;
            p.angularVelocity = preAngularVelocity;
        }

        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        float impulseMagnitude = collision.impulse.magnitude;
        if (impulseMagnitude >= breakImpulse)
        {
            Break();
        }
    }
}
