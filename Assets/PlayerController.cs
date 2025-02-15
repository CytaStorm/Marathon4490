using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpStrength;
    [SerializeField] float gravity;
    [SerializeField] float terminalVelocity;

    [SerializeField] float _buttonAccel;
    [SerializeField] float _xDrag;

    private Rigidbody rigidBody;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        float acceleration = direction.x * 10;
        print(acceleration);

        Vector3 velocity = rigidBody.velocity;
        velocity.x += (acceleration * Time.deltaTime);

        if (velocity.x > 0)
        {
            //apply xdrag
            velocity.x = Mathf.Clamp(velocity.x - (_xDrag * Time.deltaTime), 0, float.MaxValue);
        }
        else if (velocity.x < 0)
        {
            velocity.x = Mathf.Clamp(velocity.x + (_xDrag * Time.deltaTime), float.MinValue, 0);
        }
        rigidBody.velocity = velocity;
    }

    public void Move(InputAction.CallbackContext Move)
    {
        if (Move.performed)
        {
            float input = Move.ReadValue<float>();
            print(input);

            direction = new Vector3(input, 0, 0);
        }
        else if (Move.canceled)
        {
            direction = Vector3.zero;
        }
                
    }

    public void Jump(InputAction.CallbackContext Jump)
    {
        if (Jump.performed)
        {
            Debug.Log("JUMP");
            Vector3 velocity = rigidBody.velocity;
            velocity.y = jumpStrength;
            rigidBody.velocity = velocity;
        }
    }

    
}
