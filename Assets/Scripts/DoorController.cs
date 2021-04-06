using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    Animator doorAnimator;
    private bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        doorAnimator = transform.parent.GetComponent<Animator>();
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (isOpen)
		{
            doorAnimator.SetBool("doorState", true);
		}
        else
		{
            doorAnimator.SetBool("doorState", false);
        }
        
    }

    public void toggleDoor() => isOpen = isOpen ? false : true;
}
