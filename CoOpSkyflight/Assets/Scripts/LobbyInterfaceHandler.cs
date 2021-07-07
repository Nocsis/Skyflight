using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyInterfaceHandler : MonoBehaviour
{
    public GameObject[] menu1, menu2, menu3, menu4;


    // Start is called before the first frame update
    void Start()
    {
        SetActiveMenu1(true);
        SetActiveMenu2(false);
        SetActiveMenu3(false);
        SetActiveMenu4(false);
    }

    public void SetActiveMenu1(bool active)
    {
        foreach (GameObject obj in menu1)
        {
            obj.SetActive(active);
        }
    }

    public void SetActiveMenu2(bool active)
    {
        foreach (GameObject obj in menu2)
        {
            obj.SetActive(active);
        }
    }

    public void SetActiveMenu3(bool active)
    {
        foreach (GameObject obj in menu3)
        {
            obj.SetActive(active);
        }
    }

    public void SetActiveMenu4(bool active)
    {
        foreach (GameObject obj in menu4)
        {
            obj.SetActive(active);
        }
    }

    public void Quit()
    {
        Application.Quit();
    } 
}
