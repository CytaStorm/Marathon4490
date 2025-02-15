using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    private Rigidbody rigidBody;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.velocity = direction * speed;
    }

    public void Move(InputAction.CallbackContext Move)
    {
        if (!Move.performed) return;
        float input = Move.ReadValue<float>();
        print(input);

        direction = new Vector3(input, 0, 0);        
    }
}
