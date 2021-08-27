using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MultiUserKit
{
    public class FollowingInterface : MonoBehaviour
    {
        Transform _head;
        [SerializeField]
        float lerpSpeed = 3;
        [SerializeField]
        float rotateAngle = 20;

        int currentAngle;
        float oldAngle;
        float rotationFraction;

        private void Start()
        {
            _head = LocalUser.CameraTransform;
        }

        void Update()
        {
            transform.position = Vector3.Slerp(transform.position, _head.position, lerpSpeed * Time.deltaTime);

            for (int x = 0; x < 4; x++)
            {
                if (_head.rotation.eulerAngles.y > (x * 90 - rotateAngle) && _head.rotation.eulerAngles.y < (x * 90 + rotateAngle) && currentAngle != x * 90)
                {
                    currentAngle = x * 90;
                    Rotate(currentAngle);
                }
            }
        }

        void Rotate(float targetRotation)
        {
            oldAngle = targetRotation;
            rotationFraction = 0;
            StartCoroutine(RotateCoroutine(targetRotation));
        }

        IEnumerator RotateCoroutine(float targetRotation)
        {
            rotationFraction += Time.deltaTime * lerpSpeed;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, targetRotation, 0), rotationFraction);

            yield return new WaitForEndOfFrame();
            if (rotationFraction < 1 && targetRotation == oldAngle)
                StartCoroutine(RotateCoroutine(targetRotation));
            else
                transform.rotation = Quaternion.Euler(0, targetRotation, 0);
        }
    }
}
