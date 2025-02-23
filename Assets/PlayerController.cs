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

	[SerializeField] float shotStrength;

    // Start is called before the first frame update
    void Start()
	{
		_rigidBody = gameObject.GetComponent<Rigidbody>();
		_rigidBody.freezeRotation = true;
	}

	// Update is called once per frame
	void Update()
	{
		// Jump detection
		if (Input.GetButtonDown("Jump") && _onGround)
		{
			// Player starts pressing the button
			jump = true;
		}
		if (Input.GetButtonUp("Jump") && _onGround)
		{     // Player stops pressing the button
			jumpCancel = true;
		}

		// Projectile launch code
		if (Input.GetMouseButtonDown(0))
		{
			// Apply force in the direction of the mouse
			float angle = GetAngleToMouse();
			Vector3 forceDirection = Vector3.right;
			float x = forceDirection.x;
			float y = forceDirection.y;
			// Multiply unit vector by rotation matrix
			forceDirection.x = Mathf.Cos(angle) * x - Mathf.Sin(angle) * y;
			forceDirection.y = Mathf.Sin(angle) * x + Mathf.Cos(angle) * y;
			// Multiply direction vector by force scalar
			forceDirection *= shotStrength;

			_rigidBody.AddForce(forceDirection);
		}

		// Horizontal motion
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

	private float GetAngleToMouse()
	{
        Vector3 worldLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 localLookVect = worldLocation - transform.position;
        float angleOfRot = Mathf.Atan2(localLookVect.y, localLookVect.x);

        angleOfRot *= Mathf.Rad2Deg;
		return angleOfRot;
        //return Quaternion.Euler(0, 0, angleOfRot);
    }

}
