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

        if (_jumpTimer < _maxJumpTime && !_onGround)
        {
            //apply gravity along curve
            _gravity = Mathf.Lerp(-9.8f, 9.8f, _jumpTimer/_maxJumpTime);
            _jumpTimer = Mathf.Clamp(_jumpTimer + Time.deltaTime, 0, _maxJumpTime);
        }
        else
        {
            _gravity = 9.8f;
        }
        //apply gravity
        _rigidBody.AddForce(Vector3.down * _gravity, ForceMode.Acceleration);
        

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

        // APPLY MOTION
        _rigidBody.velocity = velocity;
        
        print(_jumpTimer);
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

    private void EndJump()
    {
        _jumpInputOn = false;
        _jumpTimer = 0f;
        _rigidBody.AddForce(new Vector3(0, -160, 0), ForceMode.Acceleration);
    }

    public void Jump(InputAction.CallbackContext Jump)
    {
        if ((Jump.performed) && (_onGround))
        {
            Debug.Log("JUMP");
            //Vector3 velocity = _rigidBody.velocity;
            //velocity.y = _initialJump;
            //_rigidBody.velocity = velocity;
            _onGround = false;
            //set gravity lower
        }
        else if (Jump.canceled)
        {
            Debug.Log("JUMP off");
            //Vector3 velocity = _rigidBody.velocity;
            //velocity.y /= 2;
            //_rigidBody.velocity = velocity;
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
