using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Examine : MonoBehaviour
{

    Camera cam;
    GameObject targObj;

    Vector3 origPos;
    Vector3 origRot;

    bool examining;

    void Start()
    {
        cam = Camera.main;
        examining = false;
    }

    void Update()
    {
        ExamineObj();
        RotateObj();
        QuitExamine();
    }


    void ExamineObj()
    {
        if (Input.GetMouseButtonDown(0) && examining == false)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject == this.gameObject)
            {
                PlayerMovement.mouseChange();
                targObj = hit.transform.gameObject;

                origPos = targObj.transform.position;
                origRot = targObj.transform.rotation.eulerAngles;

                targObj.transform.position = cam.transform.position + (cam.transform.forward);

                Time.timeScale = 0;

                examining = true;
            }
        }
    }

    void RotateObj()
    {
        if (Input.GetMouseButton(0) && examining)
        {
            float rotSpeed = 15;

            float xAxis = Input.GetAxis("Mouse X") * rotSpeed;
            float yAxis = Input.GetAxis("Mouse Y") * rotSpeed;

            targObj.transform.Rotate(Vector3.up, -xAxis, Space.World);
            targObj.transform.Rotate(Vector3.right, yAxis, Space.World);
        }
    }

    void QuitExamine()
    {
        if (Input.GetMouseButtonDown(1) && examining)
        {
            PlayerMovement.mouseChange();
            targObj.transform.position = origPos;
            targObj.transform.eulerAngles = origRot;

            Time.timeScale = 1;

            examining = false;
        }
    }
}