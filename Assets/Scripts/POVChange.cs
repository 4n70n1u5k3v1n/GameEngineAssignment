using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class POVChange : MonoBehaviour
{
    [SerializeField] bool firstPerson;
    [SerializeField] GameObject cam1;
    [SerializeField] GameObject cam2;

    // Start is called before the first frame update
    void Start()
    {
        firstPerson = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (firstPerson)
            {
                cam1.SetActive(false);
                cam2.SetActive(true);
                firstPerson = false;
            }
            else
            {
                cam2.SetActive(false);
                cam1.SetActive(true);
                firstPerson = true;
            }
        }
    }
}
