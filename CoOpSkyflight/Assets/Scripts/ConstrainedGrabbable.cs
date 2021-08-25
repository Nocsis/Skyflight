using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstrainedGrabbable : OVRGrabbable
{
    [SerializeField]
    private Transform handle;
    Rigidbody handleRB;
    private bool grabbed;

    protected override void Start()
    {
        base.Start();
        handleRB = handle.GetComponent<Rigidbody>();
    }

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        base.GrabBegin(hand, grabPoint);
        StartCoroutine(UpdateHandle());
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        grabbed = false;
        transform.position = handle.position;
        transform.rotation = handle.rotation;
        
        base.GrabEnd(Vector3.zero, Vector3.zero);

        handleRB.velocity = Vector3.zero;
        handleRB.angularVelocity = Vector3.zero;
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
