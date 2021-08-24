using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickSwitch : MonoBehaviour
{
    protected enum Axes
    {
        X,
        Y,
        Z
    };

    [SerializeField] protected Axes rotationAxis = Axes.X;
    
    [SerializeField] protected float turnDegrees = 30f;

    [Tooltip("Place GameObject in inactive state. If active=true, rotation will be changed by turnDegrees on Awake.")]
    [SerializeField] protected bool active = false;

    [SerializeField] protected float flickSpeed = 10;

    protected Quaternion originalRot;
    protected Quaternion activeRot;



    protected virtual void Awake()
    {
        originalRot = transform.rotation;

        if (rotationAxis == Axes.X)
            activeRot = originalRot * Quaternion.Euler(turnDegrees, 0, 0);
        else if (rotationAxis == Axes.Y)
            activeRot = originalRot * Quaternion.Euler(0, turnDegrees, 0);
        else if (rotationAxis == Axes.Z)
            activeRot = originalRot * Quaternion.Euler(0, 0, turnDegrees);

    }

    public void OnClick()
    {
        StartCoroutine(Flick());
    }

    protected virtual void ChangeActiveState()
    {
        active = !active;
    }

    protected IEnumerator Flick()
    {
        if (!active)
        {
            while (Quaternion.Angle(transform.rotation, activeRot) > 0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, activeRot, flickSpeed * Time.deltaTime);
                yield return null;
            }
        }
        else
        {
            while (Quaternion.Angle(transform.rotation, originalRot) > 0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, originalRot, flickSpeed * Time.deltaTime);
                yield return null;
            }
        }

        ChangeActiveState();
    }
}
