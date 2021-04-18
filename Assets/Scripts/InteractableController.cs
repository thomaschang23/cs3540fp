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
    private Text popup;

    private void Start()
	{
        popup = GameObject.FindGameObjectWithTag("PopUpText").GetComponent<Text>();
    }

	void Update()
    {
        RaycastHit hit;
     
        if (holding)
        {
            heldObject.transform.position = Vector3.Lerp(heldObject.transform.position,
                Camera.main.transform.position + Camera.main.transform.forward * 1.25f, Time.deltaTime * smooth);

            popup.text = "E to drop";
            if(Input.GetKeyDown(interactKey))
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
                    popup.text = "E to pickup";
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
                popup.text = "";
			}
        }


        if (Physics.Raycast(transform.position, transform.forward, out hit, interactableDistance))
        {
            if (hit.collider.CompareTag("Door"))
            {
                popup.text = "E to interact";
                if (Input.GetKeyDown(interactKey))
                {
                    GameObject door = hit.collider.gameObject;
                    door.GetComponent<DoorController>().toggleDoor();

                    AudioSource.PlayClipAtPoint(doorSFX, door.transform.position);
                }
            }
            else
			{
                popup.text = "";
            }

        }
    }
}
