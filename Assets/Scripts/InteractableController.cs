using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    public AudioClip doorSFX;
    public float interactableDistance = 3f;
    //public GameObject hand;
    public float smooth = 5;

    bool holding;
    GameObject heldObject;
    private KeyCode interactKey = KeyCode.E;


    void Update()
    {
        RaycastHit hit;
     
        if (holding)
        {
            heldObject.transform.position = Vector3.Lerp(heldObject.transform.position,
                Camera.main.transform.position + Camera.main.transform.forward, Time.deltaTime * smooth);
            
            if(Input.GetKeyDown(KeyCode.E))
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
                if (Input.GetKeyDown(interactKey))
                {
                    if (hit.collider.CompareTag("Pickup"))
                    {
                        holding = true;
                        heldObject = hit.collider.gameObject;
                        heldObject.GetComponent<Rigidbody>().isKinematic = true;
                    }
                }

            }
        }


        if (Physics.Raycast(transform.position, transform.forward, out hit, interactableDistance))
        {
            if (Input.GetKeyDown(interactKey))
			{
                if (hit.collider.CompareTag("Door"))
                {
                        GameObject door = hit.collider.gameObject;
                        door.GetComponent<DoorController>().toggleDoor();

                        AudioSource.PlayClipAtPoint(doorSFX, door.transform.position);
                }
            }
                
        }
    }
}
