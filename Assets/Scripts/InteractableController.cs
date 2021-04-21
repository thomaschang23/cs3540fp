using System.Collections;
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

    private void Start()
	{
        pickUpText = GameObject.FindGameObjectWithTag("PickUpPopUpText").GetComponent<Text>();
        doorText = GameObject.FindGameObjectWithTag("DoorPopUpText")?.GetComponent<Text>();
    }

	void Update()
    {
        RaycastHit hit;
     
        if (holding)
        {
            heldObject.transform.position = //Vector3.Lerp(heldObject.transform.position,
                //Camera.main.transform.position + Camera.main.transform.forward * 1.2f, Time.deltaTime * smooth);
                Camera.main.transform.position + Camera.main.transform.forward * 1.2f;
            pickUpText.text = "E to drop " + heldObject.name;
            if(Input.GetKeyDown(interactKey))
			{
                Debug.Log("drop");
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
                        Debug.Log("pickup");
                        holding = true;
                        heldObject = hit.collider.gameObject;
                        heldObject.GetComponent<Rigidbody>().isKinematic = true;
                    }
                }
                else
                {
                    pickUpText.text = "";
                }
            }
            else //if (pickUpText.text != "Click to Examine")
            {
                pickUpText.text = "";
            }
        }


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
            else
            {
                if (doorText != null)
                    doorText.text = "";
            }
        }
        else
        {
            if (doorText != null)
                doorText.text = "";
        }
    }
}
