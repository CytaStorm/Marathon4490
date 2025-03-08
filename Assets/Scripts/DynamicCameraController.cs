using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCameraController : MonoBehaviour
{
    public GameObject target;
    
    public float xBuffer = 1;
    public float yBuffer = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check to make sure player is inside buffer
        float targetX = target.transform.position.x;
        float thisX = transform.position.x;

        float xOffset = targetX - thisX;

        if (Mathf.Abs(xOffset) >= xBuffer)
        {
            // Follow player
            Vector3 newPosition = target.transform.position;
            newPosition.z = this.transform.position.z;
            if (xOffset > 0)
            {
                newPosition.x -= xBuffer;
            }
            else
            {
                newPosition.x += xBuffer;
            }
            
            this.transform.position = newPosition;
        }
        
    }
}
