using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ObjectRespawn : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    [Tooltip("The GameObjects that should trigger respawning on collission")]
    public GameObject[] RespawnColliders;

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
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
                    transform.position = originalPosition;
                    transform.rotation = originalRotation;
                }
            }
        }
    }
}
