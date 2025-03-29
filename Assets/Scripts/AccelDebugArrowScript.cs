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
        float angleOfRot = Mathf.Atan2(controller.Acceleration.y, controller.Acceleration.x);
        transform.rotation = Quaternion.Euler(0, 0, angleOfRot);

        Vector3 position = target.transform.position;
        position.z = -5;
        transform.position = position;
    }
}
