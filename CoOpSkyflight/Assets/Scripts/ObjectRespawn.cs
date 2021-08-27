using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ObjectRespawn : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 offset = Vector3.zero;

    [Tooltip("The GameObjects that should trigger respawning on collission")]
    public GameObject[] RespawnColliders;

    [SerializeField]
    private GameObject elevator;

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        if(elevator != null) //oh boy, this is dirty
        {
            if (elevator.TryGetComponent<elevator_move>(out elevator_move elevatorScript))
                offset = elevatorScript.upPosition.position - elevatorScript.downPosition.position;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (GameObject obj in RespawnColliders)
        {
            foreach (Collider col in obj.GetComponents(typeof(Collider)))
            {
                if (GameObject.ReferenceEquals(collision.collider, col))
                {
                    if (gameObject.TryGetComponent<OVRGrabbable>(out OVRGrabbable grabbable))
                    {
                        if (grabbable.isGrabbed)
                        {
                            return;
                        }
                    }
                    //Debug.Log("Respawned");
                    if(elevator != null)
                    {
                        if(elevator.TryGetComponent<elevator_move>(out elevator_move elevatorScript))
                        {
                            if (elevatorScript._state == elevator_move.ElevatorState.Up)
                                transform.position = originalPosition + offset;
                            else
                                transform.position = originalPosition;
                        }
                    }
                    else
                    {
                        transform.position = originalPosition;
                    }

                    transform.rotation = originalRotation;

                    Rigidbody r = transform.GetComponent<Rigidbody>();
                    r.velocity = Vector3.zero;
                    r.angularVelocity = Vector3.zero;
                }
            }
        }
    }
}
