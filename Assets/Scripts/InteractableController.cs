﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableController : MonoBehaviour
{
    public AudioClip doorSFX;
    public float interactableDistance = 3f;
    //public GameObject hand;
    public float smooth = 5;

    private bool holding;
    private GameObject heldObject;
    private KeyCode interactKey = KeyCode.E;
    private Text pickUpText;
    private Text doorText;

    private Text examineText;
    private GameObject examineObject;
    private bool examining;
    private UIManager ui;
    private Vector3 origPosition;
    private Vector3 origRotation;

    private Text noteAddedText;

    private void Start()
	{
        pickUpText = GameObject.FindGameObjectWithTag("PickUpPopUpText").GetComponent<Text>();
        doorText = GameObject.FindGameObjectWithTag("DoorPopUpText").GetComponent<Text>();
        examineText = GameObject.FindGameObjectWithTag("ExaminePopUpText").GetComponent<Text>();
        ui = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        noteAddedText = GameObject.FindGameObjectWithTag("NoteAddedText").GetComponent<Text>();
    }

	void Update()
    {
        updateExamine();
        updatePickup();
        updateDoor();
    }

    private void updateDoor()
	{
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, interactableDistance))
        {
            if (hit.collider.CompareTag("Door"))
            {
                if (doorText != null)
                    doorText.text = "E to interact";
                if (Input.GetKeyDown(interactKey))
                {
                    GameObject door = hit.collider.gameObject;
                    door.GetComponent<DoorController>().toggleDoor();

                    AudioSource.PlayClipAtPoint(doorSFX, door.transform.position);
                }
            }
        }
        else
        {
            if (doorText != null)
                doorText.text = "";
        }
    }

    private void updateExamine()
    {
        RaycastHit hit;

        if (examining)
        {
            examineText.text = "Left click and drag to rotate. Right click to Exit";
            if (Input.GetMouseButton(0))
            {
                float rotSpeed = 15;

                float xAxis = Input.GetAxis("Mouse X") * rotSpeed;
                float yAxis = Input.GetAxis("Mouse Y") * -rotSpeed;

                examineObject.transform.Rotate(Vector3.up, -xAxis, Space.World);
                examineObject.transform.Rotate(Vector3.right, yAxis, Space.World);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                examineText.text = "";
                examining = ui.ToggleExamine();
                examineObject.transform.position = origPosition;
                examineObject.transform.eulerAngles = origRotation;

                Time.timeScale = 1;

                examining = false;
            }
        }
        else
		{
            if (Physics.Raycast(transform.position, transform.forward, out hit, interactableDistance))
			{
                if (hit.collider.CompareTag("Examine"))
                {
                    examineText.text = "Click to Examine";
                    if (Input.GetMouseButton(0))
                    {
                        examining = ui.ToggleExamine();
                        examineObject = hit.transform.gameObject;

                        origPosition = examineObject.transform.position;
                        origRotation = examineObject.transform.rotation.eulerAngles;

                        examineObject.transform.position = gameObject.transform.position + (gameObject.transform.forward);

                        Time.timeScale = 0;

                        ExamineFlag examineFlag = hit.transform.GetComponent<ExamineFlag>();
                        if (!examineFlag.flagId.Equals(""))
						{
                            FlagManager.SetFlag(examineFlag.flagId, examineFlag.flagNote);

                            if (!examineFlag.flagNote.Equals(""))
                            {
                                noteAddedText.enabled = true;
                                Invoke("DisableNoteAddedText", 5f);
                            }
                        }
                    }
                }
            }
            else
			{
                examineText.text = "";

            }
        }
    }

    private void DisableNoteAddedText()
	{
        noteAddedText.enabled = false;
	}

    private void updatePickup()
	{
        RaycastHit hit;

        if (holding)
        {
            heldObject.transform.position = //Vector3.Lerp(heldObject.transform.position,
                                            //Camera.main.transform.position + Camera.main.transform.forward * 1.2f, Time.deltaTime * smooth);
                Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
            pickUpText.text = "E to drop " + heldObject.name;
            if (Input.GetKeyDown(interactKey))
            {
                holding = false;
                heldObject.GetComponent<Rigidbody>().isKinematic = false;
                heldObject = null;
            }
        }
        else
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, interactableDistance))
            {
                if (hit.collider.CompareTag("Pickup"))
                {
                    pickUpText.text = "E to pickup " + hit.collider.name;
                    if (Input.GetKeyDown(interactKey))
                    {
                        holding = true;
                        heldObject = hit.collider.gameObject;
                        heldObject.GetComponent<Rigidbody>().isKinematic = true;
                    }
                }

                
            }
            else
            {
                pickUpText.text = "";
            }
        }
    }
}
