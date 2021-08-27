using UnityEngine;

namespace MultiUserKit
{
    public class Ingameintercafehandler : MonoBehaviour
    {
        public GameObject theInterface, creditsBtn, backBtn, CreditsPlayer;

        public Transform subject, CreditsPosition;

        private Vector3 backPosition;

        void Start()
        {
            CreditsPosition = GameObject.FindGameObjectWithTag("CreditsPosition").transform;
            CreditsPlayer = GameObject.FindGameObjectWithTag("CreditsPlayer");

            creditsBtn.SetActive(true);
            backBtn.SetActive(false);
            SetInterfaceActive(false);
        }

        void Update()
        {
            if (InputManager.CheckInput(InputManager.InputTrigger.SecondaryLeft) || InputManager.CheckInput(InputManager.InputTrigger.SecondaryRight))
            {
                SetInterfaceActive(!theInterface.activeSelf);
            }

        }

        public void SetInterfaceActive(bool active)
        {
            theInterface.SetActive(active);
        }

        public void GotoCredits()
        {
            backPosition = subject.position;
            subject.position = CreditsPosition.position;

            CreditsPlayer.GetComponent<UnityEngine.Video.VideoPlayer>().Play();

            creditsBtn.SetActive(false);
            backBtn.SetActive(true);
            SetInterfaceActive(false);
        }

        public void returnFromCredits()
        {
            subject.position = backPosition;

            CreditsPlayer.GetComponent<UnityEngine.Video.VideoPlayer>().Stop();

            creditsBtn.SetActive(true);
            backBtn.SetActive(false);
            SetInterfaceActive(false);
        }

        public void Quit()
        {
            NetworkManager.singleton.StopClient();
        }
    }
}
