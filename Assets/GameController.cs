using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;

    Transform startPosition;

    [SerializeField]
    PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
        startPosition.position = playerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.dying == true)
        {
            Respawn();
            playerController.dying = false;
        }
    }

    void Respawn()
    {
        if (playerTransform != null)
        {
            playerTransform.position = startPosition.position;
        }
        else
        {
            Debug.Log("Missing Player Transform");
        }
    }
}
