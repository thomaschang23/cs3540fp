using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    public AudioClip doorSFX;
    public float interactableDistance = 3f;

    private KeyCode interactKey = KeyCode.E;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, interactableDistance))
        {
            if (hit.collider.CompareTag("Door"))
            {
                if (Input.GetKeyDown(interactKey))
                {
                    GameObject door = hit.collider.gameObject;
                    door.GetComponent<DoorController>().toggleDoor();

                    AudioSource.PlayClipAtPoint(doorSFX, door.transform.position);
                }
            }
        }
    }
}
