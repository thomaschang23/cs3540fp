using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Note I used online aids in writing this code

public class Examine : MonoBehaviour
{

    Camera cam;
    GameObject targObj;

    Vector3 origPos;
    Vector3 origRot;

    private UIManager uiManager;

    bool examining;

    Text popup;

    static List<bool> bools = new List<bool>();
    int index;

    void Start()
    {
        bools.Add(false);
        index = bools.Count - 1;
        popup = GameObject.FindGameObjectWithTag("PopUpText").GetComponent<Text>();
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
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
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 3) && hit.transform.gameObject == this.gameObject)
        {
            if (Input.GetMouseButtonDown(0) && examining == false)
            {
                examining = uiManager.ToggleExamine();
                if (examining)
                {
                    bools[index] = true;
                    popup.text = "Right Click to Exit";
                    targObj = hit.transform.gameObject;

                    origPos = targObj.transform.position;
                    origRot = targObj.transform.rotation.eulerAngles;

                    targObj.transform.position = cam.transform.position + (cam.transform.forward);

                    Time.timeScale = 0;
                }
            }
            else if (examining == false)
            {
                bools[index] = true;
                popup.text = "Click to Examine";
            }
        }
        else if (!examining && popup.text == "Click to Examine")
        {
            bools[index] = false;
            if (!bools.Contains(true))
                popup.text = "";
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
            bools[index] = false;
            popup.text = "";
            examining = uiManager.ToggleExamine();
            targObj.transform.position = origPos;
            targObj.transform.eulerAngles = origRot;

            Time.timeScale = 1;

            examining = false;
        }
    }
}