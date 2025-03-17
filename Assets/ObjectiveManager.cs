using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{

    [SerializeField]
    public int totalObjectives;

    [SerializeField]
    private int completedObjectives;

    private bool levelComplete = false;

    public int CompletedObjectives
    {
        get { return completedObjectives; }
        set { completedObjectives = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (completedObjectives == totalObjectives && !levelComplete)
        {
            LevelComplete();
        }
    }

    void LevelComplete()
    {
        Debug.Log("Level Complete!");
        levelComplete = true;

        // Here you could trigger the next level pal
    }
}
