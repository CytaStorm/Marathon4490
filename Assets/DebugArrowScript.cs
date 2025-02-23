using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugArrowScript : MonoBehaviour
{
    [SerializeField] GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = target.transform.position;
        newPos.z = -1;
        transform.position = newPos;

        Vector3 worldLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 localLookVect = worldLocation - transform.position;
        float angleOfRot = Mathf.Atan2(localLookVect.y, localLookVect.x);

        angleOfRot *= Mathf.Rad2Deg; // don’t forget to convert!!
        transform.rotation = Quaternion.Euler(0, 0, angleOfRot);

    }
}
