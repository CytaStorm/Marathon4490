using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelDebugArrowScript : MonoBehaviour
{
    [SerializeField] GameObject target;
    PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = target.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
