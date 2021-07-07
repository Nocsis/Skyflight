using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MultiUserKit
{
    public class TemporaryRoomSwitcher : MonoBehaviour
    {
        private Vector3 bridge, machineroom;
        public Transform subject;

        void Start()
        {
            if (subject == null)
                Debug.LogError("[TempTeleport] Add subject on " + gameObject.name);

            bridge = new Vector3(0, 0.3f, 0);
            machineroom = new Vector3(1, 11.2f, 14.3f);
        }

        private void Update()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            string sceneName = currentScene.name;

            if (sceneName == "Skyship_Bridge")
            {
                if (InputManager.CheckInput(InputManager.InputTrigger.SecondaryLeft))
                {
                    subject.position = bridge;
                }
                else if (InputManager.CheckInput(InputManager.InputTrigger.SecondaryRight))
                {
                    subject.position = machineroom;
                }
            }
            else
            {
                Debug.Log("Wrong scene");
            }
        }
    }
}
