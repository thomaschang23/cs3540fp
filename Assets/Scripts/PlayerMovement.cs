﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public float wSpeed = 2f;
    public float rSpeed = 5f;
    public float jSpeed = 5f;
    public float gravity = 20f;
    public Camera playerCam;
    public float sensitivity = 2.0f;
    public float vertLookLimit = 45.0f;
    public float dialogueTriggerDistance = 10.0f;

    AudioSource audioSource;


    CharacterController cc;
    Vector3 moveDir = Vector3.zero;
    float rX = 0;

    private static bool mouseUnlocked;

    private GameObject npc;

    private UIManager ui;

    [HideInInspector]
    public bool canMove = true;

    public Slider sens;

    void Start()
    {
        sens.value = sensitivity;
        ui = FindObjectOfType<UIManager>();
        cc = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

        // Lock cursor
        mouseUnlocked = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (npc != null && Input.GetButtonDown("Fire1"))
        {
            DialogueManager.TriggerDialogue(npc);
            npc = null;
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            mouseChange();
        }

        if (!mouseUnlocked)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float curX = canMove ? (isRunning ? rSpeed : wSpeed) * Input.GetAxis("Vertical") : 0;
            float curY = canMove ? (isRunning ? rSpeed : wSpeed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = moveDir.y;
            moveDir = (forward * curX) + (right * curY);

            if (Input.GetButtonDown("Jump") && canMove && cc.isGrounded)
            {
                moveDir.y = jSpeed;
            }
            else
            {
                moveDir.y = movementDirectionY;
            }

            if (!cc.isGrounded)
            {
                moveDir.y -= gravity * Time.deltaTime;
            }

            cc.Move(moveDir * Time.deltaTime);

            if (canMove)
            {
                rX += -Input.GetAxis("Mouse Y") * sensitivity;
                rX = Mathf.Clamp(rX, -vertLookLimit, vertLookLimit);
                playerCam.transform.localRotation = Quaternion.Euler(rX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * sensitivity, 0);
            }
        }
        else
        {
            moveDir = Vector3.zero;
            float movementDirectionY = moveDir.y;


            if (!cc.isGrounded)
            {
                moveDir.y -= gravity * Time.deltaTime;
            }

            cc.Move(moveDir * Time.deltaTime);
        }

        if (cc.velocity.magnitude > 1f)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.volume = Random.Range(0.8f, 1f);
                audioSource.pitch = Random.Range(0.8f, 1.1f);
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    public void changeSens()
    {
        sensitivity = sens.value;
    }

    public static void mouseChange(int newState = 0)
    {
        if (newState == 0)
        {
            mouseUnlocked = !mouseUnlocked;
            Cursor.visible = !Cursor.visible;

            if (mouseUnlocked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        else if (newState == -1)
        {
            mouseUnlocked = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (newState == 1)
        {
            mouseUnlocked = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void FixedUpdate()
    {
        if (!ui.IsDialogueOpen())
        {
            RaycastHit hit;
            Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
            if (
              Physics.Raycast(ray, out hit, dialogueTriggerDistance)
            )
            {
                if (hit.collider.CompareTag("DialogueNPC"))
				{
                    npc = hit.collider.gameObject;
                    FindObjectOfType<DialogueManager>().ShowDialogueHint();
                }
                // by tagging any object as "DialogueObject" and adding a DialogueTrigger script to the object,
                // function calls can be made by interacting with objects.
                // For example, this was done to the bed in Leslie's house. The player can click on the bed when close enough,
                // bringing up a the text, "Get some rest?", with the prompts, "Yes" and "Not yet". The "Yes" option will
                // call GameOver.NextDay().
                else if (hit.collider.CompareTag("DialogueObject"))
				{
                    npc = hit.collider.gameObject;
                    FindObjectOfType<DialogueManager>().ShowObjectDialogueHint();
                }
            }
            else
            {
                npc = null;
                FindObjectOfType<DialogueManager>().HideDialogueHint();
            }
        }
    }
}
