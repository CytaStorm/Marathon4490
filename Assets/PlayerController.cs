using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _initialJump;
    [SerializeField] float _jumpStrength;
    [SerializeField] float _gravity;
    [SerializeField] float _terminalVelocity;

    [SerializeField] float _buttonAccel;
    [SerializeField] float _xDrag;

    private Rigidbody _rigidBody;
    private Vector3 _direction;

    [SerializeField] float _maxJumpTime = 1f;
    private float _jumpTimer = 0f;
    private bool _jumpInputOn = false;
    private bool _onGround = true;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _rigidBody.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        // HORIZONTAL MOTION
        float acceleration = _direction.x * 10;

        Vector3 velocity = _rigidBody.velocity;
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

        // VERTICAL MOTION
        if (_jumpInputOn)
         {
            // Apply velocity
            velocity.y += _jumpStrength * Time.deltaTime;

            // Jumptimer calculations
            _jumpTimer += Time.deltaTime;
            if (_jumpTimer > _maxJumpTime)
            {
                _jumpTimer = 0f;
                _jumpInputOn = false;
            }
        }

        // APPLY MOTION
        _rigidBody.velocity = velocity;

    }

    public void Move(InputAction.CallbackContext Move)
    {
        if (Move.performed)
        {
            float input = Move.ReadValue<float>();
            //print(input);

            _direction = new Vector3(input, 0, 0);
        }
        else if (Move.canceled)
        {
            _direction = Vector3.zero;
        }
                
    }

    public void Jump(InputAction.CallbackContext Jump)
    {
        if ((Jump.performed) && (_onGround))
        {
            Debug.Log("JUMP");
            Vector3 velocity = _rigidBody.velocity;
            velocity.y = _initialJump;
            _rigidBody.velocity = velocity;
            _jumpInputOn = true;

            /*
            Vector3 velocity = rigidBody.velocity;
            velocity.y = jumpStrength;
            rigidBody.velocity = velocity;
            */
        }
        else if (Jump.canceled)
        {
            _jumpInputOn = false;
            Debug.Log("JUMP off");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //TODO: check if collision is ground
        _onGround = true;
        print("ON Ground");
    }

    private void OnCollisionExit(Collision collision)
    {
        //TODO: check if collision is ground
        _onGround = false;
        print("OFF Ground");
    }



}
