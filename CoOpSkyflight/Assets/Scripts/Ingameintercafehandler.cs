using UnityEngine;

namespace MultiUserKit
{
    public class Ingameintercafehandler : MonoBehaviour
    {
        public GameObject theInterface, creditsBtn, backBtn;

        public Transform subject;

        public Vector3 CreditsPosition;
        private Vector3 backPosition;

        void Start()
        {
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
            subject.position = CreditsPosition;
            
            creditsBtn.SetActive(false);
            backBtn.SetActive(true);
            SetInterfaceActive(false);
        }

        public void returnFromCredits()
        {
            subject.position = backPosition;

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
