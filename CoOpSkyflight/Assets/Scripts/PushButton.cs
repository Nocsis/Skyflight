using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButton : MonoBehaviour
{
    protected enum Directions
    {
        forward,
        back,
        left,
        right,
        up,
        down
    };

    [SerializeField] protected Directions pushDirection = Directions.back;
    [SerializeField] protected float pushDepth = 0.01f;

    [SerializeField] protected bool pressed = false;

    protected Vector3 originalPos;
    protected Vector3 pushedPos;

    protected virtual void Awake()
    {
        originalPos = transform.position;

        if (pushDirection == Directions.back)
            pushedPos = originalPos - transform.forward * pushDepth;
        else if (pushDirection == Directions.forward)
            pushedPos = originalPos + transform.forward * pushDepth;
        else if (pushDirection == Directions.left)
            pushedPos = originalPos - transform.right * pushDepth;
        else if (pushDirection == Directions.right)
            pushedPos = originalPos + transform.right * pushDepth;
        else if (pushDirection == Directions.down)
            pushedPos = originalPos - transform.up * pushDepth;
        else if (pushDirection == Directions.up)
            pushedPos = originalPos + transform.up * pushDepth;
    }

    public void OnClick()
    {
        StartCoroutine(Push());
    }

    protected virtual void ChangePressedState()
    {
        pressed = !pressed;
    }

    protected IEnumerator Push()
    {
        while (Vector3.Distance(transform.position, pushedPos) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pushedPos, pushDepth * 10 * Time.deltaTime);
            yield return null;
        }

        ChangePressedState();

        while (Vector3.Distance(transform.position, originalPos) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPos, pushDepth * 10 * Time.deltaTime);
            yield return null;
        }
    }
}
