using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleHorseDoorSound : MonoBehaviour
{
    public AudioClip sfx;
    private GameObject player;
    private float timeout = 0.0f;
    private bool clipPlayed = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (clipPlayed)
        {
            if (timeout > 0.2f)
            {
                clipPlayed = false;
                timeout = 0;
            }
            else
            {
                timeout += Time.deltaTime;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!clipPlayed && other.CompareTag("Player"))
        {
            clipPlayed = true;
            AudioSource.PlayClipAtPoint(sfx, player.transform.position, 0.2f);
        }
    }
}
