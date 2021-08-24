using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSize : MonoBehaviour
{
    GameObject cylinderTable;
    Vector3 tableHeight;
    // Start is called before the first frame update
    void Start()
    {
        cylinderTable = GameObject.Find("cylinderTable");
        tableHeight = cylinderTable.transform.position + new Vector3(0, 2, 0);
    }

    public void btnChangeHeight()
    {
        StartCoroutine(moveCoroutine());
    }

    IEnumerator moveCoroutine()
    {
        while(Vector3.Distance(cylinderTable.transform.position, tableHeight) >0.1f)
            cylinderTable.transform.position = Vector3.MoveTowards(cylinderTable.transform.position, tableHeight, Time.deltaTime);
        yield return null;
    }
}
