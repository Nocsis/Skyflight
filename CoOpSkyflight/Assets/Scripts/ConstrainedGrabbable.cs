using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ConstrainedGrabbable : OVRGrabbable
{
    [SerializeField]
    private Transform handle;
    Rigidbody handleRB;

    [SerializeField]
    private Transform original;
    Rigidbody originalRB;

    private bool grabbed;

    protected override void Start()
    {
        base.Start();
        handleRB = handle.GetComponent<Rigidbody>();

        if (original != null)
            originalRB = original.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!grabbed)
        {
            transform.position = handle.position;
            transform.rotation = handle.rotation;
        }
    }

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        base.GrabBegin(hand, grabPoint);
        StartCoroutine(UpdateHandle());
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        grabbed = false;
        
        base.GrabEnd(Vector3.zero, Vector3.zero);

        handleRB.velocity = Vector3.zero;
        handleRB.angularVelocity = Vector3.zero;

        if(original != null)
        {
            originalRB.velocity = Vector3.zero;
            originalRB.angularVelocity = Vector3.zero;

            handle.position = original.position;
        }
    }

    IEnumerator UpdateHandle()
    {
        grabbed = true;
        while (grabbed)
        {
            handleRB.MovePosition(transform.position);
            yield return null;
        }
    }
}
