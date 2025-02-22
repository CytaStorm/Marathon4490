using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField] float _gravity;
	private float _appliedGravity;

	[SerializeField] private AnimationCurve _gravityCurve;

	[SerializeField] float _xDrag;

	private Rigidbody _rigidBody;
	private Vector3 _direction;

	private bool _onGround = false;


    [SerializeField] private float jumpShortSpeed = 3f;   // Velocity for the lowest jump
    [SerializeField] private float jumpSpeed = 6f;          // Velocity for the highest jump

	bool jump = false;
	bool jumpCancel = false;

    // Start is called before the first frame update
    void Start()
	{
		_rigidBody = gameObject.GetComponent<Rigidbody>();
		_rigidBody.freezeRotation = true;
	}

	// Update is called once per frame
	void Update()
	{

		if (Input.GetButtonDown("Jump") && _onGround)
		{
			// Player starts pressing the button
			jump = true;
		}
		if (Input.GetButtonUp("Jump") && _onGround)
		{     // Player stops pressing the button
			jumpCancel = true;
		}

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
	}

	void FixedUpdate()
    {
        // Normal jump (full speed)
        if (jump)
        {
            _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, jumpSpeed, 0);
            jump = false;
        }
        // Cancel the jump when the button is no longer pressed
        if (jumpCancel)
        {
            if (_rigidBody.velocity.y > jumpShortSpeed)
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, jumpShortSpeed);
            jumpCancel = false;
        }
    }

	public void Move(InputAction.CallbackContext Move)
	{
		print("here");
		if (Move.performed)
		{
			float input = Move.ReadValue<float>();

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
			//Vector3 velocity = _rigidBody.velocity;
			//velocity.y = _initialJump;
			//_rigidBody.velocity = velocity;
			_onGround = false;
			jump = true;
			//set gravity lower
		}
		else if (Jump.canceled)
		{
			Debug.Log("JUMP off");
			jumpCancel = true;
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
