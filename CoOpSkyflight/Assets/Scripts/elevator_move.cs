using System.Collections;
using UnityEngine;

public class elevator_move : MonoBehaviour
{
    private Transform elevator, railing;

    [SerializeField]
    public Transform downPosition, upPosition;

    private Transform railingMidLeft, railingMidRight;
    [SerializeField] private Transform rotMidLeftClosed, rotMidLeftOpen, rotMRightClosed, rotMidRightOpen;

    public enum ElevatorState {Up, Down, Moving};
    [SerializeField] public ElevatorState _state;

    [SerializeField] private float speed = 1;

    private void Awake()
    {
        elevator = this.transform;
        railing = transform.Find("Slide railing");
        railingMidLeft = railing.Find("slide_mid_left");
        railingMidRight = railing.Find("slide_mid_right");

        railingMidLeft.rotation = rotMidLeftOpen.rotation;
        railingMidRight.rotation = rotMidRightOpen.rotation;
    }

    public void Move()
    {
        if (_state == ElevatorState.Down)
        {
            _state = ElevatorState.Moving;
            StartCoroutine(MoveUpwards());
        }
        else if (_state == ElevatorState.Up)
        {
            _state = ElevatorState.Moving;
            StartCoroutine(MoveDownwards());
        }
    }

    private IEnumerator MoveUpwards()
    {
        while (railingMidLeft.rotation != rotMidLeftClosed.rotation)
        {
            railingMidLeft.rotation = Quaternion.Lerp(railingMidLeft.rotation, rotMidLeftClosed.rotation, speed * Time.deltaTime);
            railingMidRight.rotation = Quaternion.Lerp(railingMidRight.rotation, rotMRightClosed.rotation, speed * Time.deltaTime);
            yield return null;
        }

        while (Vector3.Distance(upPosition.position, elevator.position) > 0.01)
        {
            elevator.position = Vector3.Lerp(elevator.position, upPosition.position, speed * Time.deltaTime);
            yield return null;
        }

        while (railingMidLeft.rotation != rotMidLeftOpen.rotation)
        {
            railingMidLeft.rotation = Quaternion.Lerp(railingMidLeft.rotation, rotMidLeftOpen.rotation, speed * Time.deltaTime);
            railingMidRight.rotation = Quaternion.Lerp(railingMidRight.rotation, rotMidRightOpen.rotation, speed * Time.deltaTime);
            yield return null;
        }

        _state = ElevatorState.Up;
    }

    private IEnumerator MoveDownwards()
    {
        while (railingMidLeft.rotation != rotMidLeftClosed.rotation)
        {
            railingMidLeft.rotation = Quaternion.Lerp(railingMidLeft.rotation, rotMidLeftClosed.rotation, speed * Time.deltaTime);
            railingMidRight.rotation = Quaternion.Lerp(railingMidRight.rotation, rotMRightClosed.rotation, speed * Time.deltaTime);
            yield return null;
        }

        while (Vector3.Distance(downPosition.position, elevator.position) > 0.01)
        {
            elevator.position = Vector3.Lerp(elevator.position, downPosition.position, speed * Time.deltaTime);
            yield return null;
        }

        while (railingMidLeft.rotation != rotMidLeftOpen.rotation)
        {
            railingMidLeft.rotation = Quaternion.Lerp(railingMidLeft.rotation, rotMidLeftOpen.rotation, speed * Time.deltaTime);
            railingMidRight.rotation = Quaternion.Lerp(railingMidRight.rotation, rotMidRightOpen.rotation, speed * Time.deltaTime);
            yield return null;
        }

        _state = ElevatorState.Down;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.name == "NetworkUserPrefab(Clone)") //dirtyyyyyyyyyyyyy way to find player collider
        {
            other.transform.root.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.name == "NetworkUserPrefab(Clone)")
        {
            other.transform.root.parent = null;
        }
    }
}
